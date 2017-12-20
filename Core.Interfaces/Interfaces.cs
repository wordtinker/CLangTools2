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
    /// <summary>
    /// Composite item that holds infromation about children items
    /// as wes as basic stats of the item.
    /// </summary>
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
    /// <summary>
    /// Specialized item that represents an object that might be a word.
    /// </summary>
    public interface IToken : IItem
    {
        TokenType Type { get; set; }
        ITokenStats Stats { get; set; }
    }
    /// <summary>
    /// Provides information about the bound word.
    /// </summary>
    public interface ITokenStats
    {
        string LWord { get; set; } // lower case Word
        int Count { get; set; } // number of occurences in a text
        Klass Know { get; set; }
    }
    /// <summary>
    /// Provides the means of analyzing structured document
    /// and marking the stats of the contained words.
    /// </summary>
    public interface ILexer
    {
        string Extension { get; }
        void LoadPlugin(string content);
        void LoadDictionary(string content);
        void ExpandDictionary();
        IItem AnalyzeText(IItem root);
    }
    /// <summary>
    /// Provides means of transfroming array of strings into
    /// structured document with paragraphs and vice versa.
    /// </summary>
    public interface ITreeBuilder
    {
        IItem Compose(string[] content);
        string Decompose(IItem root,
            Func<IItem, string> paragraphDecorator, Func<IToken, string> wordDecorator);
    }
}
