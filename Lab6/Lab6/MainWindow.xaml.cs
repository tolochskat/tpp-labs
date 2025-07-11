﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        private bool newInput = true;
        private List<string> calculationHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNumber_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (newInput)
            {
                Display.Text = button.Content.ToString();
                newInput = false;
            }
            else
            {
                Display.Text += button.Content.ToString();
            }
        }

        private void BtnOperator_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (newInput && button.Content.ToString() != "(" && button.Content.ToString() != ")")
            {
                Display.Text = "0";
                newInput = false;
            }

            Display.Text += button.Content.ToString();
            newInput = false;
        }

        private void BtnDecimal_Click(object sender, RoutedEventArgs e)
        {
            if (newInput)
            {
                Display.Text = "0.";
                newInput = false;
                return;
            }

            // Проверяем, есть ли уже точка в текущем числе
            string currentText = Display.Text;
            int lastOperatorIndex = Math.Max(
                currentText.LastIndexOf('+'),
                Math.Max(
                    currentText.LastIndexOf('-'),
                    Math.Max(
                        currentText.LastIndexOf('*'),
                        Math.Max(
                            currentText.LastIndexOf('/'),
                            Math.Max(
                                currentText.LastIndexOf('('),
                                currentText.LastIndexOf(')')
                            )
                        )
                    )
                )
            );

            string currentNumber = currentText.Substring(lastOperatorIndex + 1);
            if (!currentNumber.Contains("."))
            {
                Display.Text += ".";
            }
        }

        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text.Replace(',', '.');

                // Проверка на деление на ноль
                if (expression.Contains("/0") && !expression.Contains("/0."))
                {
                    throw new DivideByZeroException();
                }

                // Проверка на некорректные выражения
                if (string.IsNullOrWhiteSpace(expression) ||
                    IsUnbalancedParentheses(expression) ||
                    EndsWithOperator(expression))
                {
                    throw new InvalidOperationException();
                }

                var result = new DataTable().Compute(expression, null);

                // Добавляем в историю
                string historyEntry = $"{expression} = {result}";
                calculationHistory.Add(historyEntry);
                HistoryList.Items.Add(historyEntry);

                // Прокручиваем список истории вниз
                HistoryList.ScrollIntoView(historyEntry);

                // Анимация изменения результата на дисплее
                AnimateDisplayChange(result.ToString());
                newInput = true;
            }
            catch (DivideByZeroException)
            {
                AnimateDisplayChange("Ошибка: деление на ноль");
                newInput = true;
            }
            catch (Exception)
            {
                AnimateDisplayChange("Ошибка: некорректный ввод");
                newInput = true;
            }
        }

        // Анимация изменения текста на дисплее
        private void AnimateDisplayChange(string newText)
        {
            // Создаем анимацию затухания
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            // Создаем анимацию появления
            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            // Обработчик завершения первой анимации
            fadeOut.Completed += (s, e) =>
            {
                Display.Text = newText;
                Display.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            // Запускаем первую анимацию
            Display.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private bool IsUnbalancedParentheses(string expression)
        {
            int balance = 0;
            foreach (char c in expression)
            {
                if (c == '(') balance++;
                if (c == ')') balance--;
                if (balance < 0) return true;
            }
            return balance != 0;
        }

        private bool EndsWithOperator(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return true;
            char lastChar = expression[expression.Length - 1];
            return lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '/' || lastChar == '.';
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            // Анимация очистки дисплея
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = true
            };

            fadeAnimation.Completed += (s, args) =>
            {
                Display.Text = "0";
                newInput = true;
            };

            Display.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
        }
    }
}