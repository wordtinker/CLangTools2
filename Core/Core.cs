using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Core
{
    // TODO !!!
    public class StubLexer : ILexer
    {
        public string Extension => throw new NotImplementedException();

        public void ExpandDictionary()
        {
            throw new NotImplementedException();
        }

        public void LoadDictionary(string content)
        {
            throw new NotImplementedException();
        }

        public void LoadPlugin(string content)
        {
            throw new NotImplementedException();
        }
    }

}
