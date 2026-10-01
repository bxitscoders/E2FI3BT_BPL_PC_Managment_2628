using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Case : HardwareKomponente
    {
        public Formfaktor Formfaktor { get; set; } = Formfaktor.ATX;

        public Case(string hersteller, string modell, Formfaktor formfaktor, decimal ekPreis = 0.0m, decimal vkPreis = 0.0m)
            : base(hersteller, modell, ekPreis, vkPreis)
        {
            Formfaktor = formfaktor;
        }
    }
}
