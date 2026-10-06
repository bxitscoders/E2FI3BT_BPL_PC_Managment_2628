using System;

namespace PCVerwaltung.Classes
{
    /// <summary>
    /// Gemeinsame Basisklasse für alle Hardware-Komponenten (Generalisierung).
    /// Beinhaltet Pflichtfelder wie Hersteller, Modell, EK-Preis und VK-Preis.
    /// </summary>
    public abstract class HardwareKomponente
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Hersteller { get; set; } = string.Empty;
        public string Modell { get; set; } = string.Empty;
        public decimal EkPreis { get; set; } = 0.0m;
        public decimal VkPreis { get; set; } = 0.0m;

        protected HardwareKomponente(string hersteller, string modell, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m)
        {
            Hersteller = hersteller?.Trim() ?? string.Empty;
            Modell = modell?.Trim() ?? string.Empty;
            EkPreis = ekPreis;
            VkPreis = vkPreis;
        }

        public override string ToString() => $"{Hersteller} {Modell} (EK: {EkPreis:C}, VK: {VkPreis:C})";
    }
}
