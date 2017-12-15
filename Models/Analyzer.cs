using System.Collections.Generic;
using Core.Interfaces;
using Models.Interfaces;
using Shared.Tools;

namespace Models
{
    class Analyzer
    {
        private IProject project;
        private IEnumerable<IDict> dictionaries;
        private ILexer lexer;

        public Analyzer(IProject project, IEnumerable<IDict> dictionaries )
        {
            this.project = project;
            this.dictionaries = dictionaries;
            this.lexer = ModelFactory.Lexer;
            // TODO
            //printer.LoadStyle();
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
                var tree = ModelFactory.TreeBuilder.Build(fileStats.FileName, content);
                // Get stats for every element of the tree
                tree = lexer.AnalyzeText(tree);
                // TODO !!!
                // Produce new output page
                //string outPath = printer.Print(docRoot);
                //file.OutPath = outPath;
                //    // Update stats in the DB
                //    storage.UpdateStats(file);
                //    // Commit changes to DB
                //    storage.CommitStats();
                //    // Add new list of unknown words into DB
                //    var newWords = docRoot.Tokens
                //                    .Where(t => t.Stats?.Know == Klass.UNKNOWN)
                //                    .Select(t => t.Stats)
                //                    .Distinct();

                //    // commit prevents memory leak
                //    storage.UpdateWords(file.FilePath, newWords);
                //    storage.CommitWords();
            }
            // TODO
            return fileStats;
        }
    }
}
