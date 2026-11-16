using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PCVerwaltung.Classes
{
    /// <summary>
    /// Hilfsklasse zur automatischen Vorkonfiguration und Hochzählung von IP-Adressen (US-03 / UC 006).
    /// </summary>
    public static class NetworkConfig
    {
        public const string DefaultRouterIp = "192.168.1.1";

        /// <summary>
        /// Prüft, ob ein übergebener String eine gültige IPv4-Adresse ist.
        /// </summary>
        public static bool IsValidIpv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
                return false;

            if (IPAddress.TryParse(ipString.Trim(), out var ip))
            {
                return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }
            return false;
        }

        /// <summary>
        /// Berechnet die nächste IP-Adresse ausgehend von der Router-IP und einem Offset.
        /// Gemäß Lastenheft: Erste Adresse nach dem Router (+1), bei mehreren PCs wird hochgezählt (+2, +3, ...).
        /// </summary>
        public static string CalculateNextIp(string routerIp, int offsetFromRouter)
        {
            if (!IsValidIpv4(routerIp))
                routerIp = DefaultRouterIp;

            var bytes = IPAddress.Parse(routerIp.Trim()).GetAddressBytes();

            // IPv4 als 32-Bit Integer (Big-Endian) behandeln, um Subnetzüberläufe korrekt zu handhaben
            Array.Reverse(bytes);
            uint numericIp = BitConverter.ToUInt32(bytes, 0);

            numericIp += (uint)offsetFromRouter;

            byte[] resultBytes = BitConverter.GetBytes(numericIp);
            Array.Reverse(resultBytes);

            return new IPAddress(resultBytes).ToString();
        }

        /// <summary>
        /// Ermittelt die nächste freie IP-Adresse im Netzwerk des Routers anhand der bereits vergebenen PCs.
        /// </summary>
        public static string GetNextAvailableIp(string routerIp, IEnumerable<PC> existingPcs)
        {
            if (!IsValidIpv4(routerIp))
                routerIp = DefaultRouterIp;

            var usedIps = new HashSet<string>(
                existingPcs
                    .Where(p => !string.IsNullOrWhiteSpace(p.IpAdresse))
                    .Select(p => p.IpAdresse.Trim()),
                StringComparer.OrdinalIgnoreCase
            );

            // Router selbst ist belegt
            usedIps.Add(routerIp.Trim());

            // Suche ab 1 Schritt nach dem Router die erste freie Adresse
            for (int offset = 1; offset < 254; offset++)
            {
                string candidate = CalculateNextIp(routerIp, offset);
                if (!usedIps.Contains(candidate))
                {
                    return candidate;
                }
            }

            // Fallback
            return CalculateNextIp(routerIp, 1);
        }
    }
}
