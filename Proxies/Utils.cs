using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ProxyCollector.Proxies {

    internal class Utils {

        public static void Scrape(string type, string url) {
            // Fetch the text from the URL
            string text = string.Empty;
            try {
                using (WebClient client = new WebClient()) {
                    text = client.DownloadString(url);
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error occurred while scraping the URL: {ex.Message}");
                return;
            }

            // Determine the file path based on the type
            string filePath = $"proxies/{type}.txt";

            // Append the text to the appropriate file
            try {
                using (StreamWriter writer = File.AppendText(filePath)) {
                    writer.WriteLine(text);
                }
                Console.WriteLine($"[Success] scraped {type} proxies");
            } catch (Exception ex) {
                Console.WriteLine($"Error occurred while saving the proxies: {ex.Message}");
            }
        }

        public static int CountLinesInFiles(List<string> files) {
            int totalLines = 0;

            foreach (var file in files) {
                if (File.Exists(file)) {
                    int lines = File.ReadLines(file).Count();
                    totalLines += lines;
                }
            }

            return totalLines;
        }

        public static void Initialize() {
            Console.WriteLine("Initializing...");

            // Check if the proxies folder exists
            string proxiesFolderPath = "proxies";
            if (!Directory.Exists(proxiesFolderPath)) {
                Console.WriteLine("Creating proxies folder...");
                Directory.CreateDirectory(proxiesFolderPath);
            }

            // Check if the proxy files already exist
            if (!ProxyFilesExist()) {
                // Create the proxy files if they don't exist
                CreateProxyFile("Http");
                CreateProxyFile("Socks4");
                CreateProxyFile("Socks5");
                Console.WriteLine("Initialization completed successfully.");
            } else {
                //Console.WriteLine("Initialization skipped. Proxy files already exist.");
                Console.Clear();
            }
        }

        private static bool ProxyFilesExist() {
            string[] proxyTypes = { "Http", "Socks4", "Socks5" };
            foreach (string type in proxyTypes) {
                string filePath = Path.Combine("proxies", $"{type}.txt");
                if (!File.Exists(filePath)) {
                    return false;
                }
            }
            return true;
        }

        private static void CreateProxyFile(string type) {
            string filePath = Path.Combine("proxies", $"{type}.txt");

            // Check if the file already exists
            if (File.Exists(filePath)) {
                Console.WriteLine($"Proxy file {type}.txt already exists.");
                return;
            }

            Console.WriteLine($"Creating {type}.txt...");
            File.WriteAllText(filePath, string.Empty);
        }
    }
}