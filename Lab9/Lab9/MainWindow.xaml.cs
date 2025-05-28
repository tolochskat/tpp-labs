using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Lab9
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PersonalDataPage());
        }
    }

    // Класс для хранения всех введенных данных
    public class FormData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }

    // Страница 1: Личные данные
    public class PersonalDataPage : Page
    {
        private FormData formData;
        private TextBox firstNameTextBox;
        private TextBox lastNameTextBox;
        private DatePicker birthDatePicker;

        public PersonalDataPage(FormData data = null)
        {
            formData = data ?? new FormData();
            InitializeUI();
        }

        private void InitializeUI()
        {
            var stackPanel = new StackPanel { Margin = new Thickness(20) };

            // Заголовок
            stackPanel.Children.Add(new Label { Content = "Личные данные", FontSize = 18, FontWeight = FontWeights.Bold });

            // Поля ввода
            firstNameTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            lastNameTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            birthDatePicker = new DatePicker { Margin = new Thickness(0, 10, 0, 0) };

            stackPanel.Children.Add(new Label { Content = "Имя:" });
            stackPanel.Children.Add(firstNameTextBox);
            stackPanel.Children.Add(new Label { Content = "Фамилия:" });
            stackPanel.Children.Add(lastNameTextBox);
            stackPanel.Children.Add(new Label { Content = "Дата рождения:" });
            stackPanel.Children.Add(birthDatePicker);

            // Кнопка Далее
            var nextButton = new Button { Content = "Далее", Margin = new Thickness(0, 20, 0, 0), Width = 100 };
            nextButton.Click += NextButton_Click;
            stackPanel.Children.Add(nextButton);

            // Восстановление данных, если они есть
            if (!string.IsNullOrEmpty(formData.FirstName))
                firstNameTextBox.Text = formData.FirstName;
            if (!string.IsNullOrEmpty(formData.LastName))
                lastNameTextBox.Text = formData.LastName;
            if (formData.BirthDate.HasValue)
                birthDatePicker.SelectedDate = formData.BirthDate;

            Content = stackPanel;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите фамилию.");
                return;
            }

            if (birthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения.");
                return;
            }

            // Сохранение данных
            formData.FirstName = firstNameTextBox.Text;
            formData.LastName = lastNameTextBox.Text;
            formData.BirthDate = birthDatePicker.SelectedDate;

            // Переход на следующую страницу
            NavigationService.Navigate(new ContactDataPage(formData));
        }
    }

    // Страница 2: Контактные данные
    public class ContactDataPage : Page
    {
        private FormData formData;
        private TextBox emailTextBox;
        private TextBox phoneTextBox;

        public ContactDataPage(FormData data)
        {
            formData = data;
            InitializeUI();
        }

        private void InitializeUI()
        {
            var stackPanel = new StackPanel { Margin = new Thickness(20) };

            // Заголовок
            stackPanel.Children.Add(new Label { Content = "Контактные данные", FontSize = 18, FontWeight = FontWeights.Bold });

            // Поля ввода
            emailTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            phoneTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };

            stackPanel.Children.Add(new Label { Content = "Email:" });
            stackPanel.Children.Add(emailTextBox);
            stackPanel.Children.Add(new Label { Content = "Телефон:" });
            stackPanel.Children.Add(phoneTextBox);

            // Кнопки
            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 20, 0, 0) };

            var backButton = new Button { Content = "Назад", Width = 100, Margin = new Thickness(0, 0, 10, 0) };
            backButton.Click += BackButton_Click;
            buttonsPanel.Children.Add(backButton);

            var nextButton = new Button { Content = "Далее", Width = 100 };
            nextButton.Click += NextButton_Click;
            buttonsPanel.Children.Add(nextButton);

            stackPanel.Children.Add(buttonsPanel);

            // Восстановление данных, если они есть
            if (!string.IsNullOrEmpty(formData.Email))
                emailTextBox.Text = formData.Email;
            if (!string.IsNullOrEmpty(formData.Phone))
                phoneTextBox.Text = formData.Phone;

            Content = stackPanel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация email
            string email = emailTextBox.Text;
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return;
            }

            // Валидация телефона (простая проверка - минимум 5 цифр)
            string phone = phoneTextBox.Text;
            if (string.IsNullOrWhiteSpace(phone) || !Regex.IsMatch(phone, @"^[\d\s\(\)\-+]{5,}$"))
            {
                MessageBox.Show("Пожалуйста, введите корректный номер телефона (минимум 5 цифр).");
                return;
            }

            // Сохранение данных
            formData.Email = email;
            formData.Phone = phone;

            // Переход на следующую страницу
            NavigationService.Navigate(new AddressPage(formData));
        }
    }

    // Страница 3: Адрес
    public class AddressPage : Page
    {
        private FormData formData;
        private TextBox cityTextBox;
        private TextBox streetTextBox;
        private TextBox houseNumberTextBox;
        private TextBox apartmentNumberTextBox;

        public AddressPage(FormData data)
        {
            formData = data;
            InitializeUI();
        }

        private void InitializeUI()
        {
            var stackPanel = new StackPanel { Margin = new Thickness(20) };

            // Заголовок
            stackPanel.Children.Add(new Label { Content = "Адрес", FontSize = 18, FontWeight = FontWeights.Bold });

            // Поля ввода
            cityTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            streetTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            houseNumberTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
            apartmentNumberTextBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };

            stackPanel.Children.Add(new Label { Content = "Город:" });
            stackPanel.Children.Add(cityTextBox);
            stackPanel.Children.Add(new Label { Content = "Улица:" });
            stackPanel.Children.Add(streetTextBox);
            stackPanel.Children.Add(new Label { Content = "Номер дома:" });
            stackPanel.Children.Add(houseNumberTextBox);
            stackPanel.Children.Add(new Label { Content = "Номер квартиры (опционально):" });
            stackPanel.Children.Add(apartmentNumberTextBox);

            // Кнопки
            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 20, 0, 0) };

            var backButton = new Button { Content = "Назад", Width = 100, Margin = new Thickness(0, 0, 10, 0) };
            backButton.Click += BackButton_Click;
            buttonsPanel.Children.Add(backButton);

            var submitButton = new Button { Content = "Отправить", Width = 100 };
            submitButton.Click += SubmitButton_Click;
            buttonsPanel.Children.Add(submitButton);

            stackPanel.Children.Add(buttonsPanel);

            // Восстановление данных, если они есть
            if (!string.IsNullOrEmpty(formData.City))
                cityTextBox.Text = formData.City;
            if (!string.IsNullOrEmpty(formData.Street))
                streetTextBox.Text = formData.Street;
            if (!string.IsNullOrEmpty(formData.HouseNumber))
                houseNumberTextBox.Text = formData.HouseNumber;
            if (!string.IsNullOrEmpty(formData.ApartmentNumber))
                apartmentNumberTextBox.Text = formData.ApartmentNumber;

            Content = stackPanel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация обязательных полей
            if (string.IsNullOrWhiteSpace(cityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите город.");
                return;
            }

            if (string.IsNullOrWhiteSpace(streetTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите улицу.");
                return;
            }

            if (string.IsNullOrWhiteSpace(houseNumberTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите номер дома.");
                return;
            }

            // Сохранение данных
            formData.City = cityTextBox.Text;
            formData.Street = streetTextBox.Text;
            formData.HouseNumber = houseNumberTextBox.Text;
            formData.ApartmentNumber = apartmentNumberTextBox.Text;

            // Отображение всех данных
            string message = $"Личные данные:\n" +
                           $"Имя: {formData.FirstName}\n" +
                           $"Фамилия: {formData.LastName}\n" +
                           $"Дата рождения: {formData.BirthDate:dd.MM.yyyy}\n\n" +
                           $"Контактные данные:\n" +
                           $"Email: {formData.Email}\n" +
                           $"Телефон: {formData.Phone}\n\n" +
                           $"Адрес:\n" +
                           $"Город: {formData.City}\n" +
                           $"Улица: {formData.Street}\n" +
                           $"Дом: {formData.HouseNumber}\n" +
                           $"Квартира: {formData.ApartmentNumber}";

            MessageBox.Show(message, "Все введенные данные");
        }
    }
}