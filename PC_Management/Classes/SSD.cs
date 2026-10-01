using System;

namespace PCVerwaltung.Classes
{
    public class SSD : HardwareKomponente
    {
        public SsdTyp Typ { get; set; } = SsdTyp.NVMe;
        public int KapazitaetGB { get; set; } = 1000;
        public int LesegeschwindigkeitMBs { get; set; } = 7000;

        public SSD(string hersteller, string modell, SsdTyp typ, int kapazitaetGB, int lesegeschwindigkeitMBs, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            Typ = typ;
            KapazitaetGB = kapazitaetGB;
            LesegeschwindigkeitMBs = lesegeschwindigkeitMBs;
        }

        public override string ToString() => $"{Hersteller} {Modell} ({KapazitaetGB} GB {Typ})";
    }
}
