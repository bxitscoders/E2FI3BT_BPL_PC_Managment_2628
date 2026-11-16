using PCVerwaltung.Classes;
using PCVerwaltung.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<Case> Cases { get; } = new();
        public static ObservableCollection<CPU> CPUs { get; } = new();
        public static ObservableCollection<Mainboard> Mainboards { get; } = new();
        public static ObservableCollection<Ram> Rams { get; } = new();
        public static ObservableCollection<SSD> SSDs { get; } = new();
        public static ObservableCollection<PC> PCs { get; } = new();

        public App()
        {
            DatabaseService.InitializeDatabase();
            ReloadFromDatabase();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseService.InitializeDatabase();
            ReloadFromDatabase();
        }

        public static void ReloadFromDatabase()
        {
            Cases.Clear();
            foreach (var item in DatabaseService.LoadCases()) Cases.Add(item);

            CPUs.Clear();
            foreach (var item in DatabaseService.LoadCpus()) CPUs.Add(item);

            Mainboards.Clear();
            foreach (var item in DatabaseService.LoadMainboards()) Mainboards.Add(item);

            Rams.Clear();
            foreach (var item in DatabaseService.LoadRams()) Rams.Add(item);

            SSDs.Clear();
            foreach (var item in DatabaseService.LoadSsds()) SSDs.Add(item);

            PCs.Clear();
            foreach (var item in DatabaseService.LoadPcs()) PCs.Add(item);
        }

        // Instanz-Properties für DataContext = App.Current Datenbindung in XAML
        public ObservableCollection<Case> CasesList => Cases;
        public ObservableCollection<CPU> CPUsList => CPUs;
        public ObservableCollection<Mainboard> MainboardsList => Mainboards;
        public ObservableCollection<Ram> RamsList => Rams;
        public ObservableCollection<SSD> SSDsList => SSDs;
        public ObservableCollection<PC> PCsList => PCs;
    }
}
