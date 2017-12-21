using Core.Interfaces;
using Shared.Tools;
using System.Text;

namespace Models
{
    /// <summary>
    /// Manages the printing of the output page.
    /// </summary>
    internal class Printer
    {
        private const string STYLEEXT = ".css";
        private string css;

        /// <summary>
        /// Finds CSS file for a given language and saves it
        /// for later use in printing.
        /// </summary>
        /// <param name="language"></param>
        internal void LoadStyle(string language)
        {
            // Load CSS file
            string cssDir = Config.StyleDirectoryPath;
            IO.CombinePath(out string cssPath, cssDir, language);
            IO.ChangeExtension(cssPath, STYLEEXT, out cssPath);
            IO.ReadAllText(cssPath, out css);
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
            string HTML = ToHTML(root, header);
            IO.ChangeExtension(filePath, Config.OutExtension, out string outPath);
            return IO.SaveFile(outPath, HTML) ? outPath : null;
        }
        /// <summary>
        /// Makes an HTML page out of IITem.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private string ToHTML(IItem root, string header)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'>");
            sb.Append(string.Format("<title>{0}</title></head>", header));
            sb.Append("<body><article>");
            sb.Append(ModelFactory.TreeBuilder.Decompose(root,
                p => $"<p>{p.Content}</p>",
                w =>
                {
                    if (w.Type == TokenType.WORD)
                    {
                        string tag;
                        if (w.Stats?.Know == Klass.UNKNOWN)
                        {
                            tag = string.Format(
                                "<span class={0}>{1}</span><sub>{2}</sub>",
                                w.Stats.Know,
                                w.Content,
                                w.Stats.Count);
                        }
                        else
                        {
                            tag = string.Format(
                                "<span class={0}>{1}</span>",
                                w.Stats?.Know,
                                w.Content);
                        }
                        return tag;
                    }
                    else
                    {
                        return w.Content;
                    }
                }));

            sb.Append("</article><style>");
            if (css != null)
            {
                sb.Append(css);
            }
            else
            {
                // Use default style
                sb.Append(@"
    body {font-family:sans-serif;
            line-height: 1.5;}
    span.KNOWN
            {background-color: white;
            font-weight: normal;font-style: normal;
            border-bottom: 3px solid green;}
    span.MAYBE
            {background-color: white;
            font-weight: normal;font-style: normal;
            border-bottom: 3px solid yellowgreen;}
                ");
            }
            sb.Append("</style></body></html>");
            return sb.ToString();
        }
    }
}
