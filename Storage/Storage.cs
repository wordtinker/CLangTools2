using Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Storage
{

    public class SQLiteStorage : IStorage
    {
        private const string dbFile = "lt.db";
        // DB connection
        private SQLiteConnection dbConn;

        public SQLiteStorage(string directory)
        {
            string dbFileName = Path.Combine(directory, dbFile);
            string connString = string.Format("Data Source={0};Version=3;foreign keys=True;", dbFileName);
            dbConn = new SQLiteConnection(connString);
            dbConn.Open();
            InitializeTables();
        }
        public void Close()
        {
            dbConn.Close();
        }

        private void InitializeTables()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Languages(lang TEXT PRIMARY KEY, directory TEXT)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS Files(" +
                "name TEXT, path TEXT PRIMARY KEY, " +
                "lang TEXT REFERENCES Languages(lang) ON DELETE CASCADE, project TEXT," +
                " size INTEGER, known INTEGER," +
                "maybe INTEGER, unknown INTEGER)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteNonQuery();
            }

            // No ON DELETE CASCADE for file external key
            // deletion of single file will be too slow.
            sql = "CREATE TABLE IF NOT EXISTS Words(" +
                "word TEXT, file TEXT, quantity INTEGER)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Adds new language and it's folder to DB.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void AddLanguage(string name, string folder)
        {
            string sql = "INSERT INTO Languages VALUES(@lang, @directory)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                SQLiteParameter param = new SQLiteParameter("@lang")
                {
                    Value = name,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@directory")
                {
                    Value = folder,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Provides list of pairs: language, project folder.
        /// </summary>
        /// <returns>DataTable with language, project folder.</returns>
        public IEnumerable<(string name, string folder)> GetLanguages()
        {
            string sql = "SELECT lang, directory FROM Languages";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetString(1));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Removes the stats of the given language from DB.
        /// </summary>
        /// <param name="language"></param>
        public void RemoveLanguage(string name)
        {
            SQLiteParameter param = new SQLiteParameter("@lang")
            {
                Value = name,
                DbType = System.Data.DbType.String
            };

            using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
            {
                cmd.Parameters.Add(param);
                // Delete all words from previous analysis.
                cmd.CommandText = "DELETE FROM Words WHERE file IN " +
                    "(SELECT path FROM Files WHERE lang=@lang)";
                cmd.ExecuteNonQuery();
                // Delete language and files.
                cmd.CommandText = "DELETE FROM Languages WHERE lang=@lang";
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Return projects for chosen language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public IEnumerable<string> GetProjects(string language)
        {
            string sql = "SELECT project FROM Files WHERE lang=@lang GROUP BY project";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                SQLiteParameter param = new SQLiteParameter("@lang")
                {
                    Value = language,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return dr.GetString(0);
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Removes the project from DB.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="project"></param>
        public void RemoveProject(string language, string project)
        {
            SQLiteParameter langParam = new SQLiteParameter("@lang")
            {
                Value = language,
                DbType = System.Data.DbType.String
            };

            SQLiteParameter projectParam = new SQLiteParameter("@project")
            {
                Value = project,
                DbType = System.Data.DbType.String
            };

            using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
            {
                cmd.Parameters.Add(langParam);
                cmd.Parameters.Add(projectParam);
                // Delete all words from previous analysis.
                cmd.CommandText = "DELETE FROM Words WHERE file IN " +
                    "(SELECT path FROM Files WHERE lang=@lang AND project=@project)";
                cmd.ExecuteNonQuery();
                // Delete all stats from previous analysis
                cmd.CommandText = "DELETE FROM Files WHERE lang=@lang AND project=@project";
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Returns list of fileStats for given language and project.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public IEnumerable<(string name, string path, int size, int known,
            int maybe, int unknown)> GetFileStats(string language, string project)
        {
            SQLiteParameter langParam = new SQLiteParameter("@lang")
            {
                Value = language,
                DbType = System.Data.DbType.String
            };

            SQLiteParameter projectParam = new SQLiteParameter("@project")
            {
                Value = project,
                DbType = System.Data.DbType.String
            };

            string sql = "SELECT name, path, size, known, maybe, unknown FROM Files " +
                "WHERE lang=@lang AND project=@project";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.Parameters.Add(langParam);
                cmd.Parameters.Add(projectParam);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetString(1), dr.GetInt32(2),
                        dr.GetInt32(3), dr.GetInt32(4), dr.GetInt32(5));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Removes the stats of the given file from the DB.
        /// </summary>
        /// <param name="filePath"></param>
        public void RemoveFileStats(string filePath)
        {
            SQLiteParameter path = new SQLiteParameter("@path")
            {
                Value = filePath,
                DbType = System.Data.DbType.String
            };

            using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
            {
                cmd.Parameters.Add(path);
                // Remove words related to file.
                cmd.CommandText = "DELETE FROM Words WHERE file=@path";
                cmd.ExecuteNonQuery();
                // Remove the stats for file
                cmd.CommandText = "DELETE FROM Files WHERE path=@path";
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Add new set of stats into DB.
        /// </summary>
        public void CommitStats(string name, string path, string lang, string project, int size, int known, int maybe, int unknown)
        {
            string sql = "INSERT OR REPLACE INTO Files " +
                "VALUES(@name, @path, @lang, @project, @size, @known, @maybe, @unknown)";

            using (SQLiteCommand cmd = dbConn.CreateCommand())
            {
                cmd.CommandText = sql;
                SQLiteParameter param = new SQLiteParameter("@name")
                {
                    Value = name,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@path")
                {
                    Value = path,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@lang")
                {
                    Value = lang,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@project")
                {
                    Value = project,
                    DbType = System.Data.DbType.String
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@size")
                {
                    Value = size
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@known")
                {
                    Value = known
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@maybe")
                {
                    Value = maybe
                };
                cmd.Parameters.Add(param);

                param = new SQLiteParameter("@unknown")
                {
                    Value = unknown
                };
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Add words to DB as transaction.
        /// </summary>
        public void CommitWords(string filePath, IEnumerable<(string word, int count)> tokens)
        {
            using (SQLiteTransaction transaction = dbConn.BeginTransaction())
            {
                string command = "INSERT INTO Words VALUES(@word, @file, @quantity)";
                foreach (var (word, count) in tokens)
                {
                    SQLiteParameter pathParam = new SQLiteParameter("@file")
                    {
                        Value = filePath,
                        DbType = System.Data.DbType.String
                    };

                    using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
                    {
                        cmd.CommandText = command;
                        cmd.Parameters.Add(pathParam);

                        SQLiteParameter param = new SQLiteParameter("@word")
                        {
                            Value = word,
                            DbType = System.Data.DbType.String
                        };
                        cmd.Parameters.Add(param);

                        param = new SQLiteParameter("@quantity")
                        {
                            Value = count,
                            DbType = System.Data.DbType.Int32
                        };
                        cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            GC.Collect();
        }
        /// <summary>
        /// Provides the list of unknown words and quantities for given
        /// language, project and file.
        /// </summary>
        /// <param name="filePath">path to file</param>
        /// <returns></returns>
        public IEnumerable<(string, int)> GetUnknownWords(string filePath)
        {

            SQLiteParameter fileParam = new SQLiteParameter("@file")
            {
                Value = filePath,
                DbType = System.Data.DbType.String
            };
            string sql = "SELECT word, quantity FROM Words " +
                "WHERE file=@file ORDER BY quantity DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.Parameters.Add(fileParam);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetInt32(1));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Provides the list of unknown words and quantities for given
        /// project.
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public IEnumerable<(string, int)> GetUnknownWords(string lang, string project)
        {
            SQLiteParameter projectParam = new SQLiteParameter("@project")
            {
                Value = project,
                DbType = System.Data.DbType.String
            };

            SQLiteParameter langParam = new SQLiteParameter("@lang")
            {
                Value = lang,
                DbType = System.Data.DbType.String
            };

            string sql = "SELECT word, SUM(quantity) as sum " +
                "FROM Words JOIN Files on Words.file = Files.path " +
                "WHERE project=@project AND lang=@lang " +
                "GROUP BY word ORDER BY sum DESC LIMIT 100";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.Parameters.Add(projectParam);
                cmd.Parameters.Add(langParam);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetInt32(1));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Provides a list of files that contain given word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFilesWithWord(string word)
        {
            string sql = "SELECT file FROM Words WHERE word=@word";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.Parameters.AddWithValue("word", word);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return dr.GetString(0);
                }
            }
        }
        public bool LanguageExists(string langName)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM Languages WHERE lang=@lang LIMIT 1)";
            SQLiteParameter lang = new SQLiteParameter("@lang")
            {
                Value = langName,
                DbType = System.Data.DbType.String
            };
            return ParamExists(sql, lang);
        }
        public bool FolderExists(string folderName)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM Languages WHERE directory=@dir LIMIT 1)";
            SQLiteParameter dir = new SQLiteParameter("@dir")
            {
                Value = folderName,
                DbType = System.Data.DbType.String
            };
            return ParamExists(sql, dir);
        }
        private bool ParamExists(string sql, SQLiteParameter param)
        {
            bool exists = false;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.Parameters.Add(param);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // will be read once, as sql is short circuited
                    exists = dr.GetInt32(0) == 1;
                }
                dr.Close();
            }
            return exists;
        }
    }
}

