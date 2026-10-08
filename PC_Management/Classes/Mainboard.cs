using System;

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

        public Mainboard() : base("", "") { }

        public Mainboard(string hersteller, string modell, Formfaktor formfaktor, SockelTyp sockel, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m, int ramSlots = 4)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            Formfaktor = formfaktor;
            Sockel = sockel;
            RamSlots = ramSlots;
        }

        public override string ToString() => $"{Hersteller} {Modell} ({Formfaktor}, {Sockel})";
    }
}
