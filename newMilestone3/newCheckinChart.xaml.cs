using Npgsql;
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
using System.Windows.Shapes;

namespace newMilestone3
{
    /// <summary>
    /// Interaction logic for newCheckinChart.xaml
    /// 
    /// 
    /// </summary>
    /// 
    public partial class newCheckinChart : Window
    {
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password = Ed734833; Database = milestone2";
        }

       
        public newCheckinChart(string selectedBusiness)
        {
            InitializeComponent();
            checkInColumnChart(selectedBusiness);
        }

        private void checkInColumnChart(string selectedBusiness)
        {
            List<KeyValuePair<string, int>> chartData = new List<KeyValuePair<string, int>>();
            using (var comm = new NpgsqlConnection(buildConnString()))
            {

                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT day, sum(morning + afternoon + evening + night) FROM checkin WHERE business_id = '" + selectedBusiness + "' GROUP BY day ORDER BY day; ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
                    }
                }
                comm.Close();
            }
            newCheckinChart1.DataContext = chartData;
        }
    }
}
