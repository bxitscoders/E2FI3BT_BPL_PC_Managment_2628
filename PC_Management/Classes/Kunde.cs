using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PCVerwaltung.Classes
{
    /// <summary>
    /// Domänenklasse für Kundenstammdaten inklusive Bonitätsranking (US-04 / UC 003).
    /// </summary>
    public class Kunde : INotifyPropertyChanged
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex PhoneRegex = new(
            @"^[+\d][\d\s/\-()]{4,24}$",
            RegexOptions.Compiled);

        private string _vorname = string.Empty;
        private string _nachname = string.Empty;
        private string _adresse = string.Empty;
        private string _email = string.Empty;
        private string _telefon = string.Empty;
        private KundenRanking _ranking = KundenRanking.STANDARD;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Vorname
        {
            get => _vorname;
            set
            {
                if (_vorname != value)
                {
                    _vorname = value?.Trim() ?? string.Empty;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string Nachname
        {
            get => _nachname;
            set
            {
                if (_nachname != value)
                {
                    _nachname = value?.Trim() ?? string.Empty;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string Adresse
        {
            get => _adresse;
            set
            {
                if (_adresse != value)
                {
                    _adresse = value?.Trim() ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value?.Trim() ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        public string Telefon
        {
            get => _telefon;
            set
            {
                if (_telefon != value)
                {
                    _telefon = value?.Trim() ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        public KundenRanking Ranking
        {
            get => _ranking;
            set
            {
                if (_ranking != value)
                {
                    _ranking = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StandardRabattProzent));
                    OnPropertyChanged(nameof(IstFinanzierungMoeglich));
                    OnPropertyChanged(nameof(KonditionenSummary));
                }
            }
        }

        public string FullName => $"{Vorname} {Nachname}".Trim();

        public decimal StandardRabattProzent => Ranking switch
        {
            KundenRanking.PRIME => 14m,
            KundenRanking.STANDARD => 5m,
            _ => 0m
        };

        public bool IstFinanzierungMoeglich => Ranking != KundenRanking.LOW;

        public string KonditionenSummary => Ranking switch
        {
            KundenRanking.PRIME => "14 % Bar-Rabatt | Finanzierung ✅",
            KundenRanking.STANDARD => "5 % Bar-Rabatt | Finanzierung ✅",
            _ => "0 % Rabatt | Nur Barzahlung ⚠️"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public Kunde() { }

        public Kunde(string vorname, string nachname, string adresse, string email, string telefon, KundenRanking ranking = KundenRanking.STANDARD)
        {
            Vorname = vorname;
            Nachname = nachname;
            Adresse = adresse;
            Email = email;
            Telefon = telefon;
            Ranking = ranking;
        }

        public static bool IsValidEmail(string? email)
            => !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());

        public static bool IsValidPhone(string? telefon)
            => !string.IsNullOrWhiteSpace(telefon) && PhoneRegex.IsMatch(telefon.Trim());

        public static bool IsValidAddress(string? adresse)
            => !string.IsNullOrWhiteSpace(adresse) && adresse.Trim().Length >= 5 && adresse.Contains(' ');

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public override string ToString()
            => $"{FullName} [{Ranking}] – {Email} | {Adresse}";
    }
}
