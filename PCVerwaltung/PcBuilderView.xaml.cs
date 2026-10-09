
using PCVerwaltung.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PCVerwaltung
{
    public partial class PcBuilderView : UserControl
    {
        public PcBuilderView()
        {
            InitializeComponent();

            // Komponentenlisten aus App.xaml.cs laden
            CmbCase.ItemsSource = App.Cases;
            CmbMainboard.ItemsSource = App.Mainboards;
            CmbCpu.ItemsSource = App.CPUs;
            CmbSsd.ItemsSource = App.SSDs;

            // Gespeicherte PCs anzeigen
            dgPCs.ItemsSource = App.PCs;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Ausgewählte Komponenten auslesen
            var selCase = CmbCase.SelectedItem as Case;
            var selMb = CmbMainboard.SelectedItem as Mainboard;
            var selCpu = CmbCpu.SelectedItem as CPU;
            var selSsd = CmbSsd.SelectedItem as SSD;

            // Prüfen, ob alle Komponenten ausgewählt wurden
            if (selCase is null ||
                selMb is null ||
                selCpu is null ||
                selSsd is null)
            {
                MessageBox.Show(
                    "Bitte Gehäuse, Mainboard, CPU und SSD auswählen.",
                    "Auswahl unvollständig",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // Gehäuse und Mainboard auf Kompatibilität prüfen
            bool fits = selCase.Formfaktor switch
            {
                Formfaktor.ATX => true,

                Formfaktor.MicroATX =>
                    selMb.Formfaktor == Formfaktor.MicroATX ||
                    selMb.Formfaktor == Formfaktor.MiniITX,

                Formfaktor.MiniITX =>
                    selMb.Formfaktor == Formfaktor.MiniITX,

                _ => false
            };

            if (!fits)
            {
                MessageBox.Show(
                    $"Formfaktor nicht kompatibel:\n" +
                    $"Gehäuse: {selCase.Formfaktor}\n" +
                    $"Mainboard: {selMb.Formfaktor}",
                    "Inkompatible Komponenten",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // CPU und Mainboard auf Sockel-Kompatibilität prüfen
            // Nur wenn die CPU eine Sockel-Property besitzt
            var cpuSockelProp = typeof(CPU).GetProperty("Sockel");

            if (cpuSockelProp != null)
            {
                var cpuSockel = cpuSockelProp.GetValue(selCpu);

                if (!Equals(cpuSockel, selMb.Sockel))
                {
                    MessageBox.Show(
                        $"Sockel nicht kompatibel:\n" +
                        $"CPU: {cpuSockel}\n" +
                        $"Mainboard: {selMb.Sockel}",
                        "Inkompatible Komponenten",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }
            }

            // Neuen PC erstellen
            var pc = new PC(
                selCase,
                selCpu,
                selMb,
                selSsd
            );

            // PC in Gesamtübersicht speichern
            App.PCs.Add(pc);

            // Tabelle aktualisieren
            dgPCs.Items.Refresh();

            // Erfolgsmeldung
            MessageBox.Show(
                "PC wurde erfolgreich zur Liste hinzugefügt.",
                "Gespeichert",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}
