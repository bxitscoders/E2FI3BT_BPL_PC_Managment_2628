using System;

namespace PCVerwaltung.Classes
{
    public class Ram : HardwareKomponente
    {
        public RamTyp Typ { get; set; } = RamTyp.DDR5;
        public int KapazitaetGB { get; set; } = 16;
        public int TaktfrequenzMHz { get; set; } = 5600;

        public Ram(string hersteller, string modell, RamTyp typ, int kapazitaetGB, int taktfrequenzMHz, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            Typ = typ;
            KapazitaetGB = kapazitaetGB;
            TaktfrequenzMHz = taktfrequenzMHz;
        }

        public override string ToString() => $"{Hersteller} {Modell} ({KapazitaetGB} GB {Typ}-{TaktfrequenzMHz})";
    }
}
