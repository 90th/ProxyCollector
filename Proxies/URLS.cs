namespace ProxyCollector.Proxies {

    internal class URLS {
        private static string psHTTP = "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
        private static string psSOCKS4 = "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks4&timeout=10000&country=all&ssl=all&anonymity=all";
        private static string psSOCKS5 = "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks5&timeout=10000&country=all&ssl=all&anonymity=all";

        public static string PsSOCKS5 { get => psSOCKS5; set => psSOCKS5 = value; }
        public static string PsSOCKS4 { get => psSOCKS4; set => psSOCKS4 = value; }
        public static string PsHTTP { get => psHTTP; set => psHTTP = value; }
    }
}