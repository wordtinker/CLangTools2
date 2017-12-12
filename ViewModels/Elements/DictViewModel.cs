using Models.Interfaces;

namespace ViewModels
{
    public class DictViewModel
    {
        private IDict dictionary;

        // Properties
        public string FileName { get => dictionary.FileName; }
        public string DictType { get => dictionary.DictType.ToString(); }
        public string FilePath { get => dictionary.FilePath; }
        // Constructors
        public DictViewModel(IDict dictionary)
        {
            this.dictionary = dictionary;
        }
        // Equals override. For proper usage inside collections
        public override bool Equals(object obj)
        {
            if (obj is DictViewModel other)
            {
                return this.dictionary.Equals(other.dictionary);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return dictionary.GetHashCode();
        }
    }
}
