using System;
using FileHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace CovScan
{

    public class DataBuilding
    {
        
        public ObservableCollection<DataGrid> grid(int selecteddpItem)
        {
            ObservableCollection<DataGrid> dataGrid = new ObservableCollection<DataGrid>();
            var systemPath = System.Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData
            );

            this.Dispatcher.Invoke(() => {
                textbox.Text = i.ToString();
            });

            string fullPath = systemPath + "/" + "mydatanew.xml";

            DataTable internalTable = new() { TableName = "Data" };
            internalTable.Columns.Add("id", typeof(int));
            internalTable.Columns.Add("departement", typeof(int));
            internalTable.Columns.Add("cp", typeof(int));
            internalTable.Columns.Add("nom", typeof(string));
            internalTable.Columns.Add("ville", typeof(string));
            internalTable.Columns.Add("web", typeof(string));
            internalTable.Columns.Add("internal_id", typeof(string));
            internalTable.Columns.Add("agenda", typeof(string));
            internalTable.Columns.Add("pfizer", typeof(string));
            internalTable.Columns.Add("moderna", typeof(string));
            internalTable.Columns.Add("aztrazeneca", typeof(string));
            internalTable.ReadXml(fullPath);

            var foundRows = internalTable.Select("departement =" + selecteddpItem);
     
            foreach (var r in foundRows)
            {
                var splited_id = Convert.ToString(r["internal_id"]).Split('-');

                dataGrid.Add(new DataGrid()
                {
                    departement = Convert.ToString(r["departement"]),
                    cp = Convert.ToInt32(r["cp"]),
                    nom = Convert.ToString(r["nom"]),
                    ville = Convert.ToString(r["ville"]),
                    web = Convert.ToString(r["web"]),
                    pfizer = this.search(splited_id[1], Convert.ToString(r["pfizer"]),
                        Convert.ToString(r["agenda"])),
                    moderna = this.search(splited_id[1], Convert.ToString(r["moderna"]),
                        Convert.ToString(r["agenda"])),
                    aztrazeneca = this.search(splited_id[1], Convert.ToString(r["aztrazeneca"]),
                        Convert.ToString(r["agenda"]))
                });
            }

            return dataGrid;
        }

        public class DataGrid
        {
            public string departement { get; set; }
            public int cp { get; set; }
            public string nom { get; set; }
            public string ville { get; set; }
            public string web { get; set; }
            public string pfizer { get; set; }
            public string moderna { get; set; }
            public string aztrazeneca { get; set; }
        }


        public string search(string idCentre, string motif, string agenda)
        {

            if (motif.Contains("/"))
            {
                return "Non proposé";
            }

            try
            {

                var webRequest =
                    WebRequest.Create(
                        @"https://partners.doctolib.fr/availabilities.json?start_date=2021-04-05&visit_motive_ids=" +
                        motif + "&agenda_ids=" + agenda + "&insurance_sector=public&practice_ids=" + idCentre +
                        "&destroy_temporary=true&limit=1");

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var json = reader.ReadToEnd();
                    Doctolib doctolib = JsonConvert.DeserializeObject<Doctolib>(json);

                    if (doctolib.Next_slot is null)
                        return doctolib.Message;
                    else
                    {
                        return doctolib.Next_slot;
                    }
                }
            }
            catch (Exception e)
            {
                return "Erreur";
            }
        }

        public class Doctolib
        {
            public string Next_slot { get; set; }
            public string Message { get; set; }
        }

        public void Build(string url, string name, ObservableCollection<ComboBoxItem> combobox)
        {

            var systemPath = System.Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData
            );
        
            string fullPath = systemPath + "/" + name;

            var engine = new FileHelperEngine(typeof(Gouv));
                var center = (Gouv[]) engine.ReadFile(fullPath);

                if (center.Any())
                {
                    
                    foreach (var data in center)
                    {
                      var combo = data.code_departement + " - " + data.nom_departement;
                      combobox.Add(new ComboBoxItem { Content = combo, Tag = data.code_departement });
                    }
                }


        }
        public void GetCsv(string url, string name)
        {
            System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            var systemPath = System.Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData
            );
            string fullPath = systemPath + "/"+name;
            File.WriteAllText(fullPath, results);
            sr.Close();
        }

    }

}
