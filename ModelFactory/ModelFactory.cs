using Core;
using Core.Interfaces;
using ModelFactory.Interfaces;
using Models;
using Models.Interfaces;
using Storage;
using Storage.Interfaces;
using System;

namespace ModelFactory
{
    /// <summary>
    /// Container that provides model and validator classes for
    /// an app, and some internal classes.
    /// </summary>
    public class ModelFactory : IModelFactory
    {
        private IDataProvider dataProvider;
        private IValidate validator;
        public ModelFactory(string workingDirectory, string stylePath, string commonDicName)
        {
            if (workingDirectory == null) throw new ArgumentNullException("WorkingDirectory", "Working directory is not set");
            IStorage storage = new SQLiteStorage(workingDirectory);
            Config.StyleDirectoryPath = stylePath;
            Config.CommonDictionaryName = commonDicName;
            dataProvider = new Model(storage, () => new Lexer(), () => TreeBuilder);
            validator = new LingvaValidator(storage);
        }
        public IDataProvider Model => dataProvider;
        public IValidate Validator => validator;
        private ITreeBuilder TreeBuilder { get; } = new TreeBuilder();
    }
}
