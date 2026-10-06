using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    /// <summary>
    /// Reines DTO für den Transport/Binding – ohne Logik.
    /// </summary>
    public class Mainboard : HardwareKomponente
    {
        public Formfaktor Formfaktor { get; set; }
        public SockelTyp Sockel { get; set; }  
        public int RamSlots { get; set; } = 4;

        public Mainboard(string hersteller, string modell, Formfaktor formfaktor, SockelTyp sockel, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m, int ramSlots = 4)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            if (string.IsNullOrWhiteSpace(hersteller)) throw new Exception("Board-Hersteller ist Pflicht.");
            if (string.IsNullOrWhiteSpace(modell)) throw new Exception("Board-Modell ist Pflicht.");

            Formfaktor = formfaktor;
            Sockel = sockel;
            RamSlots = ramSlots;
        }

        public override string ToString() => $"{Hersteller} {Modell} ({Formfaktor}, {Sockel})";
    }
}
