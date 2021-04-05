using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ScrapySharp.Network;
using System.Threading;
 

namespace CovScan
{
    

    public partial class MainWindow : Window
    {
        public ObservableCollection<ComboBoxItem> dpItems { get; set; }
        public ComboBoxItem SelecteddpItem { get; set; }
        public delegate void Del(string message);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tbSettingText.Text = "CoviScan - Etat [OK]";

            DataBuilding dataBuilding = new();
            dpItems = new ObservableCollection<ComboBoxItem>();
            dpItems.Add(new ComboBoxItem { Content = "Selectionner le département", Tag = "0" });
            SelecteddpItem = dpItems.First();

            dataBuilding.Build("https://static.data.gouv.fr/resources/departements-de-france/20200425-135513/departements-france.csv", "dep.csv", dpItems);

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Del hadler = DelegateMethod;
            tbSettingText.Text = "Démarrage de la recherche.........................";
            ObservableCollection<DataGrid> dataGrid = new();
            DataBuilding databuild = new();
            
            var mee = Convert.ToInt32(SelecteddpItem.Tag);

            dataG.ItemsSource = await Task.Factory.StartNew(
                () => databuild.grid(mee), TaskCreationOptions.LongRunning);

            tbSettingText.Text = "Recherche effectuée avec succès";
        }


        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Trace.WriteLine(link);
            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        public static void DelegateMethod(string message)
        {
            Console.WriteLine(message);
        }
    }
    }

