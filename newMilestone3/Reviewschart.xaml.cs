using Npgsql;
using System;
using System.IO;
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
using System.Device.Location;
using System.Data;
using System.ComponentModel;

namespace newMilestone3
{
    /// <summary>
    /// Interaction logic for Reviewschart.xaml
    /// </summary>
    public partial class Reviewschart : Window
    {
        public class Reviews2
        {
            public string Date { get; set; }
            public string UserName { get; set; }
            public string Stars { get; set; }
            public string Text { get; set; }
            public string Funny { get; set; }
            public string Useful { get; set; }
            public string Cool { get; set; }
        }
        public Reviewschart(string selectedBusiness)
        {
            InitializeComponent();
            ReviewsChart1(selectedBusiness);
        }

        private void ReviewsChart1(string selectedBusiness)
        {
           
            List<Reviews2> reviewData = new List<Reviews2>();
            using (var comm = new NpgsqlConnection("Host=localhost; Username=postgres; Password = Ed734833; Database = milestone2"))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT created_on, users.name, review.stars, review.review_text, review.funny, review.useful," +
                        " review.cool FROM business, review, users WHERE business.business_id = review.business_id " +
                        "AND users.user_id = review.user_id AND review.business_id = '" + selectedBusiness + "' ORDER BY review.created_on desc;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            reviewData.Add(new Reviews2() { Date = reader.GetDate(0).ToString(),
                                UserName = reader.GetString(1),
                                Stars = reader.GetDouble(2).ToString(),
                                Text = reader.GetString(3),
                                Funny = reader.GetInt32(4).ToString(),
                                Useful = reader.GetInt32(5).ToString(),
                                Cool = reader.GetInt32(6).ToString() });
                        }
                    }
                }
                comm.Close();
            }
            ReviewsView.ItemsSource = reviewData;
        }
    }
}
