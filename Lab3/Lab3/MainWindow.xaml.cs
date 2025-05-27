using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotesApp
{
    public partial class MainWindow : Window
    {
        // Основной список всех заметок
        private List<string> notes = new List<string>();

        // Список для хранения отфильтрованных заметок (поиск)
        private List<string> filteredNotes = new List<string>();

        // Путь к файлу для сохранения заметок
        private string notesFilePath = "notes.txt";

        // Текущий индекс цвета для текстового поля
        private int currentColorIndex = 0;

        // Массив цветов для фона текстового поля
        private readonly string[] textBoxColors =
        {
            "#FFF9C4", // Светло-желтый
            "#FFECB3", // Бежевый
            "#FFE0B2"  // Светло-оранжевый
        };

        // Конструктор главного окна
        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();  // Загружаем заметки из файла
            UpdateNotesList(notes);  // Обновляем список заметок
        }

        // Загрузка заметок из файла
        private void LoadNotes()
        {
            if (File.Exists(notesFilePath))
            {
                notes = new List<string>(File.ReadAllLines(notesFilePath));
            }
        }

        // Сохранение заметок в файл
        private void SaveNotes()
        {
            File.WriteAllLines(notesFilePath, notes);
        }

        // Обновление списка заметок в интерфейсе
        private void UpdateNotesList(List<string> notesList)
        {
            notesListBox.ItemsSource = null;  // Очищаем текущий источник
            notesListBox.ItemsSource = notesList;  // Устанавливаем новый список
        }

        // Обработчик кнопки "Добавить"
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что поле ввода не пустое
            if (!string.IsNullOrWhiteSpace(noteTextBox.Text))
            {
                notes.Add(noteTextBox.Text);  // Добавляем новую заметку
                noteTextBox.Clear();  // Очищаем поле ввода

                // Обновляем список (полный или отфильтрованный)
                UpdateNotesList(string.IsNullOrEmpty(searchTextBox.Text) ? notes : filteredNotes);

                // Выделяем последнюю добавленную заметку
                notesListBox.SelectedIndex = notes.Count - 1;
            }
        }

        // Обработчик кнопки "Сохранить"
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что заметка выбрана и поле не пустое
            if (notesListBox.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(noteTextBox.Text))
            {
                // Находим оригинальную заметку в основном списке
                string selectedNote = notesListBox.SelectedItem.ToString();
                int originalIndex = notes.IndexOf(selectedNote);

                // Если заметка найдена, обновляем ее
                if (originalIndex != -1)
                {
                    notes[originalIndex] = noteTextBox.Text;
                    UpdateNotesList(string.IsNullOrEmpty(searchTextBox.Text) ? notes : filteredNotes);
                }
            }
        }

        // Обработчик кнопки "Удалить"
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (notesListBox.SelectedIndex != -1)
            {
                // Находим и удаляем заметку из основного списка
                string selectedNote = notesListBox.SelectedItem.ToString();
                notes.Remove(selectedNote);

                noteTextBox.Clear();  // Очищаем поле ввода

                // Обновляем список (полный или отфильтрованный)
                UpdateNotesList(string.IsNullOrEmpty(searchTextBox.Text) ? notes : filteredNotes);
            }
        }

        // Обработчик изменения выбранной заметки
        private void notesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (notesListBox.SelectedIndex != -1)
            {
                // Отображаем текст выбранной заметки в поле редактирования
                noteTextBox.Text = notesListBox.SelectedItem.ToString();
            }
        }

        // Обработчик изменения текста в поле поиска
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();
            filteredNotes = new List<string>();

            // Фильтруем заметки по введенному тексту
            foreach (string note in notes)
            {
                if (note.ToLower().Contains(searchText))
                {
                    filteredNotes.Add(note);
                }
            }

            // Обновляем список отфильтрованных заметок
            UpdateNotesList(filteredNotes);
        }

        // Обработчик кнопки изменения цвета текстового поля
        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            // Переключаемся на следующий цвет (циклически)
            currentColorIndex = (currentColorIndex + 1) % textBoxColors.Length;
            var converter = new BrushConverter();

            try
            {
                // Применяем новый цвет к текстовому полю
                noteTextBox.Background = (Brush)converter.ConvertFromString(textBoxColors[currentColorIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении цвета: {ex.Message}");
            }
        }

        // Обработчик закрытия окна
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveNotes();  // Сохраняем заметки перед закрытием
        }
    }
}