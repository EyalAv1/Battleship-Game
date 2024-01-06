using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipTrail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object save_content;
        public MainWindow()
        {
            InitializeComponent();
            save_content = Content;
        }
        public void go_back_to_start_page()
        {
            Content = save_content;
            Application.Current.MainWindow.Height = 700;
            Application.Current.MainWindow.Width = 1000;
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            start asd = new start(this);
            this.Content = asd;
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            guide asd = new guide(this);
            this.Content = asd;
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("?האם אתה רוצה לצאת מהמשחק", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

    }
}
