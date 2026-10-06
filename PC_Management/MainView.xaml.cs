using PCVerwaltung.Classes;
using System;
using System.Windows;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
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
            view.Saved += data => App.Cases.Add(data);
            ContentHost.Content = view;
        }

        private void OnMenuMainboard(object sender, RoutedEventArgs e)
        {
            var view = new MainboardView();
            view.Saved += data => App.Mainboards.Add(data);
            ContentHost.Content = view;
        }

        private void OnMenuCpu(object sender, RoutedEventArgs e)
        {
            var view = new CpuView();
            view.Saved += data => App.CPUs.Add(data);
            ContentHost.Content = view;
        }

        private void OnMenuRam(object sender, RoutedEventArgs e)
        {
            var view = new RamView();
            view.Saved += data => App.Rams.Add(data);
            ContentHost.Content = view;
        }

        private void OnMenuSsd(object sender, RoutedEventArgs e)
        {
            var view = new SsdView();
            view.Saved += data => App.SSDs.Add(data);
            ContentHost.Content = view;
        }

        private void OnMenuPcSystemAssemble(object sender, RoutedEventArgs e) => ContentHost.Content = new PcBuilderView();

        private void OnMenuGenerateInvoice(object sender, RoutedEventArgs e) => ContentHost.Content = new InvoiceView();

        private void OnMenuAllLists(object sender, RoutedEventArgs e) => ContentHost.Content = new AllListsView();
    }
}
