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
    /// Interaktionslogik für CpuView.xaml
    /// </summary>
    public partial class CpuView : UserControl
    {
        public event Action<CPU>? Saved;

        public CpuView()
        {
            InitializeComponent();
            cmbSockel.ItemsSource = Enum.GetValues(typeof(SockelTyp)).Cast<SockelTyp>();
            cmbSockel.SelectedItem = SockelTyp.AM5;
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

            if (!TryParseDouble(txtTakt.Text, out double takt) || takt <= 0)
            {
                SetError(txtTakt, "Bitte eine gültige Taktfrequenz in GHz eingeben (z. B. 4.2).");
                ok = false;
            }

            if (!int.TryParse(txtKerne.Text.Trim(), out int kerne) || kerne <= 0)
            {
                SetError(txtKerne, "Bitte eine gültige Anzahl an Kernen eingeben (z. B. 8).");
                ok = false;
            }

            if (!TryParsePrice(txtEkPreis.Text, out decimal ekPreis))
            {
                SetError(txtEkPreis, "Bitte einen gültigen EK-Preis eingeben (z. B. 290,00).");
                ok = false;
            }

            if (!TryParsePrice(txtVkPreis.Text, out decimal vkPreis))
            {
                SetError(txtVkPreis, "Bitte einen gültigen VK-Preis eingeben (z. B. 379,00).");
                ok = false;
            }

            if (!ok) return;

            var data = new CPU(
                txtHersteller.Text.Trim(),
                txtModell.Text.Trim(),
                takt,
                (SockelTyp)cmbSockel.SelectedItem,
                ekPreis,
                vkPreis,
                kerne);

            Saved?.Invoke(data);

            MessageBox.Show(
                $"CPU erfolgreich gespeichert:\nModell: {data.Modell}\nSockel: {data.Sockel}\nTakt: {data.Taktfrequenz:0.##} GHz ({data.Kerne} Kerne)\nEK: {data.EkPreis:C} | VK: {data.VkPreis:C}",
                "CPU erfasst", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = string.Empty;
            txtModell.Text = string.Empty;
            txtTakt.Text = string.Empty;
            txtKerne.Text = "8";
            txtEkPreis.Text = string.Empty;
            txtVkPreis.Text = string.Empty;
            cmbSockel.SelectedIndex = 0;
            ClearAllErrors();
        }

        private static bool TryParseDouble(string text, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;
            text = text.Trim().Replace("GHz", "").Trim();
            return double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value) ||
                   double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
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
            ClearError(txtTakt);
            ClearError(txtKerne);
            ClearError(txtEkPreis);
            ClearError(txtVkPreis);
        }
        #endregion
    }
}
