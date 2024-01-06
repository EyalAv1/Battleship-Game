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
    /// Interaction logic for guide.xaml
    /// </summary>
    public partial class guide : Page
    {
        private MainWindow guide_window;
        public guide(MainWindow guide_window)
        {
            InitializeComponent();
            this.guide_window = guide_window;
            Application.Current.MainWindow.Height = 700;
            Application.Current.MainWindow.Width = 1000;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            guide_window.go_back_to_start_page();
        }

    }
}
