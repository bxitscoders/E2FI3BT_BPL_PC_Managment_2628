
using PCVerwaltung.Classes;
using System.Collections.Generic;
using System.Windows;

namespace PCVerwaltung
{
    public partial class App : Application
    {
        // Gehäuse
        public static List<Case> Cases { get; } = new()
        {
            new Case("Fractal Design", "Meshify 2", Formfaktor.ATX),
            new Case("Cooler Master", "NR400", Formfaktor.MicroATX),
            new Case("NZXT", "H1", Formfaktor.MiniITX),
            new Case("be quiet!", "Pure Base 500DX", Formfaktor.ATX),
            new Case("Lian Li", "O11 Dynamic", Formfaktor.ATX),
        };

        // Prozessoren
        public static List<CPU> CPUs { get; } = new()
        {
            new CPU("AMD Ryzen 7 7800X3D", 4.2),
            new CPU("Intel Core i5-13600K", 3.5),
            new CPU("AMD Ryzen 5 5600", 3.5),
            new CPU("Intel Core i7-12700F", 2.1),
            new CPU("AMD Ryzen 7 5700G", 3.8),
        };

        // Mainboards
        public static List<Mainboard> Mainboards { get; } = new()
        {
            new Mainboard("ASUS", "TUF GAMING B650-PLUS",
                Formfaktor.ATX, SockelTyp.AM5),

            new Mainboard("MSI", "PRO B760M-A",
                Formfaktor.MicroATX, SockelTyp.LGA1700),

            new Mainboard("Gigabyte", "B550I AORUS PRO AX",
                Formfaktor.MiniITX, SockelTyp.AM4),

            new Mainboard("ASRock", "B650M Pro RS",
                Formfaktor.MicroATX, SockelTyp.AM5),

            new Mainboard("ASUS", "ROG Strix Z690-A",
                Formfaktor.ATX, SockelTyp.LGA1700),
        };

        // SSDs
        // Formfaktor und Sockel sind vorläufige Platzhalter,
        // weil die aktuelle SSD-Klasse diese Parameter verlangt.
        public static List<SSD> SSDs { get; } = new()
        {
            new SSD("Samsung", "990 PRO 1TB",
                Formfaktor.ATX, SockelTyp.AM5),

            new SSD("Western Digital", "WD Black SN850X 2TB",
                Formfaktor.MicroATX, SockelTyp.LGA1700),

            new SSD("Crucial", "P5 Plus 1TB",
                Formfaktor.MiniITX, SockelTyp.AM4),

            new SSD("Kingston", "KC3000 2TB",
                Formfaktor.ATX, SockelTyp.LGA1700),

            new SSD("Samsung", "980 PRO 500GB",
                Formfaktor.MicroATX, SockelTyp.AM5),
        };

        // Vorkonfigurierte PCs
        public static List<PC> PCs { get; } = new()
        {
            new PC(Cases[0], CPUs[0], Mainboards[0], SSDs[0]),
            new PC(Cases[1], CPUs[1], Mainboards[1], SSDs[1]),
            new PC(Cases[2], CPUs[2], Mainboards[2], SSDs[2]),
            new PC(Cases[3], CPUs[3], Mainboards[4], SSDs[3]),
            new PC(Cases[4], CPUs[4], Mainboards[3], SSDs[4]),
        };
    }
}
