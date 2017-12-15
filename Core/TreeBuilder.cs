using Core.Interfaces;

namespace Core
{
    public class TreeBuilder : ITreeBuilder
    {
        public IItem Build(string name, string[] content)
        {
            // Build composite tree
            TokenizerWithStats tknz = new TokenizerWithStats();
            Document root = new Document { Name = name };
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
    }
}
