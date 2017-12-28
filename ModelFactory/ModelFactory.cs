using Core;
using Core.Interfaces;
using Models;
using Models.Interfaces;
using Storage;
using Storage.Interfaces;
using System;
using Unity;

namespace ModelFactory
{
    /// <summary>
    /// Container that provides model and validator classes for
    /// an app, and some internal classes.
    /// </summary>
    public class ModelFactory
    {
        public IUnityContainer Container { get; private set; }

        public ModelFactory(string workingDirectory, string stylePath, string commonDicName)
        {
            if (workingDirectory == null) throw new ArgumentNullException("WorkingDirectory", "Working directory is not set");
            IStorage storage = new SQLiteStorage(workingDirectory);
            Config.StyleDirectoryPath = stylePath;
            Config.CommonDictionaryName = commonDicName;
            Container = new UnityContainer();
            // TODO
            Container.RegisterInstance<IDataProvider>(new Model(storage, () => new Lexer(), () => TreeBuilder));
            Container.RegisterInstance<IValidate>(new LingvaValidator(storage));
        }
        private ITreeBuilder TreeBuilder { get; } = new TreeBuilder();
    }
}
