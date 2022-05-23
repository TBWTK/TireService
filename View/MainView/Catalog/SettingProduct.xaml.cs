using System;
using System.Collections.Generic;
using System.IO;
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
using TireService.Model;

namespace TireService.View.MainView.Catalog
{
    public partial class SettingProduct : UserControl
    {
        Products products;
        public SettingProduct()
        {
            products = new Products();
            InitializeComponent();
            using(var context = new TireServiceEntities())
            {
                CategoryComboBox.ItemsSource = context.Categories.ToList();
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = CategoryComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Categories selectedItem = (Categories)CategoryComboBox.SelectedItem;
                using (var context = new TireServiceEntities())
                {
                    TypeComboBox.ItemsSource = context.TypeProducts.Where(t => t.Category == selectedItem.Id).ToList();
                }
                CategoryProduct.Visibility = Visibility.Hidden;
                TypeProduct.Visibility = Visibility.Visible;
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = TypeComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                TypeProducts selectedItem = (TypeProducts)TypeComboBox.SelectedItem;
                products.TypeProduct = selectedItem.Id;
                TypeProduct.Visibility = Visibility.Hidden;
                SettingsProduct.Visibility = Visibility.Visible;
            }
        }

        // Загрузка фото
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        private void DowlandPhoto(object sender, RoutedEventArgs e)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            Microsoft.Win32.OpenFileDialog ofdPicture = new Microsoft.Win32.OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.JPG;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                myBitmapImage.UriSource = new Uri(ofdPicture.FileName);
                myBitmapImage.EndInit();
                Photo.Source = myBitmapImage;
                products.Photo = getJPGFromImageControl(Photo.Source as BitmapImage);
            }
        }

        private void CreateProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(NameProductTextBox.Text != "" && WeightProductTextBox.Text != "" && QuantityProductTextBox.Text != "" && DescriptionProductTextBox.Text != "" && PriceProductTextBox.Text != "" && Photo.Source != null)
            {
                try
                {
                    products.NameProduct = NameProductTextBox.Text;
                    products.WeightProduct = Convert.ToDouble(WeightProductTextBox.Text);
                    products.QuantityProduct = Convert.ToInt32(QuantityProductTextBox.Text);
                    products.DescriptionProduct = DescriptionProductTextBox.Text;
                    products.Price = Convert.ToDecimal(PriceProductTextBox.Text);
                    using (var context = new TireServiceEntities())
                    {
                        context.Products.Add(products);
                        context.SaveChanges();
                    }
                    MessageBox.Show("Продукт создан и добавлен в базу данных");
                    NameProductTextBox.Text = "";
                    WeightProductTextBox.Text = "";
                    QuantityProductTextBox.Text = "";
                    DescriptionProductTextBox.Text = "";
                    PriceProductTextBox.Text = "";
                    Photo.Source = null;
                    CategoryComboBox.SelectedIndex = -1;
                    TypeComboBox.SelectedIndex = -1;

                    SettingsProduct.Visibility = Visibility.Hidden;
                    CategoryProduct.Visibility = Visibility.Visible;

                }
                catch
                {
                    MessageBox.Show("При вводе не целых чисел, вводите их через запятую!");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!!!!");
            }
        }
    }
}
