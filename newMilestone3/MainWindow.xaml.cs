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
    // TEAM CE 
    // Edwin Aguilera, Connor Hamilton
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
        
            InitializeComponent();
           AddStates();
            AddColumnsToFGrid();
            AddColumnsToReviewsGrid();
            AddColToBGrid();
            
             Get_days();
            //addRatingChoices();
            popualteFromTimeBox();
            popualteToTimeBox();
            populateSortComboBox();
            addRatings();
        }
       
        static class RandomUtil
        {
            /// <summary>
            /// Get random string of 11 characters.
            /// </summary>
            /// <returns>Random string.</returns>
            public static string GetRandomString()
            {
                string path = System.IO.Path.GetRandomFileName();
                path = path.Replace(".", ""); // Remove period.
                return path;
            }
        }
        public class Business
        {
          
            public string business_id { get; set; }
            public string name { get; set; }
            public double stars { get; set; }
            public int review_Count { get; set; }
            public int isOpen { get; set; }// 1 for when business is open 0 for closed.
            public int num_checkIns { get; set; }
            public double review_Rating { get; set; }
            public double distance { get; set; }
            public string address { get; set; }
       
            public string state { get; set; }
            public string city { get; set; }
            public string zipcode { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }

       

        }

        public class Friend
        {
            public string friend_name { get; set; }

            public string Yelping_since { get; set;}

            public double Friend_stars { get; set; }

            public string Friend_ID { get; set; }
        }
        public class User_Distance
        {
            public double User_latitude { get; set; }
            public double User_longitude { get; set; }

            public double distance { get; set; }
        }


        User_Distance user = new User_Distance();

        public class UserOBJ
        {
            public string user_id { get; set; }
            public string name { get; set; }
            public double average_stars { get; set; }
            public string yelping_since { get; set; }
            public int funny { get; set; }
            public int cool { get; set; }
            public int useful { get; set; }
        }
        public class Reviews
        {
            public string ReviewUserName { get; set; }

            public string ReviewBusinessName { get; set; }

            public string ReviewCity { get; set; }

            public string ReviewText { get; set; }
        }


        private string buildConnString()
        {
            
            return "Host=localhost; Username=postgres; Password = Ed734833; Database = milestone2";

        }

        public void AddColumnsToFGrid()// add cols to friends grid
        {
            DataGridTextColumn name = new DataGridTextColumn();
            name.Header = "Name";
            name.Binding = new Binding("friend_name");
            FriendsDGrid.Columns.Add(name);

            DataGridTextColumn avgStars = new DataGridTextColumn();
            avgStars.Header = "Avg Stars";
            avgStars.Binding = new Binding("Friend_stars");
            FriendsDGrid.Columns.Add(avgStars);

            DataGridTextColumn yelpingSince = new DataGridTextColumn();
            yelpingSince.Header = "Yelping Since";
            yelpingSince.Binding = new Binding("Yelping_since");
            FriendsDGrid.Columns.Add(yelpingSince);
        }

        public void AddColumnsToReviewsGrid()
        {
            DataGridTextColumn userName = new DataGridTextColumn();
            userName.Header = "User Name";
            userName.Binding = new Binding("ReviewUserName");
            ReviewsByFriendsDataGrid.Columns.Add(userName);

            DataGridTextColumn business = new DataGridTextColumn();
            business.Header = "Business";
            business.Binding = new Binding("ReviewBusinessName");
            ReviewsByFriendsDataGrid.Columns.Add(business);

            DataGridTextColumn city = new DataGridTextColumn();
            city.Header = "City";
            city.Binding = new Binding("ReviewCity");
            ReviewsByFriendsDataGrid.Columns.Add(city);

            DataGridTextColumn text = new DataGridTextColumn();
            text.Header = "Text";
            text.Binding = new Binding("ReviewText");
            ReviewsByFriendsDataGrid.Columns.Add(text);
        }


        //This is the bussiness information side 

        public void AddColToBGrid()
        {
            // used examples from last milestone
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Business Name";
            column.Binding = new Binding("name");
            BGRID.Columns.Add(column);

            DataGridTextColumn column1 = new DataGridTextColumn();
            column1.Header = "Address";
            column1.Binding = new Binding("address");
            BGRID.Columns.Add(column1);

            DataGridTextColumn column2 = new DataGridTextColumn();
            column2.Header = "City";
            column2.Binding = new Binding("city");
            BGRID.Columns.Add(column2);

            DataGridTextColumn column3 = new DataGridTextColumn();
            column3.Header = "State";
            column3.Binding = new Binding("state");
            BGRID.Columns.Add(column3);

            DataGridTextColumn column4 = new DataGridTextColumn();
            column4.Header = "Distance (miles)";
            column4.Binding = new Binding("distance");
            BGRID.Columns.Add(column4);

            DataGridTextColumn column5 = new DataGridTextColumn();
            column5.Header = "Stars";
            column5.Binding = new Binding("stars");
            BGRID.Columns.Add(column5);

            DataGridTextColumn column6 = new DataGridTextColumn();
            column6.Header = "# of Reviews";
            column6.Binding = new Binding("review_Count");
            BGRID.Columns.Add(column6);

            DataGridTextColumn column7 = new DataGridTextColumn();
            column7.Header = "Avg Review Rating";
            column7.Binding = new Binding("review_Rating");
            BGRID.Columns.Add(column7);

            DataGridTextColumn column8 = new DataGridTextColumn();
            column8.Header = "Total CheckIns";
            column8.Binding = new Binding("num_checkIns");
            BGRID.Columns.Add(column8);
        }
        public void AddStates()
        {
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stateList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }

        public void populateSortComboBox()
        {
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Business Name", Tag = "ORDER BY name" });
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Highest Rating", Tag = "ORDER BY stars DESC" });
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Most Reviews", Tag = "ORDER BY review_count DESC" });
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Best Review Rating", Tag = "ORDER BY reviewrating DESC" });
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Most Checkins", Tag = "ORDER BY numCheckins DESC" });
            sortComboBox.Items.Add(new ComboBoxItem() { Content = "Nearest", Tag = "ORDER BY distance" });

            sortComboBox.SelectedIndex = 0;
        }

        public void AddCities()
        {
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                cityListBox.Items.Clear();
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT DISTINCT city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "'ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }
        public void AddZips()
        {
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                ZipListBOX.Items.Clear();
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT DISTINCT postal_code FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' and city='" + cityListBox.SelectedItem.ToString() + "' ORDER BY postal_code;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ZipListBOX.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }

        public void AddBusiness()
        {
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                CatListBox.Items.Clear();
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    //SELECT * FROM businesscategory as bc Natural JOIN business as b WHERE b.business_id = bc.business_id
                    cmd.CommandText = "SELECT DISTINCT category FROM businesscategory as bc NATURAL JOIN business as b WHERE state = '" + stateList.SelectedItem.ToString() + "' and city='" + cityListBox.SelectedItem.ToString() + "' " +
                        "and postal_code = '" + ZipListBOX.SelectedItem.ToString() + "' and b.business_id = bc.business_id  ORDER BY category;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }


       



        private void setLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((UserLatitudeTextBox.Text != null && UserLatitudeTextBox.Text != "") && (UserLongitudeTextBox.Text != null && UserLongitudeTextBox.Text != ""))
            {
                user.User_latitude = Convert.ToDouble(UserLatitudeTextBox.Text);
                user.User_longitude = Convert.ToDouble(UserLongitudeTextBox.Text);
            }
        }

        private void FriendsDGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BGRID.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                AddCities();
            }
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            updateBGRID();
        }

        private void Get_days()
        {
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                dayComboBox.Items.Clear();
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT DISTINCT day  FROM businesshours";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dayComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }
        private void updateBGRID()
        {
            if (CatListBox.SelectedIndex > -1 && cityListBox.SelectedIndex > -1 && stateList.SelectedIndex > -1 && ZipListBOX.SelectedIndex > -1)
            {
                BGRID.Items.Clear();
                StringBuilder sb = new StringBuilder();

                using (var comm = new NpgsqlConnection(buildConnString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {

                        string SQL_string = "";

                        if (dayComboBox.SelectedIndex > -1 && ToTimeBox.SelectedIndex > -1 && FromTimeBox.SelectedIndex >-1)
                        {
                            SQL_string += " AND businesshours.day = '" + dayComboBox.SelectedItem.ToString() + "' AND " +
                                                    "businesshours.open = '" + FromTimeBox.SelectedItem.ToString() + "' AND " +
                                                    "businesshours.close = '" + ToTimeBox.SelectedItem.ToString() + "'   ";
                        }

                        cmd.Connection = comm;


                        // THis will check if more than 1 catagory has been added to the category box by user
                        if (selectedCategoriesListBox.Items.Count >= 1)
                        {
                            sb.Append("FROM business WHERE business.business_id IN (SELECT business_id FROM businesscategory WHERE category = '" + selectedCategoriesListBox.Items[0].ToString() + "') ");
                            for (int i = 1; i < selectedCategoriesListBox.Items.Count; i++)
                            {
                                sb.Append("AND business.business_id IN (SELECT business_id FROM businesscategory WHERE category = '" + selectedCategoriesListBox.Items[i].ToString() + "') ");
                            }
                        }
                        else
                        { // this handeled is user has selected somthing on the category list but not added it to the box
                            sb.Append("FROM businesscategory WHERE category = '" + CatListBox.SelectedItem.ToString() + "' ");
                        }

                        string builtString = sb.ToString();
                        string end_sql = "";

                        if(sortComboBox.SelectedIndex > -1)
                        {
                            end_sql = sortComboBox.SelectedItem.ToString();
                        }

                        //cmd.CommandText = "SELECT * FROM business NATURAL JOIN review NATURAL JOIN businesscategory " +
                        //    "WHERE state ='" + stateList.SelectedItem.ToString() + "' and city='" + cityListBox.SelectedItem.ToString() + "' " +
                        //    "and postal_code = '" + ZipListBOX.SelectedItem.ToString() + "'ORDER BY name; ";

                        //SELECT* FROM business, (SELECT DISTINCT business_id as busID FROM business WHERE business.business_id IN(SELECT business_id FROM businesscategory WHERE category = 'American (Traditional)')

                        //AND business.business_id IN(SELECT business_id FROM businesscategory

                        //WHERE category = 'Burgers') AND business.business_id IN(SELECT business_id

                        //FROM businesscategory WHERE category = 'Fast Food') ) a WHERE state = 'AZ'

                        //and city = 'Chandler' and postal_code = '85226' and a.busID = business.business_id

                        //ORDER BY name ASC;


                        // major issue is that because natural join businesshour, it will bring duplicated but i need it for the filter by hours and day 
                        SQL_string += ((ComboBoxItem)sortComboBox.SelectedItem).Tag.ToString() + "";
                        cmd.CommandText = "SELECT   * FROM business natural join businesshours, (SELECT DISTINCT business_id as busID " + builtString + ") as a " +
                           "WHERE state='" + stateList.SelectedItem.ToString() + "' and city='" + cityListBox.SelectedItem.ToString() + "' " +
                           "and postal_code = '" + ZipListBOX.SelectedItem.ToString() + "' and a.busID = business.business_id  " + SQL_string
                           + "; ";

                        

                        // cmd.CommandText = "SELECT  name, address, city, state, reviewrating, review_count, stars  " +
                        //"FROM business NATURAL JOIN review NATURAL JOIN businesscategory WHERE state = '" + stateList.SelectedItem.ToString() +;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Business business = new Business();
                                business.business_id = reader.GetString(0);
                                business.name = reader.GetString(1);
                                business.address = reader.GetString(2);
                                business.city = reader.GetString(3);
                                business.state = reader.GetString(4);
                                business.zipcode = reader.GetString(5);
                                business.latitude = reader.GetDouble(6);
                                business.longitude = reader.GetDouble(7);
                                business.stars = reader.GetDouble(8);
               
                                
                                business.review_Count = reader.GetInt32(9);
                                business.isOpen = reader.GetInt32(10);
                                business.num_checkIns = reader.GetInt32(11);
                                business.review_Rating = reader.GetDouble(12);
                                BGRID.Items.Add(business);

                                // will handle user location 
                                if (user.User_latitude != 0 && user.User_longitude != 0)
                                {
                                    var businessCoOrds = new GeoCoordinate(business.latitude, business.longitude);


                                    var userCoOrds = new GeoCoordinate(user.User_latitude, user.User_longitude);

                                    var meters = userCoOrds.GetDistanceTo(businessCoOrds);

                                    business.distance = meters / 1609.344;
                                }
                            }
                        }
                    }
                    comm.Close();
                    sb.Clear();
                }
                NumOfBusinesses.Text = BGRID.Items.Count.ToString();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (CatListBox.SelectedIndex > -1)
            {
                selectedCategoriesListBox.Items.Add(CatListBox.SelectedItem.ToString());
            }
        }

        private void addReviewButton_Click(object sender, RoutedEventArgs e)
        {


            if (UserIDMatch.SelectedIndex > -1 && BGRID.SelectedIndex > -1 && ratingComboBox.SelectedIndex > -1)
            {
                DateTime today = DateTime.Today;
                var guid = Guid.NewGuid().ToString();
                string review_id = RandomUtil.GetRandomString();

                using (var comm = new NpgsqlConnection(buildConnString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "INSERT INTO review (review_id, user_id, business_id,stars, created_on,review_text, funny, cool, useful) " +
                            "VALUES ('" + review_id + "', '" + UserIDMatch.SelectedItem.ToString() + "', '" + ((Business)BGRID.SelectedItem).business_id + "','"+ Convert.ToDouble(ratingComboBox.SelectedItem.ToString())  +  "', CAST('" + today.Date + "' AS date), '" + reviewText.Text + "', " +" 0, 0, 0)";
                        cmd.ExecuteNonQuery();
                       
                        updateBGRID();
                    }
                    comm.Close();
                }
            }
        }

        private void checkinButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ratingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ToTimeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void FromTimeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void popualteFromTimeBox()
        {
            //will add times in the From box
            FromTimeBox.Items.Add("");

            for (int i = 0; i <= 23; i++)
                FromTimeBox.Items.Add(i.ToString().PadLeft(2, '0') + ":00:00");
        }

        private void popualteToTimeBox()
        {// this will add times to the To box
            ToTimeBox.Items.Add("");

            for (int i = 0; i <= 23; i++)
                ToTimeBox.Items.Add(i.ToString().PadLeft(2, '0') + ":00:00");
            
        }
        private void addRatings()
        {
            //string[] ratings = new string[] { "1", "2", "3", "4", "5" };
            //ratingComboBox.ItemsSource = ratings;
            //ratingComboBox.IsEnabled = false;
            for (int i = 1; i <= 5; i++)
                ratingComboBox.Items.Add(i.ToString());

        }
        private void dayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //using (var comm = new NpgsqlConnection(buildConnString()))
            //{
            //    ToTimeBox.Items.Clear();
            //    FromTimeBox.Items.Clear();
            //    comm.Open();
            //    using (var cmd = new NpgsqlCommand())
            //    {
            //        cmd.Connection = comm;
            //        cmd.CommandText = "SELECT DISTINCT open FROM businesshours WHERE day = '" + dayComboBox.SelectedItem.ToString() + "' ORDER BY open;";
            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                FromTimeBox.Items.Add(reader.GetTimeSpan(0));
            //            }
            //        }
            //        if (FromTimeBox.SelectedIndex > -1)
            //        {
            //            cmd.CommandText = "SELECT DISTINCT close FROM businesshours WHERE day = '" + dayComboBox.SelectedItem.ToString() + "' AND open = '" + FromTimeBox.SelectedItem.ToString() + "' ORDER BY close ASC;";
            //            using (var reader = cmd.ExecuteReader())
            //            {
            //                while (reader.Read())
            //                {
            //                    ToTimeBox.Items.Add(reader.GetTimeSpan(0));
            //                }
            //            }
            //        }
            //        else
            //        {
            //            cmd.CommandText = "SELECT DISTINCT close FROM businesshours WHERE day = '" + dayComboBox.SelectedItem.ToString() + "' ORDER BY close ASC;";
            //            using (var reader = cmd.ExecuteReader())
            //            {
            //                while (reader.Read())
            //                {
            //                    ToTimeBox.Items.Add(reader.GetTimeSpan(0));
            //                }
            //            }
            //        }
            //    }
            //    comm.Close();
            //}
        }

        private void NumOfBusinesses_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void busPerZipCodeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            if (BGRID.SelectedIndex > -1)
            {
                Reviewschart popup = new Reviewschart(((Business)BGRID.SelectedItem).business_id);
                popup.Show();
            }
        }

        private void checkinsButton_Click(object sender, RoutedEventArgs e)
        {
            

          

            if (BGRID.SelectedIndex > -1)
            {
                newCheckinChart checkInsPopUp = new newCheckinChart(((Business)BGRID.SelectedItem).business_id);
                checkInsPopUp.ShowDialog();
            }
        }

        private void cityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BGRID.Items.Clear();
            if (cityListBox.SelectedIndex > -1)
            {
                AddZips();
            }
        }

        private void ZipListBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BGRID.Items.Clear();

            if (ZipListBOX.SelectedIndex > -1)
            {
                AddBusiness();
            }
        }

        private void CatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (CatListBox.SelectedIndex > -1)
            {
                selectedCategoriesListBox.Items.Remove(CatListBox.SelectedItem.ToString());
            }
        }

      

        private void BGRID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BGRID.SelectedIndex > -1)
            {
                selectedBusiness.Text = ((Business)BGRID.SelectedItem).name;
            }
        }

        private void selectedCategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void userTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserIDMatch.Items.Clear();

            FriendsDGrid.Items.Clear();

            //not currenlty working
            //ReviewsByFriendsDataGrid.Items.Clear();
            using (var comm = new NpgsqlConnection(buildConnString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT user_id FROM users WHERE name = '" + userTextBox.Text.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserIDMatch.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }


        }

        private void UserIDMatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string user_id = UserIDMatch.SelectedItem.ToString();
            if (UserIDMatch.SelectedIndex > -1)
            {
                //clear all info tables and otehr text boxes 
                FriendsDGrid.Items.Clear();
                //not working
                //ReviewsByFriendsDataGrid.Items.Clear();

                using (var comm = new NpgsqlConnection(buildConnString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "SELECT name, average_stars, fans, yelping_since, funny, cool, useful FROM users WHERE user_id = '" + user_id + "';";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserNameTBOX.Text = reader.GetString(0);
                                UserStarsTextBox.Text = reader.GetDouble(1).ToString();
                                UserFansTextBox.Text = reader.GetInt32(2).ToString();
                                yelpingSinceTextBox.Text = reader.GetDate(3).ToString();
                                FunnyVotesTextBox.Text = reader.GetInt32(4).ToString();
                                CoolVotesTextBox.Text = reader.GetInt32(5).ToString();
                                UsefulVotesTextBox.Text = reader.GetInt32(6).ToString();
                            }
                        }


                        //run query for users friends
                        //SELECT DISTINCT friend_id, name FROM friends natural join users WHERE  friend_id = '7uEuP9iJEzCshvDKS7_n8w' order by name

                        //SELECT temp.name, temp.average_stars, temp.yelping_since, F.friend_id
//                        FROM users as U, friends as F, (SELECT * FROM users) as temp WHERE U.user_id = '7uEuP9iJEzCshvDKS7_n8w'
//AND U.user_id = F.user_id AND
//F.friend_id = temp.user_id ORDER BY temp.name


                        //cmd.CommandText = "SELECT distinct name, average_stars, yelping_since, friend_id FROM friends Natural JOIN users " +
                        //    "WHERE user_id = '" + UserIDMatch.SelectedItem.ToString() + "' WHERE friend_id = users.user_id; ";
                        cmd.CommandText = "SELECT obj.name, obj.average_stars, obj.yelping_since, Frd.friend_id FROM users as Usr, friends as Frd, (SELECT * FROM users) as obj WHERE Usr.user_id = '" + user_id + "' AND Usr.user_id = Frd.user_id" +
                            " AND Frd.friend_id = obj.user_id ORDER BY obj.name;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FriendsDGrid.Items.Add(new Friend { friend_name = reader.GetString(0), Friend_stars = reader.GetDouble(1), Yelping_since = (reader.GetDate(2)).ToString(), Friend_ID = reader.GetString(3) });
                            }
                        }
                        //need to work on the review by friends grid
                    }
                    comm.Close();
                }
            }
        }
        //"DELETE from friendslist button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Get the current user.
            string user_id = UserIDMatch.SelectedItem.ToString();

            if (FriendsDGrid.SelectedIndex > -1)
            {
                using (var comm = new NpgsqlConnection(buildConnString()))
                {

                    comm.Open();
                    //run the query to delete friend from database
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "DELETE FROM friends WHERE friend_id = '" + ((Friend)FriendsDGrid.SelectedItem).Friend_ID + "' AND user_id = '" + UserIDMatch.SelectedItem.ToString() + "'";
                        cmd.ExecuteNonQuery(); //ExecuteNonQuery command.why because we are not querying a database, we are modifying.


                        FriendsDGrid.Items.Clear();

                        cmd.CommandText = "SELECT distinct name, average_stars, yelping_since, friend_id FROM users, (SELECT DISTINCT friend_id FROM friends " +
                                "WHERE user_id = '" + UserIDMatch.SelectedItem.ToString() + "') as current WHERE current.friend_id = users.user_id; ";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FriendsDGrid.Items.Add(new Friend { friend_name = reader.GetString(0), Friend_stars = reader.GetDouble(1), Yelping_since = (reader.GetDate(2)).ToString(), Friend_ID = reader.GetString(3) });
                            }
                        }
                       
                    }
                    comm.Close();
                }
            }
        }

        private void selectedBusiness_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
