using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface ILingva
    {
        string Language { get; set; }
        string Folder { get; set; }
    }
    // TODO
    public interface IDataProvider
    {
        IEnumerable<ILingva> GetLanguages();
        IEnumerable<string> GetProjectsForLanguage(ILingva lingva);
    }
}
