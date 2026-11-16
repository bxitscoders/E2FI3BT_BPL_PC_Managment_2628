using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class SSD
    {
        public Guid Id { get; }
        public string Hersteller { get; set; }
        public string Modell { get; set; }
        public Formfaktor Formfaktor { get; set; }
        public SockelTyp Sockel { get; set; }

        public SSD(string hersteller, string modell,
                   Formfaktor formfaktor, SockelTyp sockel)
        {
            if (string.IsNullOrWhiteSpace(hersteller))
                throw new ArgumentException("SSD-Hersteller ist Pflicht.");

            if (string.IsNullOrWhiteSpace(modell))
                throw new ArgumentException("SSD-Modell ist Pflicht.");

            Id = Guid.NewGuid();
            Hersteller = hersteller.Trim();
            Modell = modell.Trim();
            Formfaktor = formfaktor;
            Sockel = sockel;
        }

        public override string ToString()
            => $"{Hersteller} {Modell} ({Formfaktor}, {Sockel})";
    }
}