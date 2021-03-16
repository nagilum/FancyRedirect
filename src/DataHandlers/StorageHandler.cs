using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace FancyRedirect.DataHandlers
{
    public class StorageHandler
    {
        #region Static properties

        /// <summary>
        /// Full path to the local storage.
        /// </summary>
        public static string StoragePath { get; set; }

        #endregion

        #region Static IO functions

        /// <summary>
        /// Create the database and table.
        /// </summary>
        public static void CreateTable()
        {
            StoragePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "storage.sqlite.db");

            if (File.Exists(StoragePath))
            {
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={StoragePath}");
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS UrlEntries (" +
                    "  [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "  [Created] NVARCHAR(32) NOT NULL," +
                    "  [LastUsed] NVARCHAR(32)," +
                    "  [Code] NVARCHAR(32) NOT NULL," +
                    "  [Url] NVARCHAR(2048) NOT NULL" +
                    ");";

                command.ExecuteNonQuery();
            }
            catch
            {
                // TODO
            }
        }

        /// <summary>
        /// Get a list of all entries.
        /// </summary>
        public static List<UrlEntry> GetAll()
        {
            return new DatabaseContext()
                .UrlEntries
                .ToList();
        }

        /// <summary>
        /// Get an entry by code, and possibly update last-used.
        /// </summary>
        public static UrlEntry GetByCode(string code, bool updateLastUsed)
        {
            using var db = new DatabaseContext();

            var entry = db.UrlEntries
                .FirstOrDefault(n => n.Code == code);

            if (!updateLastUsed ||
                entry == null)
            {
                return entry;
            }

            entry.LastUsed = DateTimeOffset.Now.ToString();
            db.SaveChanges();

            return entry;
        }

        /// <summary>
        /// Get an entry by uri, insert if not found.
        /// </summary>
        public static UrlEntry GetOrInsertByUri(Uri uri)
        {
            using var db = new DatabaseContext();

            var entry = db.UrlEntries
                .FirstOrDefault(n => n.Url == uri.ToString());

            if (entry != null)
            {
                return entry;
            }

            var ccl = 2;
            var acl = 0;
            const int macl = 50;

            var code = Guid.NewGuid()
                .ToString()
                .Replace("-", "")
                .Substring(0, ccl);

            while (true)
            {
                if (!db.UrlEntries.Any(n => n.Code == code))
                {
                    break;
                }

                code = Guid.NewGuid()
                    .ToString()
                    .Replace("-", "")
                    .Substring(0, ccl);

                acl++;

                if (acl < macl)
                {
                    continue;
                }

                ccl++;
                acl = 0;
            }

            entry = new UrlEntry
            {
                Created = DateTimeOffset.Now.ToString(),
                Code = code,
                Url = uri.ToString()
            };

            db.UrlEntries.Add(entry);
            db.SaveChanges();

            return entry;
        }

        #endregion
    }
}
