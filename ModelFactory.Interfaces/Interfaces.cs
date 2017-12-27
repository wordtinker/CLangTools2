using Models.Interfaces;

namespace ModelFactory.Interfaces
{
    public interface IModelFactory
    {
        IDataProvider Model { get; }
        IValidate Validator { get; }
    }
}
