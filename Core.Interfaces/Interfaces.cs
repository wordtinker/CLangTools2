namespace Core.Interfaces
{
    public interface ILexer
    {
        string Extension { get; }
        void LoadPlugin(string content);
        void LoadDictionary(string content);
        void ExpandDictionary();
    }
}
