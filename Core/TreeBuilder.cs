using System;
using System.Linq;
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

        public string Decompose(IItem root,
            Func<IItem, string> paragraphDecorator, Func<IToken, string> wordDecorator)
        {
            foreach (Token token in root.Items.OfType<Token>())
            {
                token.Content = wordDecorator.Invoke(token);
            }
            StringBuilder sb = new StringBuilder();
            foreach (Paragraph p in root.Items.OfType<Paragraph>())
            {
                sb.Append(paragraphDecorator.Invoke(p));
            }
            return sb.ToString();
        }
    }
}
