using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class CPU : HardwareKomponente
    {
        public double Taktfrequenz { get; set; } = 0.0;
        public SockelTyp Sockel { get; set; } = SockelTyp.AM5;
        public int Kerne { get; set; } = 8;

        public CPU(string hersteller, string modell, double taktfrequenz, SockelTyp sockel, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m, int kerne = 8)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            Taktfrequenz = taktfrequenz;
            Sockel = sockel;
            Kerne = kerne;
        }

        // Rückwärtskompatibler Konstruktor
        public CPU(string modell, double taktfrequenz)
            : this(modell.Contains("AMD") ? "AMD" : (modell.Contains("Intel") ? "Intel" : "Unbekannt"),
                   modell, taktfrequenz, SockelTyp.AM5, 0.0m, 0.0m)
        {
        }
    }
}
