using PCVerwaltung.Classes;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<Case> Cases { get; } = new()
        {
            new Case("Fractal Design", "Meshify 2",       Formfaktor.ATX,       85.00m, 119.90m),
            new Case("Cooler Master",  "NR400",           Formfaktor.MicroATX,  48.00m,  69.90m),
            new Case("NZXT",           "H1",              Formfaktor.MiniITX,  140.00m, 199.00m),
            new Case("be quiet!",      "Pure Base 500DX", Formfaktor.ATX,       72.00m,  99.90m),
            new Case("Lian Li",        "O11 Dynamic",     Formfaktor.ATX,      110.00m, 149.90m),
        };

        public static ObservableCollection<CPU> CPUs { get; } = new()
        {
            new CPU("AMD",   "Ryzen 7 7800X3D", 4.2, SockelTyp.AM5,     290.00m, 379.00m, 8),
            new CPU("Intel", "Core i5-13600K",  3.5, SockelTyp.LGA1700, 220.00m, 289.00m, 14),
            new CPU("AMD",   "Ryzen 5 5600",    3.5, SockelTyp.AM4,      85.00m, 119.00m, 6),
            new CPU("Intel", "Core i7-12700F",  2.1, SockelTyp.LGA1700, 190.00m, 249.00m, 12),
            new CPU("AMD",   "Ryzen 7 5700G",   3.8, SockelTyp.AM4,     135.00m, 179.00m, 8),
        };

        public static ObservableCollection<Mainboard> Mainboards { get; } = new()
        {
            new Mainboard("ASUS",     "TUF GAMING B650-PLUS", Formfaktor.ATX,      SockelTyp.AM5,     140.00m, 189.90m, 4),
            new Mainboard("MSI",      "PRO B760M-A",          Formfaktor.MicroATX, SockelTyp.LGA1700, 105.00m, 139.90m, 4),
            new Mainboard("Gigabyte", "B550I AORUS PRO AX",   Formfaktor.MiniITX,  SockelTyp.AM4,     130.00m, 175.00m, 2),
            new Mainboard("ASRock",   "B650M Pro RS",         Formfaktor.MicroATX, SockelTyp.AM5,      95.00m, 129.90m, 4),
            new Mainboard("ASUS",     "ROG Strix Z690-A",     Formfaktor.ATX,      SockelTyp.LGA1700, 180.00m, 239.00m, 4),
        };

        public static ObservableCollection<Ram> Rams { get; } = new()
        {
            new Ram("Corsair",  "Vengeance LPX",   RamTyp.DDR4, 16, 3200, 32.00m,  44.90m),
            new Ram("G.Skill",  "Ripjaws S5",      RamTyp.DDR5, 32, 5600, 68.00m,  94.90m),
            new Ram("Kingston", "Fury Beast",      RamTyp.DDR5, 32, 6000, 75.00m, 104.90m),
            new Ram("Crucial",  "Pro RAM",         RamTyp.DDR4, 32, 3200, 52.00m,  69.90m),
            new Ram("Corsair",  "Dominator Plat.", RamTyp.DDR5, 64, 6000, 160.00m, 219.00m),
        };

        public static ObservableCollection<SSD> SSDs { get; } = new()
        {
            new SSD("Samsung",  "990 PRO",      SsdTyp.NVMe, 1000, 7450, 75.00m, 109.90m),
            new SSD("WD",       "Black SN850X", SsdTyp.NVMe, 2000, 7300, 115.00m, 159.00m),
            new SSD("Crucial",  "P3 Plus",      SsdTyp.NVMe, 1000, 5000, 45.00m,  62.90m),
            new SSD("Kingston", "KC600",        SsdTyp.SATA,  512,  550, 30.00m,  42.50m),
            new SSD("Samsung",  "870 EVO",      SsdTyp.SATA, 1000,  560, 55.00m,  79.90m),
        };

        public static ObservableCollection<PC> PCs { get; } = new()
        {
            new PC(Cases[0], CPUs[0], Mainboards[0]),
            new PC(Cases[1], CPUs[1], Mainboards[1]),
            new PC(Cases[2], CPUs[2], Mainboards[2]),
            new PC(Cases[3], CPUs[3], Mainboards[4]),
            new PC(Cases[4], CPUs[4], Mainboards[3]),
        };

        // Instanz-Properties für DataContext = App.Current Datenbindung in XAML
        public ObservableCollection<Case> CasesList => Cases;
        public ObservableCollection<CPU> CPUsList => CPUs;
        public ObservableCollection<Mainboard> MainboardsList => Mainboards;
        public ObservableCollection<Ram> RamsList => Rams;
        public ObservableCollection<SSD> SSDsList => SSDs;
        public ObservableCollection<PC> PCsList => PCs;
    }
}
