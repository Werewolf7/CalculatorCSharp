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

namespace CalcTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Первый операнд
        double firstNumber = 0;
        //Второй операнд
        double secondNumber = 0;
        //Текущий оператор
        string operation = "";


        /*
        * Сделано для того, что бы не было магических констант по коду, переменных без имени, и при исправление не нужно было лезть по всем местам в коде
        */

        //Значение по умолчани для текстового окна
        string defaultTextBoxValue = "0";

        //Значение по умолчани для операндов
        double defaultNumberValue = 0;

        //Значение по умолчани для строковых переменных
        string emptyStringValue = "";

        //Значение для инверсии
        double inverseNumberValue = -1;

        //Символ 
        string commaChar = ",";

        //Если делаем что то вроде num / 0
        string uncertainResult = "Результат не определён";


        //отображение вида оператор -> лямбда для данного оператора
        Dictionary<string, Func<double, double, double>> operations = new Dictionary<string, Func<double, double, double>>
        {
            ["+"] = delegate (double firstNumber, double secondNumber) { return firstNumber + secondNumber; },
            ["-"] = delegate (double firstNumber, double secondNumber) { return firstNumber - secondNumber; },
            ["*"] = delegate (double firstNumber, double secondNumber) { return firstNumber * secondNumber; },
            ["/"] = delegate (double firstNumber, double secondNumber) { return firstNumber / secondNumber; },
            ["min"] = delegate (double firstNumber, double secondNumber) { return Math.Min(firstNumber, secondNumber); },
            ["max"] = delegate (double firstNumber, double secondNumber) { return Math.Max(firstNumber, secondNumber); },
            ["avg"] = delegate (double firstNumber, double secondNumber) { return (firstNumber + secondNumber) / 2; },
            ["x^y"] = delegate (double firstNumber, double secondNumber) { return Math.Pow(firstNumber, secondNumber); }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            String number = button.Content.ToString()!;


            if (calc_text_box.Text == defaultTextBoxValue || calc_text_box.Text == uncertainResult)
            {
                calc_text_box.Text = number;
            }
            else
            {
                calc_text_box.Text += number;
            }

            if (operation == emptyStringValue)
            {
                Double.TryParse(calc_text_box.Text, out firstNumber);
            }
            else
            {
                Double.TryParse(calc_text_box.Text, out secondNumber);
            }
        }

        private void button_op_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            operation = button.Content.ToString()!;
            calc_text_box.Text = defaultTextBoxValue;
        }

        private void button_equal_op_Click(object sender, RoutedEventArgs e)
        {
            double result_calc = defaultNumberValue;

            result_calc = operations[operation](firstNumber, secondNumber);
            if (Double.IsInfinity(result_calc))
            {
                calc_text_box.Text = uncertainResult;
                operation = emptyStringValue;
                firstNumber = defaultNumberValue;
                secondNumber = defaultNumberValue;
            }
            else
            {
                calc_text_box.Text = result_calc.ToString();
                operation = emptyStringValue;
                firstNumber = result_calc;
                secondNumber = defaultNumberValue;
            }
        }

        private void button_clear_op_Click(object sender, RoutedEventArgs e)
        {
            firstNumber = defaultNumberValue;
            secondNumber = defaultNumberValue;
            operation = emptyStringValue;
            calc_text_box.Text = defaultTextBoxValue;
        }

        private void button_clear_entry_op_Click(object sender, RoutedEventArgs e)
        {
            if (operation == emptyStringValue)
            {
                firstNumber = defaultNumberValue;
            }
            else
            {
                secondNumber = defaultNumberValue;
            }
            calc_text_box.Text = defaultTextBoxValue;
        }

        private void button_backspace_op_Click(object sender, RoutedEventArgs e)
        {
            if (calc_text_box.Text == uncertainResult)
            {
                calc_text_box.Text = defaultTextBoxValue;
                return;
            }

            calc_text_box.Text = DeleteLastSymbol(calc_text_box.Text);
            if (operation == emptyStringValue)
            {
                firstNumber = Double.Parse(calc_text_box.Text);
            }
            else
            {
                secondNumber = Double.Parse(calc_text_box.Text); ;
            }
        }

        private void button_plus_minus_op_Click(object sender, RoutedEventArgs e)
        {
            if (operation == emptyStringValue)
            {
                firstNumber *= inverseNumberValue;
                calc_text_box.Text = firstNumber.ToString();
            }
            else
            {
                secondNumber *= inverseNumberValue;
                calc_text_box.Text = secondNumber.ToString();
            }
        }

        private string DeleteLastSymbol(string currentTextBoxValue)
        {
            //если число имеет 1 цифру
            if (currentTextBoxValue.Length == 1)
            {
                currentTextBoxValue = defaultTextBoxValue;
            }
            else
            {
                currentTextBoxValue = currentTextBoxValue.Remove(currentTextBoxValue.Length - 1, 1);
                //если после удаления из числа(например) 10,5 - осталось 10, [обработка запятой в конце]
                if (currentTextBoxValue.EndsWith(commaChar))
                {
                    currentTextBoxValue = currentTextBoxValue.Remove(currentTextBoxValue.Length - 1, 1);
                }
            }
            return currentTextBoxValue;

        }

        private void button_comma_op_Click(object sender, RoutedEventArgs e)
        {
            //если операрция не задана, то вставляем запяту в первое число
            if (operation == emptyStringValue)
            {
                AddComma(firstNumber);
            }
            else
            {
                AddComma(secondNumber);
            }
        }

        private void AddComma(double num)
        {
            //если запятая уже была нажата
            if (calc_text_box.Text.Contains(commaChar))
            {
                return;
            }
            calc_text_box.Text += commaChar;
        }
    }
}
