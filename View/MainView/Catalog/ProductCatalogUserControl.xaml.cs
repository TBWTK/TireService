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
using TireService.Model;

namespace TireService.View.MainView.Catalog
{
    /// <summary>
    /// Логика взаимодействия для ProductCatalogUserControl.xaml
    /// </summary>
    public partial class ProductCatalogUserControl : UserControl
    {
        List<int> IdProducts;
        int IndexItemCatalog = 0;
        decimal additionsSum;
        int idUser;

        List<Products> products;
        List<Products> HandlerProducts;

        public ProductCatalogUserControl(int ID)
        {
            idUser = ID;

            IdProducts = new List<int>();
            products = new List<Products>();
            HandlerProducts = new List<Products>();

            InitializeComponent();
            ClearSearchPanels();
        }


        // Функция по выгрузке в лист айдишников машин 
        private void LoadIdFromIndex()
        {
            IdProducts.Clear();
            IndexItemCatalog = 0;
            foreach (var item in products)
            {
                IdProducts.Add(item.Id);
            }

            if (OneProductDisplay.IsChecked == true)
            {
                var filteredOneProduct = products.Where(c => c.Id == IdProducts[IndexItemCatalog]);

                ProductCatalog.ItemsSource = filteredOneProduct;
            }
            else if (FourProductDisplay.IsChecked == true)
            {
                if (IdProducts.Count() < 4)
                {
                    var filteredFourProduct = products.Where(u => u.Id >= IdProducts[IndexItemCatalog] && u.Id <= IdProducts[IdProducts.Count() - 1]);
                    ProductCatalog.ItemsSource = filteredFourProduct;
                }
                else
                {
                    int numberForCorrectDisplayDuringTransitions = 3;

                    var filteredFourProduct = products.Where(u => u.Id >= IdProducts[IndexItemCatalog] && u.Id <= IdProducts[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                    ProductCatalog.ItemsSource = filteredFourProduct;
                }
            }
            else
            {
                ProductCatalog.ItemsSource = products;
            }
        }


        // Переключение отображения товара спо 1, по 4, списком
        private void OneProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateBig"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Visible;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();
        }
        private void FourProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateSmall"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Visible;
            ClearSearchPanels();
        }
        private void ListProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateSmall"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();
        }


        // Переключение корзины по 1 товару
        private void ButtonPreviousOneProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog < 1)
                IndexItemCatalog = 0;
            else
                IndexItemCatalog--;

            //var carsItems = ProductCatalog.ItemsSource.Cast<Cars>();
            var filtered = products.Where(c => c.Id == IdProducts[IndexItemCatalog]);
            ProductCatalog.ItemsSource = filtered;
        }
        private void ButtonNextOneProduct_Click(object sender, RoutedEventArgs e)
        {
            IndexItemCatalog++;

            if (IndexItemCatalog < IdProducts.Count())
            {
                var filtered = products.Where(c => c.Id == IdProducts[IndexItemCatalog]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
                IndexItemCatalog--;
        }


        // Переключение корзины по 4 товара
        private void ButtonPreviousFourProduct_Click(object sender, RoutedEventArgs e)
        {
            int numberForCorrectDisplayDuringTransitions = 5;
            int numberToDisplayWhenEmptyClicks = 3;
            int numberForAccessingProductsByIndex = 3;


            if (IndexItemCatalog - numberForAccessingProductsByIndex >= 0)
            {
                IndexItemCatalog -= numberForAccessingProductsByIndex;
            }

            if (IndexItemCatalog - numberForAccessingProductsByIndex >= 0)
            {
                var filtered = products.Where(u => u.Id > IdProducts[IndexItemCatalog] && u.Id < IdProducts[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
            {
                if (IndexItemCatalog + numberToDisplayWhenEmptyClicks < IdProducts.Count())
                {
                    var filtered = products.Where(u => u.Id >= IdProducts[IndexItemCatalog] && u.Id <= IdProducts[IndexItemCatalog + numberToDisplayWhenEmptyClicks]);
                    ProductCatalog.ItemsSource = filtered;
                }
                else
                {
                    var filtered = products.Where(u => u.Id >= IdProducts[IndexItemCatalog] && u.Id <= IdProducts[IdProducts.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }
            }
        }
        private void ButtonNextFourProduct_Click(object sender, RoutedEventArgs e)
        {
            int numberForCorrectDisplayDuringTransitions = 5;
            int numberToDisplayFinalProducts = 1;
            int numberForAccessingProductsByIndex = 3;

            if (IndexItemCatalog + numberForAccessingProductsByIndex < IdProducts.Count() - 1)
            {
                IndexItemCatalog += numberForAccessingProductsByIndex;
                //if (IndexItemCatalog + numberForAccessingProductsByIndex > IdProducts.Count() - 1)
                //{
                //    IndexItemCatalog -= numberForAccessingProductsByIndex;
                //}
            }

            if (IndexItemCatalog + numberForCorrectDisplayDuringTransitions < IdProducts.Count())
            {
                var filtered = products.Where(u => u.Id > IdProducts[IndexItemCatalog] && u.Id < IdProducts[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
            {
                if (IndexItemCatalog + numberToDisplayFinalProducts < IdProducts.Count() - 1)
                {
                    var filtered = products.Where(u => u.Id > IdProducts[IndexItemCatalog + numberToDisplayFinalProducts] && u.Id <= IdProducts[IdProducts.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }
                else
                {
                    var filtered = products.Where(u => u.Id >= IdProducts[IndexItemCatalog] && u.Id <= IdProducts[IdProducts.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }
            }
        }



        // Фильтры поиска
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = CategoryComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                // Выгрузка в объект выбранного экземпляра
                Categories selectedItem = (Categories)CategoryComboBox.SelectedItem;

                // Обновление ComboBox для вбора моделей на основе выбранного бренда
                using (var context = new TireServiceEntities())
                {
                    TypeComboBox.ItemsSource = context.TypeProducts.Where(c => c.Category == selectedItem.Id).ToList();
                }

                products = products.Where(p => p.TypeProducts.Category == selectedItem.Id).ToList();

                LoadIdFromIndex();
            }
        }
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = TypeComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                TypeProducts selectedItem = (TypeProducts)TypeComboBox.SelectedItem;
                products = products.Where(c => c.TypeProduct == selectedItem.Id).ToList();
                LoadIdFromIndex();
            }
        }
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var filter = products.Where(a => a.Price >= Convert.ToInt32(PriceStart.Text) && a.Price <= Convert.ToInt32(PriceEnd.Text)).ToList();
                ProductCatalog.ItemsSource = filter;
                LoadIdFromIndex();
            }
            catch
            {
                MessageBox.Show("Введите число");
            }
        }

        // Переход к корзине
        private void MoveToBasket_Click(object sender, RoutedEventArgs e)
        {
            BasketGird.Visibility = Visibility.Visible;
            CatalogGrid.Visibility = Visibility.Hidden;
            BasketCatalog.ItemsSource = HandlerProducts;
        }


        // Выбор товара
        private void ButtonChoice_Click(object sender, RoutedEventArgs e)
        {
            Products curItem = (Products)((ListBoxItem)ProductCatalog.ContainerFromElement((Button)sender)).Content;
            decimal temp = Convert.ToDecimal(QuentytiProduct.Text);
            additionsSum = (decimal)(temp + curItem.Price);
            QuentytiProduct.Text = $"{additionsSum:0.00}";
            SumOrder.Text = $"{additionsSum:0.00}";

            HandlerProducts.Add(curItem);
        }


        // Отчистка поиска и обновление элементов
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchPanels();
        }
        private void ClearSearchPanels()
        {
            CategoryComboBox.SelectedIndex = -1;
            TypeComboBox.SelectedIndex = -1;
            PriceStart.Text = "";
            PriceEnd.Text = "";
            using (var context = new TireServiceEntities())
            {
                CategoryComboBox.ItemsSource = context.Categories.ToList();
                TypeComboBox.ItemsSource = context.TypeProducts.ToList();
                products = context.Products.Where(p => p.QuantityProduct > 0).ToList();
            }

            LoadIdFromIndex();
        }


        // Удаление товара
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Products curItem = (Products)((ListBoxItem)BasketCatalog.ContainerFromElement((Button)sender)).Content;

            var newProducts = HandlerProducts.Where(u => u.Id != curItem.Id);

            decimal temp = Convert.ToDecimal(QuentytiProduct.Text);
            additionsSum = (decimal)(temp - curItem.Price);
            QuentytiProduct.Text = $"{additionsSum:0.00}";
            SumOrder.Text = $"{additionsSum:0.00}";

            HandlerProducts.Remove(curItem);
            BasketCatalog.ItemsSource = newProducts;
        }


        private void BackToCatalog_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BasketGird.Visibility = Visibility.Hidden;
            CatalogGrid.Visibility = Visibility.Visible;
        }

        private void MakeOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (HandlerProducts.Count != 0)
            {
                try
                {
                    using (var context = new TireServiceEntities())
                    {
                        var datetime = DateTime.Now;
                        var onlydate = datetime.Date;
                        Orders newOrder = new Orders()
                        {
                            User = idUser,
                            Amount = additionsSum,
                            Date = onlydate
                        };
                        context.Orders.Add(newOrder);
                        context.SaveChanges();

                        var getOrder = context.Orders.SingleOrDefault(o => o.User == idUser && o.Amount == additionsSum && o.Date == onlydate);

                        foreach (var item in HandlerProducts)
                        {
                            Baskets productToBasket = new Baskets
                            {
                                Product = item.Id,
                                Order = getOrder.Id

                            };
                            context.Baskets.Add(productToBasket);
                        }
                        context.SaveChanges();
                    }
                    MessageBox.Show("Заказ оформлен дон");

                    additionsSum = 0;
                    QuentytiProduct.Text = $"{additionsSum:0.00}";
                    SumOrder.Text = $"{additionsSum:0.00}";

                    HandlerProducts.Clear();
                    BasketCatalog.ItemsSource = HandlerProducts.ToList();
                }
                catch
                {
                    MessageBox.Show("Произшла непредвиденная ошибка, помолитесь");
                }

            }
            else
                MessageBox.Show("Товаров нет в корзине");
        }


    }
}
