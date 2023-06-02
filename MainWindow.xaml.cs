using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Lab3_301249589_Fadeeva__Question1
{

    public partial class MainWindow : INotifyPropertyChanged
    {
        private ObservableCollection<CollectionElement> eList;
        private ObservableCollection<CollectionElement> bList;
        private ObservableCollection<CollectionElement> tList;
        private ObservableCollection<CollectionElement> jList;

        private ObservableCollection<Item> cart;

        Dictionary<string, System.Windows.Controls.ComboBox> collectionDictionary = new Dictionary<string, System.Windows.Controls.ComboBox>();

        List<string> items = new List<string>();

        private double subTotal;
        private double tax;
        private double total;

        public double Subtotal { 
            get { return subTotal; } 
            set { if (subTotal != value) { subTotal = Math.Round(value, 2); OnPropertyChanged(); } } 
        }
        public double Tax { 
            get { return tax; } 
            set { if (tax != value) { tax = Math.Round(value, 2); ; OnPropertyChanged(); } } 
        }
        public double Total { 
            get { return total; } 
            set { if (total != value) { total = Math.Round(value, 2); ; OnPropertyChanged(); } } 
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            cart = new ObservableCollection<Item>();

            eList = new ObservableCollection<CollectionElement>()
            {
                new CollectionElement("Wireless Headphones, $249.99", 249.99),
                new CollectionElement("iPhone X Pro, $1799.99", 1799.99),
                new CollectionElement("Crystal Smart TV, $1299.99", 1299.99),
                new CollectionElement("Electric Kettle, $59.99", 59.99)
            };

            eNames.SelectedIndex = 0;
            eNames.ItemsSource = eList;

            bList = new ObservableCollection<CollectionElement>()
            {
                new CollectionElement("Harry Potter and Philosopher's Stone, $19.99", 19.99),
                new CollectionElement("Alice in the Wonderland, $45.99", 45.99),
                new CollectionElement("The Old man and the Sea, $18.99", 18.99),
                new CollectionElement("The Little Prince, $16.99", 16.99)
            };

            bNames.ItemsSource = bList;
            bNames.SelectedIndex = 0;

            tList = new ObservableCollection<CollectionElement>()
            {
                new CollectionElement("Lego, $99.99", 99.99),
                new CollectionElement("Plush Toy, $9.99", 9.99),
                new CollectionElement("Puzzles, $29.99", 29.99),
                new CollectionElement("Robot Toy, $49.99", 49.99)
            };

            tNames.ItemsSource = tList;
            tNames.SelectedIndex = 0;


            jList = new ObservableCollection<CollectionElement>()
            {
                new CollectionElement("Pearl Necklace, $1599.99", 1599.99),
                new CollectionElement("Wedding Ring, $799.99", 799.99),
                new CollectionElement("Silver Earrings, $399.99", 399.99),
                new CollectionElement("Golden Bracelet, $449.99", 449.99)
            };

            jNames.ItemsSource = jList;
            jNames.SelectedIndex = 0;

            itemsList.ItemsSource = cart;

            collectionDictionary["electronics"] = eNames;
            collectionDictionary["books"] = bNames;
            collectionDictionary["toys"] = tNames;
            collectionDictionary["jewelry"] = jNames;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Button _myButton = (Button)sender;
            string value = _myButton.CommandParameter.ToString(); 

            Item i = (Item)collectionDictionary[value].SelectedValue;

            if (items.Contains(i.Name))
            {
                foreach (Item item in cart)
                {
                    if (item.Name == i.Name)
                    {
                        if (item.Quantity == 1) { items.Remove(i.Name); cart.Remove(item); item.Quantity--; Subtotal -= item.Price; Tax = Subtotal * (0.18); Total = Subtotal + Tax; }
                        else { item.Quantity--; Subtotal -= item.Price; Tax = Subtotal * (0.18); Total = Subtotal + Tax; }
                        break;
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Button _myButton = (Button)sender;
            string value = _myButton.CommandParameter.ToString(); 

            Item i = (Item)collectionDictionary[value].SelectedValue;

            if (items.Contains(i.Name))
            {
                foreach (Item item in cart)
                {
                    if (item.Name == i.Name) { item.Quantity++; Subtotal += item.Price; Tax = Subtotal * (0.18); Total = Subtotal + Tax; break; }
                }
            }
            else
            {
                cart.Add(i);
                items.Add(i.Name);
                i.Quantity++;
                Subtotal += i.Price;
                Tax = Subtotal * (0.18);
                Total = Subtotal + Tax;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Subtotal = 0;
            Total = 0;
            Tax = 0;
            items.Clear();
            cart.Clear();
        }

        private void itemsList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Subtotal = 0;

            foreach (Item item in cart)
            {
                Subtotal += (item.Quantity) * item.Price;

            }
            Tax = Subtotal * (0.18);
            Total = Subtotal + Tax;

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = e.Uri.ToString(), UseShellExecute = true });
        }
    }

    public interface Item : INotifyPropertyChanged
    {
        string Name { get; set; }
        double Price { get; set; }
        public int Quantity { get; set; }

    }

    public class CollectionElement : Item
    {
        string name;
        public string Name { 
            get { return name; } 
            set { if (name != value) { name = value; OnPropertyChanged(); } } 
        }

        double price;
        public double Price { 
            get { return price; } 
            set { if (price != value) { price = value; OnPropertyChanged(); } } 
        }

        int quantity;
        public int Quantity { 
            get { return quantity; } 
            set { if (quantity != value) { quantity = value; OnPropertyChanged(); } } 
        }

        public CollectionElement(string name, double price)
        {
            Name = name;
            Price = price;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}