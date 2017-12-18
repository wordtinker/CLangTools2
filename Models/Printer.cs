using Core.Interfaces;
using Shared.Tools;

namespace Models
{
    /// <summary>
    /// Manages the printing of the output page.
    /// </summary>
    internal class Printer
    {
        private const string STYLEEXT = ".css";

        private string css;

        internal void LoadStyle(string language)
        {
            // TODO ctor?
            // Load CSS file
            string cssDir = Config.StyleDirectoryPath;
            IO.CombinePath(out string cssPath, cssDir, language);
            IO.ChangeExtension(cssPath, STYLEEXT, out cssPath);
            IO.ReadAllText(cssPath, out string css);
        }
        /// <summary>
        /// Creates marked up output file for a given Item and
        /// return path of the created file.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        internal string Print(IItem root, string header, string filePath)
        {
            // Get the HTML and save
            // TODO
            string HTML = ModelFactory.TreeBuilder.Decompose(root, doc => "", p => "", w => "");
            IO.ChangeExtension(filePath, Config.OutExtension, out string outPath);
            return IO.SaveFile(outPath, HTML) ? outPath : null;
        }
    }
}
