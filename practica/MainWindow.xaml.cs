using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace practica
{
    public partial class MainWindow : Window
    {
        public int count = 0;
        public string log;
        public string pass; 
        public bool flag = false;
        public int countview = 0;
        private string captcha;
        private DispatcherTimer timer = null;

        public MainWindow()
        {
            InitializeComponent();
            GenerateCaptcha();
        }
        public void Timer()
        {
            MessageBox.Show("Попытки исчерпаны, попробуйте через 10сек", "Уведомление");
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }
        private void timerTick(object sender, EventArgs e)
        {
            timer.Stop();
            MessageBox.Show("Вы снова можете попробовать войти", "Уведомление");
            VhodBtn.IsEnabled = true;
            count = 0;

            login.Clear(); passwordBox.Clear();
            CptText.Visibility = Visibility.Hidden;
            Cpt.Visibility = Visibility.Hidden;
            Refresh.Visibility = Visibility.Hidden;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            UserData.HowVhod = 1;
            MessageBox.Show("В данном режиме возможен только просмотр товара!", "Уведомление");
            var main = new Main();
            main.Show();
            Close();
        }
        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            count++;

            switch (count)
            {
                case 1:
                    Proverka();
                    break;
                case 2:
                    Proverka();
                    break;
                case 3:
                    if (CptText.Text != Cpt.Text)
                    {
                        Timer();
                        VhodBtn.IsEnabled = false;
                    }
                    else
                    {
                        Proverka();
                    }
                    break;
            }
        }
        public void Proverka()
        {
            try
            {
                string connection = "Server=DESKTOP-VNV8BDD\\MSSQLSERVER01; Database=practika; Trusted_Connection=True;";
                string query = "SELECT login,password FROM Users WHERE login = @login AND password = @password";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@login", login.Text);
                    cmd.Parameters.AddWithValue("@password", passwordBox.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            log = reader["login"].ToString();
                            pass = reader["password"].ToString();
                            UserData.login = log;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            if (login.Text == log && passwordBox.Password == pass)
            {   
                UserData.HowVhod = 2;
                var mainWindow = new Main();
                mainWindow.Show();
                Close();
            }
            else
            {
                login.Text = "";
                passwordBox.Password = "";

                if(count == 3)
                {
                    Timer();
                    VhodBtn.IsEnabled = false;
                }

                if(count < 3)
                {
                    MessageBox.Show("Неверно!!!", "Уведомление");
                }
                
                if (count == 2)
                {
                    CptText.Visibility = Visibility.Visible;
                    Cpt.Visibility = Visibility.Visible;
                    Refresh.Visibility = Visibility.Visible;
                }
            }
        }
        private void Click_lg(object sender, RoutedEventArgs e)
        {
            login.Clear();
            PassText.Visibility = Visibility.Hidden;
        }
        private void Cpt_Click(object sender, RoutedEventArgs e)
        {
            CptText.Clear();
        }
        private void ViewPass_Click(object sender, RoutedEventArgs e)
        {
            countview++;
            string textPss = passwordBox.Password;
            if (countview == 1)
            {
                passwordBox.Visibility = Visibility.Hidden;
                passwordTxtbox.Text = textPss;
                passwordTxtbox.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBox.Visibility = Visibility.Visible;
                passwordTxtbox.Text = textPss;
                passwordTxtbox.Visibility = Visibility.Hidden;
                countview = 0;
            }
        }
        private void GenerateCaptcha()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rand = new Random();
            captcha = new string(new char[3]).Select(_ => chars[rand.Next(chars.Length)]).Aggregate("", (a, b) => a + b);
            Cpt.Text = captcha;
        }
        private void UpdateCpt_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
    }
}
