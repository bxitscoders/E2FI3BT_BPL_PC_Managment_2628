using PCVerwaltung.Classes;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für SsdView.xaml
    /// </summary>
    public partial class SsdView : UserControl
    {
        public event Action<SSD>? Saved;

        public SsdView()
        {
            InitializeComponent();
            cmbTyp.ItemsSource = Enum.GetValues(typeof(SsdTyp)).Cast<SsdTyp>();
            cmbTyp.SelectedItem = SsdTyp.NVMe;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            ClearAllErrors();

            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtHersteller.Text))
            {
                SetError(txtHersteller, "Bitte Hersteller angeben.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtModell.Text))
            {
                SetError(txtModell, "Bitte Modell angeben.");
                ok = false;
            }

            if (!int.TryParse(txtKapazitaet.Text.Trim(), out int kapazitaet) || kapazitaet <= 0)
            {
                SetError(txtKapazitaet, "Bitte eine gültige Kapazität in GB angeben (z. B. 500, 1000 oder 2000).");
                ok = false;
            }

            if (!int.TryParse(txtLesen.Text.Trim(), out int lesen) || lesen <= 0)
            {
                SetError(txtLesen, "Bitte eine gültige Lesegeschwindigkeit in MB/s angeben (z. B. 7000).");
                ok = false;
            }

            if (!TryParsePrice(txtEkPreis.Text, out decimal ekPreis))
            {
                SetError(txtEkPreis, "Bitte einen gültigen EK-Preis eingeben (z. B. 75,00).");
                ok = false;
            }

            if (!TryParsePrice(txtVkPreis.Text, out decimal vkPreis))
            {
                SetError(txtVkPreis, "Bitte einen gültigen VK-Preis eingeben (z. B. 109,90).");
                ok = false;
            }

            if (!ok) return;

            var data = new SSD(
                txtHersteller.Text.Trim(),
                txtModell.Text.Trim(),
                (SsdTyp)cmbTyp.SelectedItem,
                kapazitaet,
                lesen,
                ekPreis,
                vkPreis);

            Saved?.Invoke(data);

            MessageBox.Show(
                $"SSD erfolgreich gespeichert:\nHersteller: {data.Hersteller}\nModell: {data.Modell}\nTyp: {data.Typ} | {data.KapazitaetGB} GB ({data.LesegeschwindigkeitMBs} MB/s)\nEK: {data.EkPreis:C} | VK: {data.VkPreis:C}",
                "SSD erfasst", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = string.Empty;
            txtModell.Text = string.Empty;
            txtKapazitaet.Text = "1000";
            txtLesen.Text = "7000";
            txtEkPreis.Text = string.Empty;
            txtVkPreis.Text = string.Empty;
            cmbTyp.SelectedIndex = 0;
            ClearAllErrors();
        }

        private static bool TryParsePrice(string text, out decimal price)
        {
            price = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;
            text = text.Trim().Replace("€", "").Trim();
            return (decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out price) ||
                    decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out price)) && price >= 0;
        }

        #region Validation Helpers
        private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60));

        private void SetError(Control c, string msg)
        {
            c.BorderBrush = ErrorBrush;
            c.BorderThickness = new Thickness(1.5);
            c.ToolTip = msg;
        }

        private void ClearError(Control c)
        {
            c.ClearValue(Border.BorderBrushProperty);
            c.ClearValue(Border.BorderThicknessProperty);
            c.ClearValue(ToolTipProperty);
        }

        private void ClearAllErrors()
        {
            ClearError(txtHersteller);
            ClearError(txtModell);
            ClearError(txtKapazitaet);
            ClearError(txtLesen);
            ClearError(txtEkPreis);
            ClearError(txtVkPreis);
        }
        #endregion
    }
}
