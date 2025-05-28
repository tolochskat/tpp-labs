using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GreenNotepad
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = null; // Текущий открытый файл

        public MainWindow()
        {
            InitializeComponent();

            // Установка команд с привязкой к обработчикам
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewCommand_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenCommand_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveCommand_Executed, SaveCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, e) => Close()));
        }

        // Обработчик команды "Новый"
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Проверка на несохраненные изменения
            if (!string.IsNullOrEmpty(TextArea.Text))
            {
                var result = MessageBox.Show("Сохранить изменения в текущем файле?", "Зеленый блокнот",
                                           MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return; // Отмена создания нового файла
                }
            }

            // Очистка текстового поля и сброс текущего пути файла
            TextArea.Clear();
            currentFilePath = null;
            Title = "Блокнот";
        }

        // Обработчик команды "Открыть"
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Диалог открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Чтение файла и отображение содержимого
                    TextArea.Text = File.ReadAllText(openFileDialog.FileName);
                    currentFilePath = openFileDialog.FileName;
                    Title = $"Блокнот - {Path.GetFileName(currentFilePath)}";
                }
                catch (IOException ex)
                {
                    // Обработка ошибок ввода-вывода
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Проверка возможности выполнения команды "Сохранить"
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(TextArea.Text);
        }

        // Обработчик команды "Сохранить"
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFile();
        }

        // Метод для сохранения файла
        private void SaveFile()
        {
            // Если файл уже существует, сохраняем в него
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                try
                {
                    File.WriteAllText(currentFilePath, TextArea.Text);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Иначе открываем диалог сохранения
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, TextArea.Text);
                        currentFilePath = saveFileDialog.FileName;
                        Title = $"Блокнот - {Path.GetFileName(currentFilePath)}";
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}