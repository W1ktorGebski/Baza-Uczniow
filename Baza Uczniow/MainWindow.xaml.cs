using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BazaUczniow
{
    public class Uczen
    {
        public string PESEL { get; set; } = string.Empty;
        public string Imie { get; set; } = string.Empty;
        public string? DrugieImie { get; set; }
        public string Nazwisko { get; set; } = string.Empty;
        public DateTime DataUrodzenia { get; set; }
        public string? Telefon { get; set; }
        public string Adres { get; set; } = string.Empty;
        public string Miejscowosc { get; set; } = string.Empty;
        public string KodPocztowy { get; set; } = string.Empty;
    }

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
            var wybrani = ListaUczniow.SelectedItems.Cast<Uczen>().ToList();
            foreach (var uczen in wybrani)
            {
                uczniowie.Remove(uczen);
            }
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Uczen>));
            using (FileStream fs = new FileStream("uczniowie.xml", FileMode.Create))
            {
                serializer.Serialize(fs, uczniowie);
            }
            MessageBox.Show("Zapisano do XML.");
        }



        private void Wczytaj(object sender, RoutedEventArgs e)
        {
            if (File.Exists("uczniowie.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Uczen>));
                using (FileStream fs = new FileStream("uczniowie.xml", FileMode.Open))
                {
                    var deserialized = serializer.Deserialize(fs);

                    if (deserialized is ObservableCollection<Uczen> lista)
                    {
                        uczniowie.Clear();
                        foreach (var uczen in lista)
                            uczniowie.Add(uczen);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się wczytać danych z pliku XML.");
                    }
                }
            }
        }

        private void OProgramie(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacja WPF – Baza uczniów\nWersja 1.0", "O programie");
        }

    }


}
