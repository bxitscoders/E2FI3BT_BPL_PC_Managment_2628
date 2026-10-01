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
    /// Interaktionslogik für GehaeuseView.xaml
    /// </summary>
    public partial class GehaeuseView : UserControl
    {
        public event Action<Case>? Saved;

        public GehaeuseView()
        {
            InitializeComponent();
            cmbFormfaktor.ItemsSource = Enum.GetValues(typeof(Formfaktor)).Cast<Formfaktor>();
            cmbFormfaktor.SelectedItem = Formfaktor.ATX;
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

            if (!TryParsePrice(txtEkPreis.Text, out decimal ekPreis))
            {
                SetError(txtEkPreis, "Bitte einen gültigen EK-Preis eingeben (z. B. 85,00).");
                ok = false;
            }

            if (!TryParsePrice(txtVkPreis.Text, out decimal vkPreis))
            {
                SetError(txtVkPreis, "Bitte einen gültigen VK-Preis eingeben (z. B. 119,90).");
                ok = false;
            }

            if (!ok) return;

            var data = new Case(
                txtHersteller.Text.Trim(),
                txtModell.Text.Trim(),
                (Formfaktor)cmbFormfaktor.SelectedItem,
                ekPreis,
                vkPreis);

            Saved?.Invoke(data);

            MessageBox.Show(
                $"Gehäuse erfolgreich gespeichert:\nHersteller: {data.Hersteller}\nModell: {data.Modell}\nFormfaktor: {data.Formfaktor}\nEK: {data.EkPreis:C} | VK: {data.VkPreis:C}",
                "Gehäuse erfasst", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = string.Empty;
            txtModell.Text = string.Empty;
            txtEkPreis.Text = string.Empty;
            txtVkPreis.Text = string.Empty;
            cmbFormfaktor.SelectedIndex = 0;
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
        private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60)); // Crimson

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
            ClearError(txtEkPreis);
            ClearError(txtVkPreis);
        }
        #endregion
    }
}
