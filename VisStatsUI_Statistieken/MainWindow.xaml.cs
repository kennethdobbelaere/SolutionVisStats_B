using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsBL.Model;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_Statistieken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        FileProcessor fileProcessor;
        IVisStatsRepository visStatsRepository;
        VisStatsManager visStatsManager;
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=PGVVisStats;Integrated Security=True; TrustServerCertificate = true";
        ObservableCollection<Vissoort> AlleVissoorten;
        ObservableCollection<Vissoort> GeselecteerdeVissoorten;
        public MainWindow()
        {
            InitializeComponent();
            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(connectionString);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);
            HavensComboBox.ItemsSource = visStatsManager.GeefHavens();
            HavensComboBox.SelectedIndex = 0;
            JaarComboBox.ItemsSource = visStatsRepository.LeesJaartallen();
            JaarComboBox.SelectedIndex = 0;
            AlleVissoorten = new ObservableCollection<Vissoort>(visStatsManager.GeefVissoorten());
            AlleSoortenListBox.ItemsSource = AlleVissoorten;
            GeselecteerdeVissoorten = new ObservableCollection<Vissoort>();
            GeselecteerdeSoortenListBox.ItemsSource = GeselecteerdeVissoorten;
        }

        private void VoegAlleSoortenToeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(Vissoort v in AlleVissoorten)
            {
                GeselecteerdeVissoorten.Add(v);
            }
            AlleVissoorten.Clear();
        }   

        private void VoegSoortenToeButton_Click(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in AlleSoortenListBox.SelectedItems) soorten.Add(v);
            foreach(Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Add(v);
                AlleVissoorten.Remove(v);
            }
        }

        private void VerwijderdSoortenButton_Click(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in GeselecteerdeSoortenListBox.SelectedItems) soorten.Add(v);
            foreach (Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Remove(v);
                AlleVissoorten.Add(v);
            }
        }

        private void VerwijderAlleSoortenButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Vissoort v in GeselecteerdeVissoorten)
            {
                AlleVissoorten.Add(v);
            }
            GeselecteerdeVissoorten.Clear();

        }
        private void ToonStatistiekenButton_Click(Object sender, RoutedEventArgs e)
        {
            Eenheid eenheid;
            if ((bool)kgRadioButton.IsChecked) eenheid = Eenheid.kg; else eenheid = Eenheid.euro;
            List<JaarVangst> vangst = visStatsManager.GeefVangst((int)JaarComboBox.SelectedItem, (Haven)HavensComboBox.SelectedItem, GeselecteerdeVissoorten.ToList(), eenheid);
            StatistiekenWindow w = new StatistiekenWindow((int)JaarComboBox.SelectedItem,(Haven)HavensComboBox.SelectedItem,eenheid,vangst);
            w.ShowDialog();
        }
    }
}
