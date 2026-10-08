using System;

namespace PCVerwaltung.Classes
{
    public class PC
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Case? Case { get; set; }
        public CPU? Cpu { get; set; }
        public Mainboard? Mainboard { get; set; }
        public Ram? Ram { get; set; }
        public SSD? Ssd { get; set; }
        public string IpAdresse { get; set; } = string.Empty;

        // Berechnete Preise & Kennzahlen
        public decimal GesamtEkPreis => 
            (Case?.EkPreis ?? 0) + 
            (Cpu?.EkPreis ?? 0) + 
            (Mainboard?.EkPreis ?? 0) + 
            (Ram?.EkPreis ?? 0) + 
            (Ssd?.EkPreis ?? 0);

        public decimal GesamtVkPreis => 
            (Case?.VkPreis ?? 0) + 
            (Cpu?.VkPreis ?? 0) + 
            (Mainboard?.VkPreis ?? 0) + 
            (Ram?.VkPreis ?? 0) + 
            (Ssd?.VkPreis ?? 0);

        public decimal Gewinn => GesamtVkPreis - GesamtEkPreis;

        // Zusammenfassende Strings für DataGrid-Anzeigen
        public string CpuSummary => Cpu != null ? $"{Cpu.Modell} ({Cpu.Taktfrequenz:0.##} GHz, {Cpu.Sockel})" : "-";
        public string RamSummary => Ram != null ? $"{Ram.Hersteller} {Ram.Modell} ({Ram.KapazitaetGB} GB {Ram.Typ})" : "-";
        public string SsdSummary => Ssd != null ? $"{Ssd.Hersteller} {Ssd.Modell} ({Ssd.KapazitaetGB} GB {Ssd.Typ})" : "-";
        public string MainboardSummary => Mainboard != null ? $"{Mainboard.Hersteller} {Mainboard.Modell} ({Mainboard.Sockel})" : "-";
        public string CaseSummary => Case != null ? $"{Case.Hersteller} {Case.Modell} ({Case.Formfaktor})" : "-";

        public PC() { }

        public PC(string name, Case @case, CPU cpu, Mainboard mainboard, Ram ram, SSD ssd, string ipAdresse = "")
        {
            Name = name;
            Case = @case ?? throw new ArgumentNullException(nameof(@case));
            Cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
            Mainboard = mainboard ?? throw new ArgumentNullException(nameof(mainboard));
            Ram = ram;
            Ssd = ssd;
            IpAdresse = ipAdresse;
        }

        // Überladung für Rückwärtskompatibilität
        public PC(Case @case, CPU cpu, Mainboard mainboard)
            : this("PC-System", @case, cpu, mainboard, null!, null!, "")
        {
        }

        public override string ToString()
            => $"{Name} [IP: {(string.IsNullOrWhiteSpace(IpAdresse) ? "Keine" : IpAdresse)}] | {Cpu?.Modell} | {Ram?.KapazitaetGB}GB RAM | {Ssd?.KapazitaetGB}GB SSD | {Mainboard?.Modell} | {Case?.Modell} | {GesamtVkPreis:0.00} €";
    }
}
