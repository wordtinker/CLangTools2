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
            // Config model properties.
            Config.StyleDirectoryPath = stylePath;
            Config.CommonDictionaryName = commonDicName;
            // Bind everything within container.
            Container = new UnityContainer();
            Container.RegisterInstance<IStorage>(new SQLiteStorage(workingDirectory));
            Container.RegisterInstance<ITreeBuilder>(new TreeBuilder());
            Container.RegisterType<ILexer, Lexer>();
            Container.RegisterType<Analyzer>();
            Container.RegisterInstance<IDataProvider>(
                new Model(Container.Resolve<IStorage>(), () => Container.Resolve<Analyzer>()));
            Container.RegisterType<IValidate, LingvaValidator>();
        }
    }
}
