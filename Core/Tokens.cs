using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Abstact composite class that represents a node of document tree.
    /// </summary>
    internal abstract class Item : IItem
    {
        // List of subnodes.
        protected List<IItem> items = new List<IItem>();
        /// <summary>
        /// Name of the node
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Number of word tokens in the node and subnodes.
        /// </summary>
        public virtual int Size => Tokens.Sum(t => t.Size);
        /// <summary>
        /// Number of known word tokens in the node.
        /// </summary>
        public virtual int Known  => Tokens.Sum(t => t.Known);
        /// <summary>
        /// Number of words that might be known.
        /// </summary>
        public virtual int Maybe => Tokens.Sum(t => t.Maybe);
        /// <summary>
        /// Enumerable of word tokens of the node and subnodes.
        /// </summary>
        public abstract IEnumerable<IToken> Tokens { get; }
        /// <summary>
        /// Enumerable of subnodes.
        /// </summary>
        public virtual IEnumerable<IItem> Items => items;
        /// <summary>
        /// Adds subnode.
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddItem(IItem item)
        {
            items.Add(item);
        }
    }
    /// <summary>
    /// Word token node.
    /// </summary>
    internal class Token : Item, IToken
    {
        public TokenType Type { get; set; }
        public ITokenStats Stats { get; set; }

        // Node implementation.
        public override int Size => this.Type == TokenType.WORD ? 1 : 0;
        public override int Known => this.Stats?.Know == Klass.KNOWN ? 1 : 0;
        public override int Maybe => this.Stats?.Know == Klass.MAYBE ? 1 : 0;
        // Token has no subnodes.
        public override IEnumerable<IToken> Tokens
        {
            get
            {
                // Empty list
                yield break;
            }
        }
        public override IEnumerable<IItem> Items
        {
            get
            {
                // Empty list
                yield break;
            }
        }
        public override void AddItem(IItem item) { /* Do nothing */ }
    }
    /// <summary>
    /// Paragraph node.
    /// </summary>
    internal class Paragraph : Item
    {
        public override IEnumerable<IToken> Tokens => this.items.OfType<Token>();
    }
    /// <summary>
    /// Document node.
    /// </summary>
    internal class Document : Item
    {
        public override IEnumerable<IToken> Tokens
        {
            get
            {
                // Prevent cyclic ref and filter errs. Document should contain only paragraphs.
                foreach (var p in this.items.OfType<Paragraph>())
                {
                    foreach (Token token in p.Tokens)
                    {
                        yield return token;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Object that holds stats for one word.
    /// This object can be shared among several word tokens.
    /// </summary>
    internal class TokenStats : ITokenStats
    {
        public string LWord { get; set; }
        public int Count { get; set; }
        public Klass Know { get; set; }
    }
    /// <summary>
    /// Flyweight factory that provides shared TokenStats object for a given word.
    /// </summary>
    internal class TokenStatsFlyweightFactory
    {
        private Dictionary<string, ITokenStats> uniqueWords = new Dictionary<string, ITokenStats>();

        public ITokenStats GetTokenStats(string word)
        {
            ITokenStats tkn;
            string lWord = word.ToLower();
            if (uniqueWords.ContainsKey(lWord))
            {
                tkn = uniqueWords[lWord];
                tkn.Count += 1;
            }
            else
            {
                tkn = new TokenStats
                {
                    LWord = lWord,
                    Know = Klass.UNDECIDED,
                    Count = 1
                };
                uniqueWords.Add(lWord, tkn);
            }
            return tkn;
        }
    }
}
