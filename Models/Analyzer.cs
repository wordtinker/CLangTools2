using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Models.Interfaces;
using Shared.Tools;
using Storage.Interfaces;

namespace Models
{
    /// <summary>
    /// Class that combines needful classes,
    /// makes analysis and stores the new data.
    /// </summary>
    public class Analyzer
    {
        private IStorage storage;
        private ITreeBuilder treeBuilder;
        private IProject project;
        private IEnumerable<IDict> dictionaries;
        private ILexer lexer;
        private Printer printer;

        public Analyzer(IStorage storage, ILexer lexer, ITreeBuilder treeBuilder)
        {
            this.storage = storage;
            this.lexer = lexer;
            this.treeBuilder = treeBuilder;
            this.printer = new Printer(treeBuilder);
            
        }
        internal void SetupProject(IProject project, IEnumerable<IDict> dictionaries)
        {
            this.project = project;
            this.dictionaries = dictionaries;
            // Remove old stats and words for project from DB.
            storage.RemoveProject(project.Parent.Language, project.Name);
            printer.LoadStyle(project.Parent.Language);
            PrepareLexer();
        }
        private void PrepareLexer()
        {
            // Load plugin into lexer if we have plugin
            string pluginDir = Config.StyleDirectoryPath;
            IO.CombinePath(out string pluginPath, pluginDir, project.Parent.Language);
            IO.ChangeExtension(pluginPath, lexer.Extension, out pluginPath); 
            if (IO.ReadAllText(pluginPath, out string jsonPluginContent))
            {
                lexer.LoadPlugin(jsonPluginContent);
            }
            // Load dictionaries
            foreach (IDict dic in dictionaries)
            {
                if (IO.ReadAllText(dic.FilePath, out string content))
                {
                    lexer.LoadDictionary(content);
                }
            }
            // Expand dictionary
            lexer.ExpandDictionary();
        }
        internal IFileStats AnalyzeFile(IFileStats fileStats)
        {
            if (IO.ReadAllLines(fileStats.FilePath, out string[] content))
            {
                // Build tree from text
                var tree = treeBuilder.Compose(content);
                // Get stats for every element of the tree
                tree = lexer.AnalyzeText(tree);
                fileStats.Size = tree.Size;
                fileStats.Known = tree.Known;
                fileStats.Maybe = tree.Maybe;
                // Produce new output page
                string outPath = printer.Print(tree, fileStats.FileName, fileStats.FilePath);
                fileStats.OutPath = outPath;
                // Update stats in the DB
                storage.CommitStats(fileStats.FileName, fileStats.FilePath,
                    project.Parent.Language, project.Name,
                    fileStats.Size.GetValueOrDefault(), fileStats.Known.GetValueOrDefault(),
                    fileStats.Maybe.GetValueOrDefault(), fileStats.Unknown.GetValueOrDefault());
                // Add new list of unknown words into DB
                var newWords = tree.Tokens
                                .Where(t => t.Stats?.Know == Klass.UNKNOWN)
                                .Select(t => t.Stats)
                                .Distinct().Select(ts => (ts.LWord, ts.Count));
                storage.CommitWords(fileStats.FilePath, newWords);
            }
            return fileStats;
        }
    }
}
