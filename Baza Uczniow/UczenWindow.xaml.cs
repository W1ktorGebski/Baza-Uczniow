using System;
using System.Linq;
using System.Windows;

namespace BazaUczniow
{
    public partial class UczenWindow : Window
    {
        public Uczen? UczenWynik { get; private set; }

        public UczenWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var pesel = txtPESEL.Text.Trim();
            var imie = FormatujTekst(txtImie.Text);
            var drugieImie = FormatujTekst(txtDrugieImie.Text);
            var nazwisko = FormatujTekst(txtNazwisko.Text);
            var dataUrodzenia = dpDataUrodzenia.SelectedDate;
            var telefon = txtTelefon.Text.Trim();
            var adres = FormatujTekst(txtAdres.Text);
            var miejscowosc = FormatujTekst(txtMiejscowosc.Text);
            var kod = txtKodPocztowy.Text.Trim();

            txtBlad.Text = "";

            if (string.IsNullOrWhiteSpace(pesel) || pesel.Length != 11 || !pesel.All(char.IsDigit))
                txtBlad.Text += "Niepoprawny PESEL.\n";

            if (!SprawdzSumeKontrolnaPesel(pesel))
                txtBlad.Text += "PESEL ma błędną sumę kontrolną.\n";

            if (string.IsNullOrWhiteSpace(imie)) txtBlad.Text += "Imię jest wymagane.\n";
            if (string.IsNullOrWhiteSpace(nazwisko)) txtBlad.Text += "Nazwisko jest wymagane.\n";
            if (string.IsNullOrWhiteSpace(adres)) txtBlad.Text += "Adres jest wymagany.\n";
            if (string.IsNullOrWhiteSpace(miejscowosc)) txtBlad.Text += "Miejscowość jest wymagana.\n";
            if (string.IsNullOrWhiteSpace(kod)) txtBlad.Text += "Kod pocztowy jest wymagany.\n";
            if (!dataUrodzenia.HasValue) txtBlad.Text += "Data urodzenia jest wymagana.\n";

            if (!dataUrodzenia.HasValue || !PorownajDateZPesel(dataUrodzenia.Value, pesel))
                txtBlad.Text += "PESEL nie pasuje do daty urodzenia.\n";


            if (!string.IsNullOrWhiteSpace(telefon))
            {
                telefon = telefon.Replace(" ", "");
                if (!telefon.StartsWith("+48"))
                    telefon = "+48" + telefon;
            }

            if (!string.IsNullOrEmpty(txtBlad.Text))
                return;

            UczenWynik = new Uczen
            {
                PESEL = pesel,
                Imie = imie,
                DrugieImie = string.IsNullOrWhiteSpace(drugieImie) ? null : drugieImie,
                Nazwisko = nazwisko,
                DataUrodzenia = dataUrodzenia.Value,
                Telefon = string.IsNullOrWhiteSpace(telefon) ? null : telefon,
                Adres = adres,
                Miejscowosc = miejscowosc,
                KodPocztowy = kod
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (CzySaZmiany())
            {
                if (MessageBox.Show("Czy na pewno chcesz anulować? Wprowadzone dane zostaną utracone.",
                    "Anuluj", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }
            DialogResult = false;
            Close();
        }

        private string FormatujTekst(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst)) return tekst;
            tekst = tekst.Trim();
            return char.ToUpper(tekst[0]) + tekst.Substring(1).ToLower();
        }

        private bool SprawdzSumeKontrolnaPesel(string pesel)
        {
            int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int suma = pesel.Take(10).Select((t, i) => (t - '0') * wagi[i]).Sum();
            int kontrolna = (10 - suma % 10) % 10;
            return kontrolna == (pesel[10] - '0');
        }

        private bool PorownajDateZPesel(DateTime data, string pesel)
        {
            int rok = data.Year % 100;
            int mies = data.Year switch
            {
                >= 2000 and < 2100 => data.Month + 20,
                >= 1900 and < 2000 => data.Month,
                _ => -1
            };
            string dzien = data.Day.ToString("D2");

            return pesel.StartsWith($"{rok:D2}{mies:D2}{dzien}");
        }

        private bool CzySaZmiany()
        {
            return !string.IsNullOrWhiteSpace(txtPESEL.Text) ||
                   !string.IsNullOrWhiteSpace(txtImie.Text) ||
                   !string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                   dpDataUrodzenia.SelectedDate.HasValue;
        }
    }
}
