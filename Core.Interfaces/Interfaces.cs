using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
    /// <summary>
    /// Word token type.
    /// </summary>
    public enum TokenType
    {
        WORD,
        NONWORD
    }
    /// <summary>
    /// Word token knowledge type.
    /// </summary>
    public enum Klass
    {
        UNDECIDED,
        KNOWN,
        MAYBE,
        UNKNOWN
    }
    public interface IItem
    {
        IEnumerable<IItem> Items { get; }
        int Known { get; }
        int Maybe { get; }
        string Content { get; set; }
        int Size { get; }
        IEnumerable<IToken> Tokens { get; }

        void AddItem(IItem item);
    }
    public interface IToken : IItem
    {
        TokenType Type { get; set; }
        ITokenStats Stats { get; set; }
    }

    public interface ITokenStats
    {
        string LWord { get; set; } // lower case Word
        int Count { get; set; } // number of occurences in a text
        Klass Know { get; set; }
    }

    public interface ILexer
    {
        string Extension { get; }
        void LoadPlugin(string content);
        void LoadDictionary(string content);
        void ExpandDictionary();
        IItem AnalyzeText(IItem root);
    }
    public interface ITreeBuilder
    {
        IItem Compose(string[] content);
        string Decompose(IItem root,
            Func<IItem, string> paragraphDecorator, Func<IToken, string> wordDecorator);
    }
}
