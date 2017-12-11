using Prism.Logging;
using System;
using System.IO;

namespace LangTools
{
    public class SimpleLogger : ILoggerFacade
    {
        private const string fileName = "Log.txt";
        private string path;
        public SimpleLogger(string folder)
        {
            path = Path.Combine(folder, fileName);
        }
        public void Log(string message, Category category, Priority priority)
        {
            string messageToLog =
                String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "{1}: {2}. Priority: {3}. Timestamp:{0:u}.\n",
                    DateTime.Now,
                    category.ToString().ToUpperInvariant(),
                    message,
                    priority.ToString());
            File.AppendAllText(path, messageToLog);
        }
    }
}
