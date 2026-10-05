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
            UpdateLiveCalculation();
            UpdateCalculatedIp();
            Loaded += (s, e) =>
            {
                UpdateLiveCalculation();
                UpdateCalculatedIp();
            };
        }

        private void OnComponentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLiveCalculation();
        }

        private void OnNetworkConfigChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCalculatedIp();
        }

        private void OnAutoIpChanged(object sender, RoutedEventArgs e)
        {
            if (TxtPcIp == null || ChkAutoIp == null)
                return;

            bool isAuto = ChkAutoIp.IsChecked == true;
            TxtPcIp.IsReadOnly = isAuto;

            if (isAuto)
            {
                UpdateCalculatedIp();
            }
        }

        private void UpdateCalculatedIp()
        {
            if (TxtPcIp == null || TxtRouterIp == null || TxtIpInfo == null || ChkAutoIp == null)
                return;

            if (ChkAutoIp.IsChecked != true)
                return;

            string routerIp = TxtRouterIp.Text?.Trim() ?? string.Empty;

            if (NetworkConfig.IsValidIpv4(routerIp))
            {
                string nextIp = NetworkConfig.GetNextAvailableIp(routerIp, App.PCs);
                TxtPcIp.Text = nextIp;
                TxtIpInfo.Text = $"Automatisch: Erste freie IP nach Router ({routerIp})";
                TxtIpInfo.Foreground = Brushes.Green;
            }
            else
            {
                TxtIpInfo.Text = "Ungültige Router-IP (Format: z. B. 192.168.1.1)";
                TxtIpInfo.Foreground = Brushes.Red;
            }
        }

        private void UpdateLiveCalculation()
        {
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

            // 3. Validierung: Netzwerk / IP-Adresse (US-03)
            string routerIp = TxtRouterIp.Text?.Trim() ?? string.Empty;
            string pcIp = TxtPcIp.Text?.Trim() ?? string.Empty;

            if (!NetworkConfig.IsValidIpv4(pcIp))
            {
                MessageBox.Show($"Bitte geben Sie eine gültige IPv4-Adresse für das PC-System ein (z. B. 192.168.1.7).",
                                "Ungültige IP-Adresse",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPcIp.Focus();
                return;
            }

            if (pcIp.Equals(routerIp, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Die IP-Adresse des PCs ({pcIp}) darf nicht mit der Router-IP ({routerIp}) identisch sein.",
                                "IP-Konflikt mit Router",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPcIp.Focus();
                return;
            }

            if (App.PCs.Any(p => p.IpAdresse.Equals(pcIp, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Die IP-Adresse '{pcIp}' ist bereits an ein anderes konfiguriertes PC-System vergeben.\nBitte wählen Sie eine freie IP-Adresse.",
                                "IP-Adresse bereits belegt",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPcIp.Focus();
                return;
            }

            // 4. Validierung: Alle 5 Komponenten ausgewählt
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

            // 5. Kompatibilitätsprüfung
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

            // 6. System mit vorkonfigurierter IP erstellen & speichern
            var pc = new PC(systemName, selCase, selCpu, selMb, selRam, selSsd, pcIp);
            App.PCs.Add(pc);

            // 7. Formular für den nächsten PC vorbereiten
            TxtSystemName.Clear();
            UpdateCalculatedIp(); // Zählt für den nächsten PC automatisch hoch!

            MessageBox.Show($"Das PC-System '{pc.Name}' wurde erfolgreich gespeichert!\n\n" +
                            $"• IP-Adresse: {pc.IpAdresse}\n" +
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
