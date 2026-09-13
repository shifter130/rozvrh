using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace rozvrh
{
    public partial class Page1 : PhoneApplicationPage
    {
        private string ob;
        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ob"))
            {
                ob = NavigationContext.QueryString["ob"];
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (lessontitle.Text == "") {
                MessageBox.Show("У чистому полі порожнеча, порожнеча, порожнеча...", "Проверьте ввод", MessageBoxButton.OK);
                return;
            }
            if (settings.Contains(ob))
            {
                settings[ob] = lessontitle.Text;
            }
            else
            {
                settings.Add(ob, lessontitle.Text);
            }
            settings.Save();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}