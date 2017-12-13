using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shared
{
    namespace Tools
    {
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
        }
    }
}
