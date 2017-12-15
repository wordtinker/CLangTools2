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

        public Analyzer(IProject project, IEnumerable<IDict> dictionaries, ILexer lexer)
        {
            this.project = project;
            this.dictionaries = dictionaries;
            this.lexer = lexer;
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

        internal IFileStats AnalyzeFile(FileStats fileStats)
        {
            // TODO
            //if (docRoot != null)
            //{
            //    // Produce new output page
            //    string outPath = printer.Print(docRoot);
            //    file.OutPath = outPath;
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
            //}
            return null;
        }
        // TODO move
        //private Document AnalyzeFile(FileStats file)
        //{
        //    if (IO.ReadAllLines(file.FilePath, out string[] content))
        //    {
        //        // Build composite tree
        //        TokenizerWithStats tknz = new TokenizerWithStats();
        //        Document root = new Document { Name = file.FileName };
        //        foreach (string paragraph in content)
        //        {
        //            Item para = new Paragraph();
        //            root.AddItem(para);
        //            foreach (Token token in tknz.Enumerate(paragraph))
        //            {
        //                para.AddItem(token);
        //            }
        //        }
        //        return lexer.AnalyzeText(root);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
