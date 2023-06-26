using ProxyCollector.Proxies;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProxyCollector {

    internal class Entry {

        private static void Main(string[] args) {
            Utils.Initialize();
            Console.WriteLine();
            Console.WriteLine("Removed Duplicates:");
            Console.WriteLine("-----------------");
            Console.WriteLine("duplicates will only be removed at the start of the program");
            var files = new List<string> { "proxies/Http.txt", "proxies/Socks4.txt", "proxies/Socks5.txt" };
            DuplicateRemover.RemoveDuplicates(files);
            Console.WriteLine($"Total duplicates removed: {DuplicateRemover.TotalDuplicatesRemoved}");
            Console.WriteLine("-----------------");
            
            // Start a separate thread for scraping proxies
            Thread scrapeThread = new Thread(() => {
                while (true) {
                    Console.WriteLine($"Scraping Proxies:");
                    Console.WriteLine("-----------------");
                    ScrapeProxies();
                    Console.WriteLine("-----------------");
                    Console.WriteLine();
                    Console.WriteLine("Idle for 15 minutes...");
                    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy}] - [{DateTime.Now:hh:mm:ss}]");

                    Thread.Sleep(TimeSpan.FromMinutes(15));
                }
            });
            Console.WriteLine();
            scrapeThread.Start();

            // Keep the main thread running
            while (true) {
                // main thread logic here
                int lineCount = Utils.CountLinesInFiles(files);
                Console.Title = $"ProxyCollector: Collected [{lineCount}] - Time: [{DateTime.Now:hh:mm:ss}]";
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static void ScrapeProxies() {
            // Scrape Http proxies
            Utils.Scrape("Http", URLS.PsHTTP);

            // Scrape Socks4 proxies
            Utils.Scrape("Socks4", URLS.PsSOCKS4);

            // Scrape Socks5 proxies
            Utils.Scrape("Socks5", URLS.PsSOCKS5);
        }
    }
}