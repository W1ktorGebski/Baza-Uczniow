using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BazaUczniow
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Uczen> uczniowie = new ObservableCollection<Uczen>();

        public MainWindow()
        {
            InitializeComponent();
            ListaUczniow.ItemsSource = uczniowie;
        }

        private void DodajUzytkownika(object sender, RoutedEventArgs e)
        {
            
        }

        private void UsunUczniow(object sender, RoutedEventArgs e)
        {
            
        }

        private void Wczytaj(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }
    }

   
}
