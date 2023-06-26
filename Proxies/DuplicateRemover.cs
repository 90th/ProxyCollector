using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProxyCollector.Proxies {
    internal class DuplicateRemover {
        private static readonly string BackupFolderPath = "backups";
        private static readonly TimeSpan MaxRuntimeForBackup = TimeSpan.FromHours(10);

        public static int TotalDuplicatesRemoved { get; private set; }

        private static Dictionary<string, int> duplicatesRemovedCount = new Dictionary<string, int>();

        public static void RemoveDuplicates(List<string> files) {
            TotalDuplicatesRemoved = 0;
            duplicatesRemovedCount.Clear();

            var programStartTime = DateTime.Now;

            // Create the backups folder if it doesn't exist
            if (!Directory.Exists(BackupFolderPath)) {
                Directory.CreateDirectory(BackupFolderPath);
            }

            foreach (var file in files) {
                if (!File.Exists(file)) {
                    // Create the file if it doesn't exist
                    File.Create(file).Close();
                }

                var lines = File.ReadAllLines(file);

                var uniqueEntries = new HashSet<string>(lines);

                var removedCount = lines.Length - uniqueEntries.Count;

                Console.WriteLine($"Duplicates removed from {GetFileName(file)}: {removedCount}");

                duplicatesRemovedCount[file] = removedCount;
                TotalDuplicatesRemoved += removedCount;

                if (DateTime.Now - programStartTime > MaxRuntimeForBackup) {
                    var backupFileName = GetBackupFileName(file);
                    File.Copy(file, backupFileName, true);
                    Console.WriteLine($"Program has been running for more than 10 hours\nStarting to create backups of proxies under ./backups/{file}");
                }

                File.WriteAllLines(file, uniqueEntries, Encoding.UTF8);
            }
        }

        public static int GetDuplicatesRemovedCount(string file) {
            if (duplicatesRemovedCount.ContainsKey(file))
                return duplicatesRemovedCount[file];
            return 0;
        }

        private static string GetBackupFileName(string file) {
            var backupDir = Path.Combine(BackupFolderPath, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(backupDir)) {
                Directory.CreateDirectory(backupDir);
            }

            var backupFileName = $"{Path.GetFileNameWithoutExtension(file)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file)}";
            return Path.Combine(backupDir, backupFileName);
        }

        private static string GetFileName(string filePath) {
            return Path.GetFileNameWithoutExtension(filePath);
        }
    }
}
