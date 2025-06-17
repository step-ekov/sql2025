using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Xml.Linq;

namespace practica
{
    public partial class Main : Window
    {
        public string connection = "Server=DESKTOP-VNV8BDD\\MSSQLSERVER01;Database=practika;Trusted_Connection=True";
        public int skidka; 
        public string proizvod;

        public Main()
        {
            InitializeComponent();

            if(UserData.HowVhod == 2)
            {
                try
                {
                    Whu();
                    VivodDan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                FIOText.Visibility = Visibility.Hidden;
                AddBtn.Visibility = Visibility.Hidden;
                RoleText.Content = "Гость";
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var avtoriz = new MainWindow();
            avtoriz.Show();
            Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void Whu()
        {
            string query = "SELECT FIO, role FROM Users WHERE login = @login";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@login", UserData.login);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        FIOText.Content = reader["FIO"].ToString();
                        RoleText.Content = reader["role"].ToString();
                    }
                }
            }
            if (RoleText.Content.ToString() == "client")
            {
                AddBtn.Visibility = Visibility.Hidden;
            }
        }
        public void VivodDan()
        {
            string query = "SELECT id_products as 'Номер', name as 'Название', " +
                "categor as 'Категория', opisanie as 'Описание', " +
                "proizvoditel as 'Производитель', zenaEd as 'Цена за ед', priceWithSkidka as 'Ценапоскидке'," +
                "ostatok as 'Остаток', edIzmer as 'Ед Измерения', skidka as 'Скидка', " +
                "kartinka as 'Фото'  from Products2";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                grid.ItemsSource = dataTable.DefaultView;
            }
        }
        private void grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                DataRowView rowView = e.Row.Item as DataRowView;
                if (rowView != null)
                {
                    if (double.Parse(rowView["Скидка"].ToString()) >= 15 && int.Parse(rowView["Остаток"].ToString()) != 0)
                    {
                        e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7fff00"));
                    }
                    else if(double.Parse(rowView["Скидка"].ToString()) >= 10 && int.Parse(rowView["Остаток"].ToString()) != 0)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.Yellow);
                    }
                    else if (double.Parse(rowView["Скидка"].ToString()) >= 5 && int.Parse(rowView["Остаток"].ToString()) != 0)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.Thistle);
                    }
                    else if (double.Parse(rowView["Остаток"].ToString()) == 0)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.Aqua);
                    }
                    else
                    {
                        e.Row.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void WhuProizvod_Click(object sender, SelectionChangedEventArgs e)
        {
            string query;
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
                proizvod = selectedItem.Content.ToString();
            try
            {
                if(proizvod == "Все производители")
                {
                    query = "SELECT id_products as 'Номер', name as 'Название', " +
                    "categor as 'Категория', opisanie as 'Описание', " +
                    "proizvoditel as 'Производитель', zenaEd as 'Цена за ед', priceWithSkidka as 'Ценапоскидке'," +
                    "ostatok as 'Остаток', edIzmer as 'Ед Измерения', skidka as 'Скидка', " +
                    "kartinka as 'Фото'  FROM Products2";
                }
                else
                {
                    query = "SELECT id_products as 'Номер', name as 'Название', " +
                    "categor as 'Категория', opisanie as 'Описание', " +
                    "proizvoditel as 'Производитель', zenaEd as 'Цена за ед', priceWithSkidka as 'Ценапоскидке'," +
                    "ostatok as 'Остаток', edIzmer as 'Ед Измерения', skidka as 'Скидка', " +
                    "kartinka as 'Фото'  FROM Products2 WHERE proizvoditel = @proizvoditel";
                }
                using(SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@proizvoditel", proizvod);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    grid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserData.AddOrUpdt = "add";
            var newsItems = new AddItems();
            newsItems.DataAdded += () => VivodDan();
            newsItems.ShowDialog();
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search.Clear();
        }
        private void isVozrast_Click(object sender, RoutedEventArgs e)
        {
            if (vozrastZena.IsChecked == true)
            {
                string query = "SELECT id_products as 'Номер', name as 'Название', " +
                "categor as 'Категория', opisanie as 'Описание', " +
                "proizvoditel as 'Производитель', zenaEd as 'Цена за ед', priceWithSkidka as 'Ценапоскидке'," +
                "ostatok as 'Остаток', edIzmer as 'Ед Измерения', skidka as 'Скидка', " +
                "kartinka as 'Фото'  FROM Products2 ORDER BY CAST(zenaEd as int) ASC";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    grid.ItemsSource = dataTable.DefaultView;
                }
            }
        }
        private void isUbiv_Click(object sender, RoutedEventArgs e)
        {
            if (ubivZena.IsChecked == true)
            {
                string query = "SELECT id_products as 'Номер', name as 'Название', " +
                "categor as 'Категория', opisanie as 'Описание', " +
                "proizvoditel as 'Производитель', zenaEd as 'Цена за ед', priceWithSkidka as 'Ценапоскидке'," +
                "ostatok as 'Остаток', edIzmer as 'Ед Измерения', skidka as 'Скидка', " +
                "kartinka as 'Фото'  FROM Products2 ORDER BY CAST(zenaEd as int) DESC";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    grid.ItemsSource = dataTable.DefaultView;
                }
            }
        }
        private void WhatDo_Click(object sender, MouseButtonEventArgs e)
        {
            if (RoleText.Content.ToString() != "client" && RoleText.Content.ToString() != "Гость")
            {
                var doing = new WhatDo();
                doing.ShowDialog();

                if (UserData.isDell == "isdell")
                {
                    Dellete();
                }
                else if (UserData.isUpdate == "isupdt")
                {
                    SaveItems();
                    var newsItems = new AddItems();
                    newsItems.DataAdded += () => VivodDan();
                    newsItems.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Что то не так");
                    return;
                }
                UserData.isDell = "";
                UserData.isUpdate = "";
            }
        }
        public void Dellete()
        {
            try
            {
                string query = "DELETE FROM Products2 WHERE id_products = @id";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    var selecteditem = grid.SelectedItem as DataRowView;
                    int id = Convert.ToInt32(selecteditem["Номер"]);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                VivodDan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        public void SaveItems()
        {
            try
            {
                var n = (DataRowView)grid.SelectedItem;
                UserData.idItems = n["Номер"].ToString();
                UserData.nameItems = n["Название"].ToString();
                UserData.categorItems = n["Категория"].ToString();
                UserData.opisanieItems = n["Описание"].ToString();
                UserData.proizvodItems = n["Производитель"].ToString();
                UserData.priceItems = n["Цена за ед"].ToString();
                UserData.ostatokItems = n["Остаток"].ToString();
                UserData.izmerItems = n["Ед Измерения"].ToString();
                UserData.skidkaItems = n["Скидка"].ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
        private void Sbros_Click(object sender, MouseButtonEventArgs e)
        {
            VivodDan();
            vozrastZena.IsChecked = false;
            ubivZena.IsChecked = false;
        }
    }
}
