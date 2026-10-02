using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PCVerwaltung.Classes;

namespace PCVerwaltung
{
    public partial class PcBuilderView : UserControl
    {
        public PcBuilderView()
        {
            InitializeComponent();
            DataContext = (App)Application.Current;
            Loaded += (s, e) => UpdateLiveCalculation();
        }

        private void OnComponentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLiveCalculation();
        }

        private void UpdateLiveCalculation()
        {
            // Null-Check vor Initialisierung aller UI-Elemente
            if (TxtLiveEk == null || TxtLiveVk == null || TxtLiveMarge == null || TxtStatus == null)
                return;

            var selCase = CmbCase?.SelectedItem as Case;
            var selMb = CmbMainboard?.SelectedItem as Mainboard;
            var selCpu = CmbCpu?.SelectedItem as CPU;
            var selRam = CmbRam?.SelectedItem as Ram;
            var selSsd = CmbSsd?.SelectedItem as SSD;

            decimal totalEk = (selCase?.EkPreis ?? 0) +
                              (selMb?.EkPreis ?? 0) +
                              (selCpu?.EkPreis ?? 0) +
                              (selRam?.EkPreis ?? 0) +
                              (selSsd?.EkPreis ?? 0);

            decimal totalVk = (selCase?.VkPreis ?? 0) +
                              (selMb?.VkPreis ?? 0) +
                              (selCpu?.VkPreis ?? 0) +
                              (selRam?.VkPreis ?? 0) +
                              (selSsd?.VkPreis ?? 0);

            decimal marge = totalVk - totalEk;

            TxtLiveEk.Text = $"{totalEk:0.00} €";
            TxtLiveVk.Text = $"{totalVk:0.00} €";
            TxtLiveMarge.Text = $"{marge:0.00} €";

            // Kompatibilitätsprüfung
            string? incompatibility = GetIncompatibilityReason(selCase, selMb, selCpu);
            if (incompatibility == null)
            {
                TxtStatus.Text = "✅ Alle Komponenten sind kompatibel";
                TxtStatus.Foreground = Brushes.Green;
            }
            else
            {
                TxtStatus.Text = $"⚠️ {incompatibility}";
                TxtStatus.Foreground = Brushes.DarkOrange;
            }
        }

        private static string? GetIncompatibilityReason(Case? selCase, Mainboard? selMb, CPU? selCpu)
        {
            if (selCase != null && selMb != null)
            {
                bool formfaktorFits = selCase.Formfaktor switch
                {
                    Formfaktor.ATX => true,
                    Formfaktor.MicroATX => selMb.Formfaktor == Formfaktor.MicroATX || selMb.Formfaktor == Formfaktor.MiniITX,
                    Formfaktor.MiniITX => selMb.Formfaktor == Formfaktor.MiniITX,
                    _ => false
                };

                if (!formfaktorFits)
                {
                    return $"Formfaktor inkompatibel: Gehäuse ({selCase.Formfaktor}) ↔ Board ({selMb.Formfaktor})";
                }
            }

            if (selCpu != null && selMb != null)
            {
                if (selCpu.Sockel != selMb.Sockel)
                {
                    return $"Sockel inkompatibel: CPU ({selCpu.Sockel}) ↔ Board ({selMb.Sockel})";
                }
            }

            return null;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            string systemName = TxtSystemName.Text?.Trim() ?? string.Empty;

            // 1. Validierung: Systemname vorhanden
            if (string.IsNullOrWhiteSpace(systemName))
            {
                MessageBox.Show("Bitte geben Sie einen eindeutigen Systemnamen für die PC-Konfiguration ein.",
                                "Systemname fehlt",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtSystemName.Focus();
                return;
            }

            // 2. Validierung: Eindeutigkeit des Systemnamens
            if (App.PCs.Any(p => p.Name.Equals(systemName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Ein PC-System mit dem Namen '{systemName}' existiert bereits.\nBitte vergeben Sie einen eindeutigen Namen.",
                                "Name bereits vergeben",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtSystemName.Focus();
                return;
            }

            // 3. Validierung: Alle 5 Komponenten ausgewählt
            var selCase = CmbCase.SelectedItem as Case;
            var selMb = CmbMainboard.SelectedItem as Mainboard;
            var selCpu = CmbCpu.SelectedItem as CPU;
            var selRam = CmbRam.SelectedItem as Ram;
            var selSsd = CmbSsd.SelectedItem as SSD;

            if (selCase is null || selMb is null || selCpu is null || selRam is null || selSsd is null)
            {
                MessageBox.Show("Bitte wählen Sie für alle 5 Hardwarekomponenten (Gehäuse, Mainboard, CPU, RAM und SSD) ein Modell aus.",
                                "Auswahl unvollständig",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 4. Kompatibilitätsprüfung
            string? incompatibility = GetIncompatibilityReason(selCase, selMb, selCpu);
            if (incompatibility != null)
            {
                var result = MessageBox.Show(
                    $"Achtung: Die gewählte Hardware ist nicht vollständig kompatibel:\n\n{incompatibility}\n\nMöchten Sie das System trotzdem speichern?",
                    "Kompatibilitätswarnung",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;
            }

            // 5. System erstellen & speichern
            var pc = new PC(systemName, selCase, selCpu, selMb, selRam, selSsd);
            App.PCs.Add(pc);

            // 6. Formular zurücksetzen & Erfolgsmeldung
            TxtSystemName.Clear();
            MessageBox.Show($"Das PC-System '{pc.Name}' wurde erfolgreich gespeichert!\n\n" +
                            $"• CPU: {selCpu.Hersteller} {selCpu.Modell}\n" +
                            $"• RAM: {selRam.KapazitaetGB} GB ({selRam.Typ})\n" +
                            $"• SSD: {selSsd.KapazitaetGB} GB ({selSsd.Typ})\n" +
                            $"• Board: {selMb.Hersteller} {selMb.Modell}\n" +
                            $"• Gehäuse: {selCase.Hersteller} {selCase.Modell}\n\n" +
                            $"Gesamtverkaufspreis: {pc.GesamtVkPreis:0.00} €",
                            "PC erfolgreich gespeichert",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
