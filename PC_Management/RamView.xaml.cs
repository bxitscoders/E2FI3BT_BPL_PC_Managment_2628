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
    /// Interaktionslogik für RamView.xaml
    /// </summary>
    public partial class RamView : UserControl
    {
        public event Action<Ram>? Saved;

        public RamView()
        {
            InitializeComponent();
            cmbTyp.ItemsSource = Enum.GetValues(typeof(RamTyp)).Cast<RamTyp>();
            cmbTyp.SelectedItem = RamTyp.DDR5;
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
                SetError(txtKapazitaet, "Bitte eine gültige Kapazität in GB angeben (z. B. 16 oder 32).");
                ok = false;
            }

            if (!int.TryParse(txtTakt.Text.Trim(), out int takt) || takt <= 0)
            {
                SetError(txtTakt, "Bitte eine gültige Taktfrequenz in MHz angeben (z. B. 5600 oder 6000).");
                ok = false;
            }

            if (!TryParsePrice(txtEkPreis.Text, out decimal ekPreis))
            {
                SetError(txtEkPreis, "Bitte einen gültigen EK-Preis eingeben (z. B. 68,00).");
                ok = false;
            }

            if (!TryParsePrice(txtVkPreis.Text, out decimal vkPreis))
            {
                SetError(txtVkPreis, "Bitte einen gültigen VK-Preis eingeben (z. B. 94,90).");
                ok = false;
            }

            if (!ok) return;

            var data = new Ram(
                txtHersteller.Text.Trim(),
                txtModell.Text.Trim(),
                (RamTyp)cmbTyp.SelectedItem,
                kapazitaet,
                takt,
                ekPreis,
                vkPreis);

            Saved?.Invoke(data);

            MessageBox.Show(
                $"RAM erfolgreich gespeichert:\nHersteller: {data.Hersteller}\nModell: {data.Modell}\nTyp: {data.Typ} | {data.KapazitaetGB} GB @ {data.TaktfrequenzMHz} MHz\nEK: {data.EkPreis:C} | VK: {data.VkPreis:C}",
                "RAM erfasst", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = string.Empty;
            txtModell.Text = string.Empty;
            txtKapazitaet.Text = "16";
            txtTakt.Text = "5600";
            txtEkPreis.Text = string.Empty;
            txtVkPreis.Text = string.Empty;
            cmbTyp.SelectedIndex = 1;
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
            ClearError(txtTakt);
            ClearError(txtEkPreis);
            ClearError(txtVkPreis);
        }
        #endregion
    }
}
