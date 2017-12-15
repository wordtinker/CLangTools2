using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shared
{
    namespace Tools
    {
        public class Math
        {
            public static double? TakePercent(int? dividend, int? divisor)
            {
                if (divisor == null || divisor == 0 || dividend == null)
                {
                    return null;
                }

                return (double)dividend / divisor;
            }
        } 
        public class IO
        {
            /// <summary>
            /// Provides list of directory names in the given directory.
            /// </summary>
            /// <param name="dir"></param>
            /// <param name="foldersInDir"></param>
            /// <returns></returns>
            public static bool ListDirectories(string dir, out List<string> inDir)
            {
                try
                {
                    inDir = Directory.GetDirectories(dir).Select(Path.GetFileName).ToList();
                    return true;
                }
                catch (Exception)
                {
                    inDir = new List<string>();
                    return false;
                }
            }
            /// <summary>
            /// Provides list of file names in the given directory.
            /// </summary>
            /// <param name="dir"></param>
            /// <param name="filesInDir"></param>
            /// <param name="filter"></param>
            /// <returns></returns>
            public static bool ListFiles(string dir, out List<string> filesInDir, string filter = "*.txt")
            {
                try
                {
                    filesInDir = Directory.GetFiles(dir, filter).Select(Path.GetFileName).ToList();
                    return true;
                }
                catch (Exception)
                {
                    filesInDir = new List<string>();
                    return false;
                }
            }
            /// <summary>
            /// Combines an array of string into a path;
            /// </summary>
            /// <param name="pathes"></param>
            /// <returns></returns>
            public static bool CombinePath(out string path, params string[] pathes)
            {
                try
                {
                    path = Path.Combine(pathes);
                    return true;
                }
                catch (Exception)
                {
                    path = null;
                    return false;
                }
            }
            /// <summary>
            /// Provides the text contents of the file.
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="content"></param>
            /// <returns></returns>
            public static bool ReadAllText(string filePath, out string content)
            {
                try
                {
                    content = File.ReadAllText(filePath, Encoding.UTF8);
                    return true;
                }
                catch (Exception)
                {
                    content = null;
                    return false;
                }
            }
            /// <summary>
            /// Provides the text contents of the file by line.
            /// </summary>
            public static bool ReadAllLines(string filePath, out string[] content)
            {
                try
                {
                    content = File.ReadAllLines(filePath, Encoding.UTF8);
                    return true;
                }
                catch (Exception)
                {
                    content = null;
                    return false;
                }
            }
            /// <summary>
            /// Changes extensions of the fileName.
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="newExt"></param>
            /// <returns></returns>
            public static bool ChangeExtension(string fileName, string newExt, out string newName)
            {
                try
                {
                    newName =  Path.ChangeExtension(fileName, newExt);
                    return true;
                }
                catch (Exception)
                {
                    newName = null;
                    return false;
                }
            }
            /// <summary>
            /// Appends the string to the file.
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="content"></param>
            /// <returns></returns>
            public static bool AppendToFile(string filePath, string content)
            {
                try
                {
                    File.AppendAllText(filePath, content);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            /// <summary>
            /// Deletes the file.
            /// </summary>
            /// <param name="fileName"></param>
            public static bool DeleteFile(string fileName)
            {
                try
                {
                    File.Delete(fileName);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
