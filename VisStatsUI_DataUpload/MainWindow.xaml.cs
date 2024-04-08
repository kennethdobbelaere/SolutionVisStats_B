using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_DataUpload {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        IFileProcessor fileProcessor;
        IVisStatsRepository visStatsRepository;
        VisStatsManager visStatsManager;
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=PGVVisStats;Integrated Security=True; TrustServerCertificate = true";

        public MainWindow() {
            InitializeComponent();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";
            openFileDialog.InitialDirectory = @"C:\Users\kenne\Downloads\vis";
            openFileDialog.Multiselect = true;

            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(connectionString);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);
        }

       

        private void Button_Click_Vissoorten(object sender, RoutedEventArgs e) {
            bool? result=openFileDialog.ShowDialog();
            if (result == true) 
            {
                var fileNames=openFileDialog.FileNames;
                VissoortenFileListBox.ItemsSource = fileNames;
                openFileDialog.FileName = null;
            }
            else VissoortenFileListBox.ItemsSource=null;
        }

        private void Button_Click_UploadVissoorten(object sender, RoutedEventArgs e) {

            foreach (string fileName in VissoortenFileListBox.ItemsSource) {


                visStatsManager.UploadVissoorten(fileName);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }

        private void Button_Click_Havens(object sender, RoutedEventArgs e)
        {
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var fileNames = openFileDialog.FileNames;
                HavensFileListBox.ItemsSource = fileNames;
                openFileDialog.FileName = null;
            }
            else HavensFileListBox.ItemsSource = null;
        }

        private void Button_Click_UploadHavens(object sender, RoutedEventArgs e)
        {

            foreach (string fileName in HavensFileListBox.ItemsSource)
            {


                visStatsManager.UploadHavens(fileName);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }

        private void Button_Click_Statistieken(object sender, RoutedEventArgs e)
        {
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var fileNames = openFileDialog.FileNames;
                StatistiekenFileListBox.ItemsSource = fileNames;
                openFileDialog.FileName = null;
            }
            else StatistiekenFileListBox.ItemsSource = null;
        }
    

        private void Button_Click_UploadStatistieken(object sender, RoutedEventArgs e)
        {
            foreach (string fileName in StatistiekenFileListBox.ItemsSource)
            {


                visStatsManager.UploadStatistieken(fileName);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }
    }
}
