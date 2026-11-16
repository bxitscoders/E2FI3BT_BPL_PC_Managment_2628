using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PCVerwaltung.Classes;

namespace PCVerwaltung.Data
{
    /// <summary>
    /// Zentraler Service für Datenbank-Operationen mit SQLite.
    /// Handhabt Migrationen, Initialisierung (Seeding) und CRUD-Operationen.
    /// </summary>
    public static class DatabaseService
    {
        /// <summary>
        /// Stellt sicher, dass die SQLite-Datenbank existiert und befüllt sie beim ersten Start mit Basisdaten.
        /// </summary>
        public static void InitializeDatabase()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            if (!db.Cases.Any())
            {
                var defaultCases = new List<Case>
                {
                    new Case("Fractal Design", "Meshify 2",       Formfaktor.ATX,       85.00m, 119.90m),
                    new Case("Cooler Master",  "NR400",           Formfaktor.MicroATX,  48.00m,  69.90m),
                    new Case("NZXT",           "H1",              Formfaktor.MiniITX,  140.00m, 199.00m),
                    new Case("be quiet!",      "Pure Base 500DX", Formfaktor.ATX,       72.00m,  99.90m),
                    new Case("Lian Li",        "O11 Dynamic",     Formfaktor.ATX,      110.00m, 149.90m),
                };

                var defaultCpus = new List<CPU>
                {
                    new CPU("AMD",   "Ryzen 7 7800X3D", 4.2, SockelTyp.AM5,     290.00m, 379.00m, 8),
                    new CPU("Intel", "Core i5-13600K",  3.5, SockelTyp.LGA1700, 220.00m, 289.00m, 14),
                    new CPU("AMD",   "Ryzen 5 5600",    3.5, SockelTyp.AM4,      85.00m, 119.00m, 6),
                    new CPU("Intel", "Core i7-12700F",  2.1, SockelTyp.LGA1700, 190.00m, 249.00m, 12),
                    new CPU("AMD",   "Ryzen 7 5700G",   3.8, SockelTyp.AM4,     135.00m, 179.00m, 8),
                };

                var defaultBoards = new List<Mainboard>
                {
                    new Mainboard("ASUS",     "TUF GAMING B650-PLUS", Formfaktor.ATX,      SockelTyp.AM5,     140.00m, 189.90m, 4),
                    new Mainboard("MSI",      "PRO B760M-A",          Formfaktor.MicroATX, SockelTyp.LGA1700, 105.00m, 139.90m, 4),
                    new Mainboard("Gigabyte", "B550I AORUS PRO AX",   Formfaktor.MiniITX,  SockelTyp.AM4,     130.00m, 175.00m, 2),
                    new Mainboard("ASRock",   "B650M Pro RS",         Formfaktor.MicroATX, SockelTyp.AM5,      95.00m, 129.90m, 4),
                    new Mainboard("ASUS",     "ROG Strix Z690-A",     Formfaktor.ATX,      SockelTyp.LGA1700, 180.00m, 239.00m, 4),
                };

                var defaultRams = new List<Ram>
                {
                    new Ram("Corsair",  "Vengeance LPX",   RamTyp.DDR4, 16, 3200, 32.00m,  44.90m),
                    new Ram("G.Skill",  "Ripjaws S5",      RamTyp.DDR5, 32, 5600, 68.00m,  94.90m),
                    new Ram("Kingston", "Fury Beast",      RamTyp.DDR5, 32, 6000, 75.00m, 104.90m),
                    new Ram("Crucial",  "Pro RAM",         RamTyp.DDR4, 32, 3200, 52.00m,  69.90m),
                    new Ram("Corsair",  "Dominator Plat.", RamTyp.DDR5, 64, 6000, 160.00m, 219.00m),
                };

                var defaultSsds = new List<SSD>
                {
                    new SSD("Samsung",  "990 PRO",      SsdTyp.NVMe, 1000, 7450, 75.00m, 109.90m),
                    new SSD("WD",       "Black SN850X", SsdTyp.NVMe, 2000, 7300, 115.00m, 159.00m),
                    new SSD("Crucial",  "P3 Plus",      SsdTyp.NVMe, 1000, 5000, 45.00m,  62.90m),
                    new SSD("Kingston", "KC600",        SsdTyp.SATA,  512,  550, 30.00m,  42.50m),
                    new SSD("Samsung",  "870 EVO",      SsdTyp.SATA, 1000,  560, 55.00m,  79.90m),
                };

                db.Cases.AddRange(defaultCases);
                db.CPUs.AddRange(defaultCpus);
                db.Mainboards.AddRange(defaultBoards);
                db.Rams.AddRange(defaultRams);
                db.SSDs.AddRange(defaultSsds);

                var defaultPcs = new List<PC>
                {
                    new PC("Gaming Beast AM5",        defaultCases[0], defaultCpus[0], defaultBoards[0], defaultRams[1], defaultSsds[0], "192.168.1.2"),
                    new PC("Office Compact Intel",    defaultCases[1], defaultCpus[3], defaultBoards[1], defaultRams[0], defaultSsds[3], "192.168.1.3"),
                    new PC("Mini-ITX Living Room",    defaultCases[2], defaultCpus[4], defaultBoards[2], defaultRams[3], defaultSsds[2], "192.168.1.4"),
                    new PC("Creator Workstation Pro", defaultCases[3], defaultCpus[1], defaultBoards[4], defaultRams[4], defaultSsds[1], "192.168.1.5"),
                    new PC("Streamer Edition AM5",    defaultCases[4], defaultCpus[2], defaultBoards[3], defaultRams[2], defaultSsds[0], "192.168.1.6"),
                };

                db.PCs.AddRange(defaultPcs);
                db.SaveChanges();
            }
        }

        public static List<Case> LoadCases()
        {
            using var db = new AppDbContext();
            return db.Cases.ToList();
        }

        public static List<CPU> LoadCpus()
        {
            using var db = new AppDbContext();
            return db.CPUs.ToList();
        }

        public static List<Mainboard> LoadMainboards()
        {
            using var db = new AppDbContext();
            return db.Mainboards.ToList();
        }

        public static List<Ram> LoadRams()
        {
            using var db = new AppDbContext();
            return db.Rams.ToList();
        }

        public static List<SSD> LoadSsds()
        {
            using var db = new AppDbContext();
            return db.SSDs.ToList();
        }

        public static List<PC> LoadPcs()
        {
            using var db = new AppDbContext();
            return db.PCs
                     .Include(p => p.Case)
                     .Include(p => p.Cpu)
                     .Include(p => p.Mainboard)
                     .Include(p => p.Ram)
                     .Include(p => p.Ssd)
                     .ToList();
        }

        public static void SaveCase(Case c)
        {
            using var db = new AppDbContext();
            db.Cases.Add(c);
            db.SaveChanges();
        }

        public static void SaveCpu(CPU cpu)
        {
            using var db = new AppDbContext();
            db.CPUs.Add(cpu);
            db.SaveChanges();
        }

        public static void SaveMainboard(Mainboard mb)
        {
            using var db = new AppDbContext();
            db.Mainboards.Add(mb);
            db.SaveChanges();
        }

        public static void SaveRam(Ram ram)
        {
            using var db = new AppDbContext();
            db.Rams.Add(ram);
            db.SaveChanges();
        }

        public static void SaveSsd(SSD ssd)
        {
            using var db = new AppDbContext();
            db.SSDs.Add(ssd);
            db.SaveChanges();
        }

        public static void SavePc(PC pc)
        {
            using var db = new AppDbContext();
            if (pc.Case != null) db.Entry(pc.Case).State = EntityState.Unchanged;
            if (pc.Cpu != null) db.Entry(pc.Cpu).State = EntityState.Unchanged;
            if (pc.Mainboard != null) db.Entry(pc.Mainboard).State = EntityState.Unchanged;
            if (pc.Ram != null) db.Entry(pc.Ram).State = EntityState.Unchanged;
            if (pc.Ssd != null) db.Entry(pc.Ssd).State = EntityState.Unchanged;

            db.PCs.Add(pc);
            db.SaveChanges();
        }
    }
}
