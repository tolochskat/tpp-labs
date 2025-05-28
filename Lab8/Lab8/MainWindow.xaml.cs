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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Globalization;
using System.IO;
using Microsoft.Win32;


namespace Lab8
{
    public partial class MainWindow : Window
    {
        private bool _isTextChanged;
        private ResourceDictionary _currentLanguage;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguage("ru"); // Русский по умолчанию
        }

        private void SetLanguage(string lang)
        {
            if (_currentLanguage != null)
                Resources.MergedDictionaries.Remove(_currentLanguage);

            _currentLanguage = (ResourceDictionary)Resources[lang];
            Resources.MergedDictionaries.Add(_currentLanguage);

            // Обновляем культуру для корректного форматирования
            CultureInfo.CurrentUICulture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = new CultureInfo(lang);

            Title = (string)_currentLanguage["WindowTitle"];
        }

        private void Language_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            SetLanguage(menuItem.Tag.ToString());
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUnsavedChanges())
                return;

            TextArea.Clear();
            _isTextChanged = false;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUnsavedChanges())
                return;

            var dialog = new OpenFileDialog
            {
                Filter = (string)_currentLanguage["FileFilter"],
                DefaultExt = ".txt"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    TextArea.Text = File.ReadAllText(dialog.FileName);
                    _isTextChanged = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                                  (string)_currentLanguage["Error"],
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = (string)_currentLanguage["FileFilter"],
                DefaultExt = ".txt"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dialog.FileName, TextArea.Text);
                    _isTextChanged = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                                  (string)_currentLanguage["Error"],
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private bool CheckUnsavedChanges()
        {
            if (!_isTextChanged)
                return false;

            return MessageBox.Show(
                (string)_currentLanguage["UnsavedChanges"],
                Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.No;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isTextChanged = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = CheckUnsavedChanges();
        }
    }
}
