using System.Windows;
using PCVerwaltung.Classes;
using PCVerwaltung.Data;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            ContentHost.Content = new AllListsView();
        }

        private void OnMenuGehaeuse(object sender, RoutedEventArgs e)
        {
            var view = new GehaeuseView();
            view.Saved += data =>
            {
                DatabaseService.SaveCase(data);
                App.Cases.Add(data);
            };
            ContentHost.Content = view;
        }

        private void OnMenuMainboard(object sender, RoutedEventArgs e)
        {
            var view = new MainboardView();
            view.Saved += data =>
            {
                DatabaseService.SaveMainboard(data);
                App.Mainboards.Add(data);
            };
            ContentHost.Content = view;
        }

        private void OnMenuCpu(object sender, RoutedEventArgs e)
        {
            var view = new CpuView();
            view.Saved += data =>
            {
                DatabaseService.SaveCpu(data);
                App.CPUs.Add(data);
            };
            ContentHost.Content = view;
        }

        private void OnMenuRam(object sender, RoutedEventArgs e)
        {
            var view = new RamView();
            view.Saved += data =>
            {
                DatabaseService.SaveRam(data);
                App.Rams.Add(data);
            };
            ContentHost.Content = view;
        }

        private void OnMenuSsd(object sender, RoutedEventArgs e)
        {
            var view = new SsdView();
            view.Saved += data =>
            {
                DatabaseService.SaveSsd(data);
                App.SSDs.Add(data);
            };
            ContentHost.Content = view;
        }

        private void OnMenuPcSystemAssemble(object sender, RoutedEventArgs e) => ContentHost.Content = new PcBuilderView();

        private void OnMenuGenerateInvoice(object sender, RoutedEventArgs e) => ContentHost.Content = new InvoiceView();

        private void OnMenuAllLists(object sender, RoutedEventArgs e) => ContentHost.Content = new AllListsView();
    }
}
