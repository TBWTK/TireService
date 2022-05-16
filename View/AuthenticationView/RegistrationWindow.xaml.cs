using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TireService.Model;

namespace TireService.View.AuthenticationView
{
    public partial class RegistrationWindow : Window
    {
        int tick = 0;
        Point now = new Point(0, 0);
        DispatcherTimer timer = new DispatcherTimer();
        public RegistrationWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        // Функции проверки пароля
        private void CheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = PassPassBoxOne.Password.Trim();
            PassPassBoxOne.Visibility = Visibility.Hidden;
            PassTextBlockOne.Visibility = Visibility.Visible;
        }
        private void UnCheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = "";
            PassTextBlockOne.Visibility = Visibility.Hidden;
            PassPassBoxOne.Visibility = Visibility.Visible;
        }
        private void CheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = PassPassBoxTwo.Password.Trim();
            PassPassBoxTwo.Visibility = Visibility.Hidden;
            PassTextBlockTwo.Visibility = Visibility.Visible;
        }
        private void UnCheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = "";
            PassTextBlockTwo.Visibility = Visibility.Hidden;
            PassPassBoxTwo.Visibility = Visibility.Visible;
        }

        // Функция регистрации аккаунта
        private void RegistrationSystem(object sender, RoutedEventArgs e)
        {
            if (PassPassBoxOne.Password != "" && PassPassBoxTwo.Password != "" && (PassPassBoxOne.Password == PassPassBoxTwo.Password) && LoginTextBox.Text != "")
            {
                try
                {
                    using (var context = new TireServiceEntities())
                    {
                        var status = context.Statuses.SingleOrDefault(x => x.NameStatus == "Активен");
                        var std = new Users()
                        {
                            Login = LoginTextBox.Text,
                            Password = PassPassBoxOne.Password.Trim(),
                            Status = status.Id
                        };
                        context.Users.Add(std);
                        context.SaveChanges();

                        var us = context.Users.SingleOrDefault(x => x.Login == LoginTextBox.Text);
                        if (us != null)
                        {
                            MainView.MainWindow mainWindow = new MainView.MainWindow(us.Id);
                            timer.Stop();
                            this.Close();
                            mainWindow.Show();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Логин существууте, введите другой логин!");
                }
            }
            else
            {
                MessageBox.Show("Для прохождения успешной регистрации введите правильные данные!");
            }
        }

        // Бездействие пользователя
        private void Timer_Tick(object sender, EventArgs e)
        {
            ++tick;
            if (tick == 20)
            {
                AuthenticationWindow authentication = new AuthenticationWindow();
                timer.Stop();
                this.Close();
                authentication.Show();
            }
        }
        private void Window_KeyDownAndUp(object sender, KeyEventArgs e)
        {
            tick = 0;
        }
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tick = 0;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point pp = e.GetPosition(this);
            if (pp != now)
            {
                tick = 0;
            }
            now = pp;
        }

        // Переход на окно входа
        private void ChangeWindowToAuthentication_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AuthenticationWindow authentication = new AuthenticationWindow();
            timer.Stop();
            this.Close();
            authentication.Show();
        }
    }
}
