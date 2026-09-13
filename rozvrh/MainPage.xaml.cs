using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;

namespace rozvrh
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void LoadSchedule()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            string[] days = new string[] { "pnd", "vto", "sre", "chet", "pyat" };

            foreach (string day in days)
            {
                for (int lessonNum = 1; lessonNum <= 7; lessonNum++)
                {
                    string key = day + lessonNum;
                    TextBlock txtBlock = this.FindName(key) as TextBlock;

                    if (txtBlock != null)
                    {
                        string subjectName;
                        if (settings.TryGetValue<string>(key, out subjectName) && !string.IsNullOrWhiteSpace(subjectName))
                        {
                            txtBlock.Text = "Урок " + lessonNum + ": " + subjectName;
                        }
                        else
                        {
                            txtBlock.Text = "Урок " + lessonNum + ": —";
                        }
                    }
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadSchedule();
            UpdateLiveTile();
        }

        private void sbros_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите сбросить все настройки? Их нельзя будет вернуть!", "ВНИМАНИЕ", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) {
                MessageBox.Show("Удалено.");
                IsolatedStorageSettings.ApplicationSettings.Clear();
                IsolatedStorageSettings.ApplicationSettings.Save();
                LoadSchedule();
                UpdateLiveTile();
            }
            if (result == MessageBoxResult.Cancel)
            {
                MessageBox.Show("Не удалено.");
            }
        }

        private void Lesson_Tap(object sender, GestureEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            if (element != null)
            {
                string elementName = element.Name;
                NavigationService.Navigate(new Uri("/Page1.xaml?ob=" + elementName, UriKind.Relative));
            }
        }

        private void UpdateLiveTile()
        {
            string dayPrefix = string.Empty;

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday: dayPrefix = "pnd"; break;
                case DayOfWeek.Tuesday: dayPrefix = "vto"; break;
                case DayOfWeek.Wednesday: dayPrefix = "sre"; break;
                case DayOfWeek.Thursday: dayPrefix = "chet"; break;
                case DayOfWeek.Friday: dayPrefix = "pyat"; break;
                default:
                    dayPrefix = string.Empty;
                    break;
            }

            int lessonCount = 0;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!string.IsNullOrEmpty(dayPrefix))
            {
                for (int i = 1; i <= 7; i++)
                {
                    string key = dayPrefix + i;
                    string subject;

                    if (settings.TryGetValue<string>(key, out subject))
                    {
                        if (!string.IsNullOrWhiteSpace(subject) && subject.Trim() != "—")
                        {
                            lessonCount++;
                        }
                    }
                }
            }

            ShellTile mainTile = ShellTile.ActiveTiles.FirstOrDefault();

            if (mainTile != null)
            {
                StandardTileData tileData = new StandardTileData
                {
                    Count = lessonCount,

                    BackTitle = "rozvrh",
                    BackContent = "Сегодня " + lessonCount + " " + GetLessonDeclension(lessonCount)
                };

                mainTile.Update(tileData);
            }
        }

        string GetLessonDeclension(int count)
        {
            int lastDigit = count % 10;
            int lastTwoDigits = count % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                return "уроков.";

            if (lastDigit == 1)
                return "урок.";

            if (lastDigit >= 2 && lastDigit <= 4)
                return "урока.";

            return "уроков.";
        }
    }
}