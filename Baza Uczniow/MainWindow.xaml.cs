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
            var okno = new UczenWindow();
            if (okno.ShowDialog() == true && okno.UczenWynik != null)
            {
                uczniowie.Add(okno.UczenWynik);
                MessageBox.Show("Dodano ucznia.");
            }
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
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Pliki CSV z separatorem (,)|*.csv|Pliki CSV z separatorem (;)|*.csv";
            saveFileDialog.Title = "Zapisz jako plik CSV";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string delimiter = ",";

                if (saveFileDialog.FilterIndex == 2)
                    delimiter = ";";

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Uczen uczen in uczniowie)
                    {
                        string row = $"{uczen.PESEL}{delimiter}{uczen.Imie}{delimiter}{uczen.DrugieImie}{delimiter}{uczen.Nazwisko}{delimiter}{uczen.DataUrodzenia:yyyy-MM-dd}{delimiter}{uczen.Telefon}{delimiter}{uczen.Adres}{delimiter}{uczen.Miejscowosc}{delimiter}{uczen.KodPocztowy}";
                        writer.WriteLine(row);
                    }
                }

                MessageBox.Show("Zapisano do pliku CSV.");
            }
        }

        private void Wczytaj(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Pliki CSV z separatorem (,)|*.csv|Pliki CSV z separatorem (;)|*.csv";
            openFileDialog.Title = "Otwórz plik CSV";

            if (openFileDialog.ShowDialog() == true)
            {
                uczniowie.Clear();

                string filePath = openFileDialog.FileName;
                string delimiter = ",";
                if (openFileDialog.FilterIndex == 2)
                    delimiter = ";";

                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);

                    foreach (var line in lines)
                    {
                        var columns = line.Split(delimiter);

                        if (columns.Length >= 9)
                        {
                            var uczen = new Uczen
                            {
                                PESEL = columns[0],
                                Imie = columns[1],
                                DrugieImie = string.IsNullOrWhiteSpace(columns[2]) ? null : columns[2],
                                Nazwisko = columns[3],
                                DataUrodzenia = DateTime.TryParse(columns[4], out var data) ? data : DateTime.MinValue,
                                Telefon = string.IsNullOrWhiteSpace(columns[5]) ? null : columns[5],
                                Adres = columns[6],
                                Miejscowosc = columns[7],
                                KodPocztowy = columns[8]
                            };

                            uczniowie.Add(uczen);
                        }
                    }

                    MessageBox.Show("Wczytano dane z pliku CSV.");
                }
            }
        }


        private void OProgramie(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacja WPF – Baza uczniów\nWersja 1.0", "O programie");
        }

    }


}
