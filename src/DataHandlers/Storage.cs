using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FancyRedirect.DataHandlers
{
    public class Storage
    {
        /// <summary>
        /// Full path to the local storage.
        /// </summary>
        private static string StoragePath { get; set; }

        /// <summary>
        /// Stored URL entries.
        /// </summary>
        private static Dictionary<string, Uri> UrlEntries { get; set; }

        #region Get functions

        /// <summary>
        /// Get the code for the stored URL, or create a storage for it.
        /// </summary>
        /// <param name="url">URL to get code for.</param>
        /// <returns>Code.</returns>
        public static string GetCode(Uri url)
        {
            string code;

            lock (UrlEntries)
            {
                foreach (var (key, value) in UrlEntries)
                {
                    if (value == url)
                    {
                        return key;
                    }
                }

                var ccl = 2;
                var acl = 0;
                const int macl = 50;

                code = Guid.NewGuid()
                    .ToString()
                    .Replace("-", "")
                    .Substring(0, ccl);

                while (true)
                {
                    if (!UrlEntries.ContainsKey(code))
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

                UrlEntries.Add(
                    code,
                    url);
            }

            // Save all entries.
            Save();

            return code;
        }

        /// <summary>
        /// Get the full URL for the given code.
        /// </summary>
        /// <param name="code">Code to fetch for.</param>
        /// <returns>Full URL.</returns>
        public static Uri GetUrl(string code)
        {
            lock (UrlEntries)
            {
                return UrlEntries.ContainsKey(code)
                    ? UrlEntries[code]
                    : null;
            }
        }

        #endregion

        #region IO functions

        /// <summary>
        /// Load all entries.
        /// </summary>
        public static void Load()
        {
            try
            {
                StoragePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "storage.json");

                if (!File.Exists(StoragePath))
                {
                    throw new FileNotFoundException(
                        "Storage file not found. Creating a new one.",
                        StoragePath);
                }

                UrlEntries = JsonSerializer.Deserialize<Dictionary<string, Uri>>(
                    File.ReadAllText(StoragePath));
            }
            catch
            {
                UrlEntries = new Dictionary<string, Uri>();
            }
        }

        /// <summary>
        /// Save all entries.
        /// </summary>
        public static void Save()
        {
            lock (UrlEntries)
            {
                try
                {
                    File.WriteAllText(
                        StoragePath,
                        JsonSerializer.Serialize(UrlEntries));
                }
                catch
                {
                    //
                }
            }
        }

        #endregion
    }
}