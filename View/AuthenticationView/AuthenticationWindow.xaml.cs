using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TireService.Model;

namespace TireService.View.AuthenticationView
{
    public partial class AuthenticationWindow : Window
    {
        private string captchaCode;
        private byte iteratorLogin = 0;
        private byte iteratorCaptcha;

        int tick = 0;
        Point now = new Point(0, 0);
        DispatcherTimer timer = new DispatcherTimer();
        public AuthenticationWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        // Проверка пароля
        private void CheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = PassPassBox.Password.Trim();
            PassPassBox.Visibility = Visibility.Hidden;
            PassTextBlock.Visibility = Visibility.Visible;
        }
        private void UnCheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = "";
            PassTextBlock.Visibility = Visibility.Hidden;
            PassPassBox.Visibility = Visibility.Visible;
        }

        // Вход в приложение
        private void EnterSystem(object sender, RoutedEventArgs e)
        {
            if (iteratorLogin < 2)
            {
                using (var context = new TireServiceEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Login == LoginTextBox.Text && x.Password == PassPassBox.Password.Trim());
                    if (us != null)
                    {
                        if (us.Statuses.NameStatus == "Активен")
                        {
                            MainView.MainWindow main = new MainView.MainWindow(us.Id);
                            main.Show();
                            timer.Stop();
                            this.Close();
                        }
                        else
                        {
                            iteratorLogin++;
                            MessageBox.Show("Пользователь неактивен обратитесь к администратору");
                        }
                    }
                    else
                    {
                        iteratorLogin++;
                        MessageBox.Show("Неверные данные или пользователя не существует!");
                    }
                }
            }
            else
            {
                BorderLogin.Visibility = Visibility.Hidden;
                iteratorLogin = 0;
                BorderCaptcha.Visibility = Visibility.Visible;
                LoadContentCapha();
            }
        }

        // Функции капчи
        private void UpdateCaptha_Click(object sender, RoutedEventArgs e)
        {
            LoadContentCapha();
        }
        private void CheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (CapthaTextBox.Text == captchaCode)
            {
                BorderLogin.Visibility = Visibility.Visible;
                BorderCaptcha.Visibility = Visibility.Hidden;
                iteratorCaptcha = 0;
            }
            else
            {
                iteratorCaptcha++;
                MessageBox.Show("Введены неверные данные!");
            }
            if (iteratorCaptcha > 3)
            {
                MessageBox.Show("Попытки для прохождения каптчи закончились, перезапустите приложение");
                this.Close();
            }
        }
        public void LoadContentCapha()
        {
            Random random = new Random();
            int a = random.Next(1, 100);
            int b = random.Next(1, 100);
            int c = random.Next(1, 100);
            int x = a + b - c;
            CapthaTextBlock.Text = $"{Convert.ToString(a)} + {Convert.ToString(b)} - {Convert.ToString(c)}";
            captchaCode = Convert.ToString(x);
        }

        // Бездействие пользователя
        private void Timer_Tick(object sender, EventArgs e)
        {
            ++tick;
            if (tick == 20)
            {
                timer.Stop();
                this.Close();
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

        // Переход на окно регистрации
        private void ChangeWindowToRegistration_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow();
            timer.Stop();
            this.Close();
            registration.Show();
        }
    }
}
