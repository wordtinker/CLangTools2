﻿using System;

namespace ViewModels.Interfaces
{
    // TODO check usage
    public interface IUIBaseService
    {
        //void ShowMessage(string message);
        //bool Confirm(string message);
        //void BeginInvoke(Action method);
    }
    // TODO check usage
    public interface IUIMainWindowService : IUIBaseService
    {
        //string CommonDictionaryName { get; }
        //string AppDir { get; }
        //string AppName { get; }
        //string CorpusDir { get; }
        //string DicDir { get; }
        //string OutDir { get; }
        void ShowHelp();
        void Shutdown();
    }
}
