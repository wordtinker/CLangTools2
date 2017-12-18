using System;
using System.Text;
using Core.Interfaces;

namespace Core
{
    public class TreeBuilder : ITreeBuilder
    {
        public IItem Compose(string[] content)
        {
            // Build composite tree
            TokenizerWithStats tknz = new TokenizerWithStats();
            Document root = new Document();
            foreach (string paragraph in content)
            {
                Item para = new Paragraph();
                root.AddItem(para);
                foreach (Token token in tknz.Enumerate(paragraph))
                {
                    para.AddItem(token);
                }
            }
            return root;
        }

        public string Decompose(IItem root, Func<IItem, string> rootDecorator,
            Func<IItem, string> paragraphDecorator, Func<IItem, string> wordDecorator)
        {
            // TODO !!!
            StringBuilder sb = new StringBuilder();
            //rootDecorator.Invoke(root);
            //sb.Append("<html><head><meta charset='utf-8'>");
            //sb.Append(string.Format("<title>{0}</title></head>", root.Name));
            //sb.Append("<body>");
            //sb.Append("<article>");
            //foreach (Paragraph p in root.Items.OfType<Paragraph>())
            //{
            //    sb.Append("<p>");
            //    foreach (Token tkn in p.Tokens)
            //    {
            //        if (tkn.Type == TokenType.WORD)
            //        {
            //            string tag;
            //            if (tkn.Stats?.Know == Klass.UNKNOWN)
            //            {
            //                tag = string.Format(
            //                    "<span class={0}>{1}</span><sub>{2}</sub>",
            //                    tkn.Stats.Know,
            //                    tkn.Name,
            //                    tkn.Stats.Count);
            //            }
            //            else
            //            {
            //                tag = string.Format(
            //                    "<span class={0}>{1}</span>",
            //                    tkn.Stats?.Know,
            //                    tkn.Name);
            //            }
            //            sb.Append(tag);
            //        }
            //        else
            //        {
            //            sb.Append(tkn.Name);
            //        }
            //    }
            //    sb.Append("</p>");
            //}

            //sb.Append("</article>");
            //sb.Append("<style>");
            
            return sb.ToString();
        }
    }
}
