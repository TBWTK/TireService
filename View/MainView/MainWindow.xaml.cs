using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TireService.Model;

namespace TireService.View.MainView
{
    public partial class MainWindow : Window
    {
        int idUser;
        int tick = 0;
        Point now = new Point(0, 0);
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow(int ID)
        {
            idUser = ID;
            InitializeComponent();

            using(var context = new TireServiceEntities())
            {
                var checkRole = context.Users.SingleOrDefault(u => u.Id == idUser);
                if(checkRole.Role == 2)
                {
                    AdminPage.Visibility = Visibility.Hidden;
                }
                if (checkRole.Role == 3)
                {
                    StatPage.Visibility = Visibility.Hidden;
                }
            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();

            PageChange.Content = new Account.PersonalAccountUserControl(idUser);
        }

        // Смена пейджей 
        private void AccountPage_Click(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new Account.PersonalAccountUserControl(idUser);
        }
        private void CatalogPage_Click(object sender, RoutedEventArgs e)
        {
        }
        private void AdminPage_Click(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new Admin.PatchingUserControl();
        }
        private void StatPage_Click(object sender, RoutedEventArgs e)
        {

        }

        // Бездекйствие пользователя
        private void Timer_Tick(object sender, EventArgs e)
        {
            ++tick;
            if (tick == 20)
            {
                timer.Stop();
                AuthenticationView.AuthenticationWindow authentication = new AuthenticationView.AuthenticationWindow();
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
    }
}
