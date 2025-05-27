using System;
using System.Windows;
using System.Windows.Controls;

namespace GuessNumberGame
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int secretNumber;
        private int attempts;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        // Начало новой игры
        private void NewGame()
        {
            secretNumber = random.Next(1, 101);
            attempts = 0;
            HintLabel.Content = "Введите число и нажмите 'Проверить'";
            AttemptsLabel.Content = "Попыток: 0";
            NumberInput.Text = "";
            NumberInput.Focus();
        }

        // Проверка числа
        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что введено число
            if (!int.TryParse(NumberInput.Text, out int userGuess))
            {
                HintLabel.Content = "Пожалуйста, введите число!";
                return;
            }

            // Проверяем диапазон
            if (userGuess < 1 || userGuess > 100)
            {
                HintLabel.Content = "Число должно быть от 1 до 100!";
                return;
            }

            attempts++;
            AttemptsLabel.Content = $"Попыток: {attempts}";

            // Сравниваем с загаданным числом
            if (userGuess < secretNumber)
            {
                HintLabel.Content = "Слишком маленькое! Попробуй еще.";
            }
            else if (userGuess > secretNumber)
            {
                HintLabel.Content = "Слишком большое! Попробуй еще.";
            }
            else
            {
                HintLabel.Content = $"Поздравляю! Ты угадал за {attempts} попыток!";
            }

            NumberInput.Text = "";
            NumberInput.Focus();
        }

        // Начать новую игру
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}
