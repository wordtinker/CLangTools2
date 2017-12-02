using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class Lingva : ILingva
    {
        // Properties
        public string Language { get; set; }
        public string Folder { get; set; }
    }

    public class StubModel : IDataProvider
    {
        // TODO STUB
        public IEnumerable<ILingva> GetLanguages()
        {
            yield return new Lingva { Language = "English", Folder = "/test" };
            yield return new Lingva { Language = "Hebrew", Folder = "/test" };
        }

        public IEnumerable<string> GetProjectsForLanguage(ILingva lingva)
        {
            yield return "sdfsdf.txt" + lingva.Language;
            yield return "sdfsdf2.txt" + lingva.Language;
        }
    }
}
