using System;
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

namespace practica
{
    public partial class AddItems : Window
    {
        public string connection = "Server=DESKTOP-VNV8BDD\\MSSQLSERVER01;Database=practika;Trusted_Connection=True";
        public string categor;
        public string proizvod;
        public string updt;
        public bool flag = false;
        public event Action DataAdded;

        public AddItems()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(UserData.AddOrUpdt == "updt")
            {
                nameItems.Text = UserData.nameItems;
                categorItems.Text = UserData.categorItems;
                proizvodItems.Text = UserData.proizvodItems;
                ostatoklItems.Text = UserData.ostatokItems;
                izmerItems.Text = UserData.izmerItems;
                priceItems.Text = UserData.priceItems;
                opisanieItems.Text = UserData.opisanieItems;
                skidkaItems.Text = UserData.skidkaItems;
                
                updt = "updt";
                idItemsText.Visibility = Visibility.Visible;
                idItemsText.Content = "Номер строки: " + UserData.idItems;
            }
            UserData.AddOrUpdt = "";
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ClearText();
            Close();
        }
        private void WhuProizvod_Click(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
                proizvod = selectedItem.Content.ToString();
        }
        private void Categor_Click(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
                categor = selectedItem.Content.ToString();
        }
        private void Acept_Click(object sender, RoutedEventArgs e)
        {
            notMinus();
            if (updt == "updt" && flag == true)
            {
                try
                {
                    string query = "UPDATE Products2 SET name = @nameItems, categor = @categor, opisanie = @opisanieItems, " +
                        "proizvoditel = @proizvod, zenaEd = @priceItems, ostatok = @ostatoklItems, " +
                        "edIzmer = @izmerItems, skidka = @skidkaItems WHERE id_products = @id";

                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@id", UserData.idItems);
                        cmd.Parameters.AddWithValue("@nameItems", nameItems.Text);
                        cmd.Parameters.AddWithValue("@categor", categor);
                        cmd.Parameters.AddWithValue("@opisanieItems", opisanieItems.Text);
                        cmd.Parameters.AddWithValue("@proizvod", proizvod);
                        cmd.Parameters.AddWithValue("@priceItems", priceItems.Text);
                        cmd.Parameters.AddWithValue("@ostatoklItems", ostatoklItems.Text);
                        cmd.Parameters.AddWithValue("@izmerItems", izmerItems.Text);
                        cmd.Parameters.AddWithValue("@skidkaItems", skidkaItems.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Товар обновлен", "Успешно");
                        DataAdded?.Invoke();

                        idItemsText.Visibility = Visibility.Hidden;
                        ClearText();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(flag == true)
            {
                try
                {
                    string query = "INSERT INTO  Products2(name, categor, opisanie, proizvoditel, zenaEd, ostatok, edIzmer, skidka, priceWithSkidka) " +
                        "VALUES(@nameItems, @categor, @opisanieItems, @proizvod, @priceItems, @ostatoklItems, @izmerItems, @skidkaItems, @priceWithSkidka)";
                    string zenaWithSkidka = "";

                    if(skidkaItems.Text != "" && skidkaItems.Text != "0")
                    {
                        zenaWithSkidka = (double.Parse(priceItems.Text) - (double.Parse(priceItems.Text) * (double.Parse(skidkaItems.Text) / 100))).ToString();
                    }

                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@nameItems", nameItems.Text);
                        cmd.Parameters.AddWithValue("@categor", categor);
                        cmd.Parameters.AddWithValue("@opisanieItems", opisanieItems.Text);
                        cmd.Parameters.AddWithValue("@proizvod", proizvod);
                        cmd.Parameters.AddWithValue("@priceItems", priceItems.Text);
                        cmd.Parameters.AddWithValue("@ostatoklItems", ostatoklItems.Text);
                        cmd.Parameters.AddWithValue("@izmerItems", izmerItems.Text);
                        cmd.Parameters.AddWithValue("@skidkaItems", skidkaItems.Text);
                        cmd.Parameters.AddWithValue("@priceWithSkidka", zenaWithSkidka);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Товар добавлен", "Успешно");
                        DataAdded?.Invoke();

                        ClearText();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            flag = false;
        }
        private void Name_Clear(object sender, RoutedEventArgs e)
        {
            nameItems.Clear();
        }
        private void Ostatok_Clear(object sender, RoutedEventArgs e)
        {
            ostatoklItems.Clear();
        }
        private void Izmer_Clear(object sender, RoutedEventArgs e)
        {
            izmerItems.Clear();
        }
        private void Price_Clear(object sender, RoutedEventArgs e)
        {
            priceItems.Clear();
        }
        private void Opisanie_Clear(object sender, RoutedEventArgs e)
        {
            opisanieItems.Clear();
        }
        private void Skidka_Clear(object sender, RoutedEventArgs e)
        {
            skidkaItems.Clear();
        }
        public void ClearText()
        {
            nameItems.Clear();
            ostatoklItems.Clear();
            izmerItems.Clear();
            priceItems.Clear();
            opisanieItems.Clear();
            skidkaItems.Clear();
        }
        public void notMinus()
        {
            if (ostatoklItems.Text[0] ==  '-')
            {
                MessageBox.Show("Остаток не может быть отрицательным!!!");
            }
            else if(priceItems.Text[0] == '-')
            {
                MessageBox.Show("Цена не может быть отрицательной!!!");
            }
            else if (skidkaItems.Text[0] == '-')
            {
                MessageBox.Show("Скидка не может быть отрицательной!!!");
            }
            else
            {
                flag = true;
            }
        }
    }
}
