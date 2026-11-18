using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using PCVerwaltung.Classes;
using PCVerwaltung.Data;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für KundenView.xaml (US-04: Kundenstammdaten & Bonitätsranking pflegen / UC 003).
    /// </summary>
    public partial class KundenView : UserControl
    {
        private Kunde? _selectedKunde;
        private readonly ICollectionView _kundenCollectionView;

        public KundenView()
        {
            InitializeComponent();
            DataContext = App.Current;

            CmbRanking.ItemsSource = Enum.GetValues(typeof(KundenRanking));
            CmbRanking.SelectedItem = KundenRanking.STANDARD;

            _kundenCollectionView = CollectionViewSource.GetDefaultView(App.Kunden);
            _kundenCollectionView.Filter = FilterKunde;
            dgKunden.ItemsSource = _kundenCollectionView;

            UpdateRankingInfo();
        }

        private bool FilterKunde(object obj)
        {
            if (obj is not Kunde k)
                return false;

            string query = TxtSearch?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(query))
                return true;

            return k.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Adresse.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Email.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Telefon.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Ranking.ToString().Contains(query, StringComparison.OrdinalIgnoreCase);
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _kundenCollectionView?.Refresh();
        }

        private void OnRankingSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRankingInfo();
        }

        private void UpdateRankingInfo()
        {
            if (TxtRankingInfo == null || CmbRanking?.SelectedItem is not KundenRanking ranking)
                return;

            switch (ranking)
            {
                case KundenRanking.PRIME:
                    TxtRankingInfo.Text = "Bonitätsstufe PRIME: 14 % Barzahlungsrabatt | Ratenfinanzierung zulässig";
                    TxtRankingInfo.Foreground = Brushes.Green;
                    break;
                case KundenRanking.STANDARD:
                    TxtRankingInfo.Text = "Bonitätsstufe STANDARD: 5 % Barzahlungsrabatt | Ratenfinanzierung zulässig";
                    TxtRankingInfo.Foreground = Brushes.DodgerBlue;
                    break;
                case KundenRanking.LOW:
                    TxtRankingInfo.Text = "Bonitätsstufe LOW: 0 % Rabatt | Nur Barzahlung (von Ratenfinanzierung ausgeschlossen)";
                    TxtRankingInfo.Foreground = Brushes.DarkOrange;
                    break;
            }
        }

        private void OnCustomerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKunden.SelectedItem is Kunde kunde)
            {
                _selectedKunde = kunde;
                TxtVorname.Text = kunde.Vorname;
                TxtNachname.Text = kunde.Nachname;
                TxtAdresse.Text = kunde.Adresse;
                TxtEmail.Text = kunde.Email;
                TxtTelefon.Text = kunde.Telefon;
                CmbRanking.SelectedItem = kunde.Ranking;

                TxtFormMode.Text = $"Kunde bearbeiten: {kunde.FullName}";
                IconFormMode.Kind = PackIconKind.AccountEdit;
                BtnSave.Content = "Änderungen speichern";
                BtnDelete.IsEnabled = true;
            }
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            _selectedKunde = null;
            dgKunden.SelectedItem = null;

            TxtVorname.Clear();
            TxtNachname.Clear();
            TxtAdresse.Clear();
            TxtEmail.Clear();
            TxtTelefon.Clear();
            CmbRanking.SelectedItem = KundenRanking.STANDARD;

            TxtFormMode.Text = "Neuen Kunden erfassen";
            IconFormMode.Kind = PackIconKind.AccountPlus;
            BtnSave.Content = "Kunde speichern";
            BtnDelete.IsEnabled = false;

            UpdateRankingInfo();
            TxtVorname.Focus();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            string vorname = TxtVorname.Text?.Trim() ?? string.Empty;
            string nachname = TxtNachname.Text?.Trim() ?? string.Empty;
            string adresse = TxtAdresse.Text?.Trim() ?? string.Empty;
            string email = TxtEmail.Text?.Trim() ?? string.Empty;
            string telefon = TxtTelefon.Text?.Trim() ?? string.Empty;
            var ranking = CmbRanking.SelectedItem is KundenRanking r ? r : KundenRanking.STANDARD;

            // 1. Validierung: Vor- und Nachname
            if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname))
            {
                MessageBox.Show("Bitte geben Sie sowohl den Vornamen als auch den Nachnamen des Kunden ein.",
                                "Fehlende Namensangabe",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                if (string.IsNullOrWhiteSpace(vorname)) TxtVorname.Focus();
                else TxtNachname.Focus();
                return;
            }

            // 2. Validierung: Adresse (Straße + Ort/PLZ)
            if (!Kunde.IsValidAddress(adresse))
            {
                MessageBox.Show("Bitte geben Sie eine vollständige Anschrift ein (z. B. 'Königstraße 24, 70173 Stuttgart').",
                                "Ungültige Anschrift",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtAdresse.Focus();
                return;
            }

            // 3. Validierung: E-Mail-Format
            if (!Kunde.IsValidEmail(email))
            {
                MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse im Format 'name@domain.de' ein.",
                                "Ungültige E-Mail-Adresse",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtEmail.Focus();
                return;
            }

            // 4. Duplikatsprüfung für E-Mail-Adresse
            bool emailDuplicate = App.Kunden.Any(k =>
                k.Id != (_selectedKunde?.Id ?? Guid.Empty) &&
                k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (emailDuplicate)
            {
                MessageBox.Show($"Ein anderer Kunde mit der E-Mail-Adresse '{email}' ist bereits im System hinterlegt.",
                                "E-Mail-Adresse bereits vergeben",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtEmail.Focus();
                return;
            }

            // 5. Validierung: Telefonnummer
            if (!Kunde.IsValidPhone(telefon))
            {
                MessageBox.Show("Bitte geben Sie eine gültige Telefonnummer ein (mindestens 5 Ziffern, z. B. '+49 711 123456').",
                                "Ungültige Telefonnummer",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtTelefon.Focus();
                return;
            }

            // 6. Neu anlegen oder bestehenden Kunden aktualisieren
            if (_selectedKunde == null)
            {
                var neuerKunde = new Kunde(vorname, nachname, adresse, email, telefon, ranking);
                DatabaseService.SaveKunde(neuerKunde);
                App.Kunden.Add(neuerKunde);

                MessageBox.Show($"Der Kunde '{neuerKunde.FullName}' (Ranking: {neuerKunde.Ranking}) wurde erfolgreich angelegt.",
                                "Kunde angelegt",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _selectedKunde.Vorname = vorname;
                _selectedKunde.Nachname = nachname;
                _selectedKunde.Adresse = adresse;
                _selectedKunde.Email = email;
                _selectedKunde.Telefon = telefon;
                _selectedKunde.Ranking = ranking;

                DatabaseService.UpdateKunde(_selectedKunde);
                _kundenCollectionView.Refresh();

                MessageBox.Show($"Die Stammdaten von '{_selectedKunde.FullName}' (Ranking: {_selectedKunde.Ranking}) wurden aktualisiert.",
                                "Kunde aktualisiert",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ResetForm();
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (_selectedKunde == null)
                return;

            var result = MessageBox.Show(
                $"Möchten Sie den Kunden '{_selectedKunde.FullName}' ({_selectedKunde.Email}) wirklich unwiderruflich löschen?",
                "Kunde löschen bestätigen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            string deletedName = _selectedKunde.FullName;
            DatabaseService.DeleteKunde(_selectedKunde);
            App.Kunden.Remove(_selectedKunde);

            ResetForm();

            MessageBox.Show($"Der Kunde '{deletedName}' wurde aus der Datenbank gelöscht.",
                            "Kunde gelöscht",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
