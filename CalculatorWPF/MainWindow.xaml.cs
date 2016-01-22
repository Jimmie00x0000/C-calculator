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
using NCalculator;
//在XAML包含足够的信息  
//name是控件的名字   Click=后面是click函数的名字
namespace CalculatorWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private calculator c;
        private string currExpression;
        private string showExpression;
        public MainWindow()//默认从这个入口进入
        {
            InitializeComponent();
            c = new calculator();
            textBox.Clear();
            currExpression = "";

        }

        private void equalButton_Click(object sender, RoutedEventArgs e)
        {
           // string s = textBox.Text;
            //s = "1+5^2/2";
            double result = c.getResult(currExpression);
            if (c.CurrentErrorType == 0||c.CurrentErrorType==10)
            {
                textBox.FontSize = 32;
                textBox.Text = System.String.Format("{0}", result);
            }
            else
            {
                textBox.FontSize = 19;
                switch (c.CurrentErrorType)
                {
                    case 1:
                        textBox.Text = "不要把零作被除数，数学夏鸣凤教的？";
                        break;
                    case 2:
                        textBox.Text = "后台逻辑表示困惑，括号是否正确匹配？";
                        break;
                    case 3:
                        textBox.Text = "字符串识别有问题，数字格式是否正确？";
                        break;
                    case 4:
                        textBox.Text = "运算符似乎乱了套，表达式是否合法？";
                        break;
                    case 5:
                        textBox.Text = "找不到操作数。";
                        break;
                    case 6:
                        textBox.Text = "缺少运算符。";
                        break;
                    default:
                        break;
                }
            }
            //currExpression = null;
            currExpression = "";
            showExpression = "";
        }

        private void digit1_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += "1";
            showExpression +="1";
            textBox.Text = showExpression;
        }

        private void digit2_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "2";
            showExpression += "2";
            textBox.Text = showExpression;
        }

        private void digit3_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "3";
            showExpression += "3";
            textBox.Text = showExpression;
        }

        private void digit4_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "4";
            showExpression += "4";
            textBox.Text = showExpression;
        }

        private void digit5_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += "5";
            showExpression += "5";
            textBox.Text = showExpression;
        }

        private void digit6_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "6";
            showExpression += "6";
            textBox.Text = showExpression;
        }

        private void digit7_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += "7";
            showExpression += "7";
            textBox.Text = showExpression;
        }

        private void digit8_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += "8";
            showExpression += "8";
            textBox.Text = showExpression;
        }

        private void digit9_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "9";
            showExpression += "9";
            textBox.Text = showExpression;
        }

        private void digit0_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += "0";
            showExpression += "0";
            textBox.Text = showExpression;
        }

        private void dot_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += ".";
            showExpression += ".";
            textBox.Text = showExpression;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            //if (currExpression == null)
            //    currExpression = "+";
            //else
            currExpression += "+";
            showExpression += "+";
            textBox.Text = showExpression;
        }

        private void sub_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "-";
            showExpression += "-";
            textBox.Text = showExpression;
        }

        private void mul_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "*";
            showExpression += "×";
            textBox.Text = showExpression;
        }

        private void div_Click(object sender, RoutedEventArgs e)
        {
           
            currExpression += "/";
            showExpression += "÷";
            textBox.Text = showExpression;
        }

        private void leftBracket_Click(object sender, RoutedEventArgs e)
        {
            
            currExpression += "(";
            showExpression += "(";
            textBox.Text = showExpression;
        }

        private void rightBracket_Click(object sender, RoutedEventArgs e)
        {
             
            currExpression += ")";
            showExpression += ")";
            textBox.Text = showExpression;
        }

        private void pi_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "P";
            showExpression += "π";
            textBox.Text = showExpression;
        }

        private void e_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "E";
            showExpression += "e";
            textBox.Text = showExpression;
        }

        private void power_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "^";
            showExpression += "^";
            textBox.Text = showExpression;
        }

        private void sqrt_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "$";
            showExpression += "√";
            textBox.Text = showExpression;
        }

       
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            currExpression = "";
            showExpression = "";
            textBox.Text = showExpression;
        }

        private void sqrt_any_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "#";
            showExpression += "√";
            textBox.Text = showExpression;
        }

        private void fact_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "!";
            showExpression += "!";
            textBox.Text = showExpression;
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            //textBox.FontSize = 19;
            currExpression = "";
            showExpression = "作者:雪欲来时  练习C#之作。\n暂不支持大位数运算，技术有限。";
            textBox.Text = showExpression;
            showExpression = "";
        }

        private void sin_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "S";
            showExpression += "sin";
            textBox.Text = showExpression;
        }

        private void cos_Click(object sender, RoutedEventArgs e)
        {
            currExpression += "C";
            showExpression += "cos";
            textBox.Text = showExpression;
        }

        

        //private void digit1_Copy2_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currExpression == null)
        //        currExpression = "1";
        //    else
        //        currExpression += "1";
        //    textBox.Text = currExpression;
        //}

    }
}
