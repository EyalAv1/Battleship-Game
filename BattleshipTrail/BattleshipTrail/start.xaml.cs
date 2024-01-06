using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BattleshipTrail
{
    /// <summary>
    /// Interaction logic for start.xaml
    /// </summary>
    public partial class start : Page
    {
        private bool isStartClicked;
        private int numOfSubs = 5;
        private int playerCounter;
        private bool startOn;
        private int[] subsAmount;
        private List<submarine> submarineList;
        private MainWindow start_window;
        private ImageSource[] objImg;
        private submarine tempSubarine;
        private Logic logic;
        private MediaPlayer sound1;
        private MediaPlayer sound2;
        public start(MainWindow start_window)
        {
            InitializeComponent();
            sound1 = new MediaPlayer();
            sound1.Open(new Uri(@"F:\משחק צוללות 15.5.19 נסיונות\BattleshipTrail\BattleshipTrail\explode.wav"));
            sound2 = new MediaPlayer();
            sound2.Open(new Uri(@"F:\משחק צוללות 15.5.19 נסיונות\BattleshipTrail\BattleshipTrail\royalty_trumpet.mp3"));
            isStartClicked = true;
            //Sound1 = new MediaPlayer();
            //s1 = new System.Media.SoundPlayer("c:/explode.wav");
            playerCounter = 5;
            subsAmount = new int[] { 0, 0, 2, 1, 1, 1 };
            logic = new Logic(gameB);
            submarineList = new List<submarine>();
            startOn = false;
            this.start_window = start_window;
            Application.Current.MainWindow.Height = 700;
            Application.Current.MainWindow.Width = 1000;
            objImg = new ImageSource[3];
           
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("?האם אתה רוצה לצאת מהמשחק", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                start_window.go_back_to_start_page();
            }
        }
        //private static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        //{
        //    if (target == null)
        //    {
        //        return null;
        //    }
        //    Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
        //    RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
        //                                                    (int)(bounds.Height * dpiY / 96.0),
        //                                                    dpiX,
        //                                                    dpiY,
        //                                                    PixelFormats.Pbgra32);
        //    DrawingVisual dv = new DrawingVisual();
        //    using (DrawingContext ctx = dv.RenderOpen())
        //    {
        //        VisualBrush vb = new VisualBrush(target);
        //        ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
        //    }
        //    rtb.Render(dv);
        //    return rtb;
        //}
        private void CutImage(string img)
        {
            int count = 0;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(img, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    objImg[count++] = new CroppedBitmap(src, new Int32Rect(j * 120, i * 120, 120, 120));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void GMouseD(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(gameB);
            int row = (int)((p.Y - 45) / 45);
            int col = (int)((p.X) / 45);
            if (col > 12)
            {
                if (playerCounter > 0)
                {
                    if (tempSubarine != null)
                    {
                        //Point p = e.GetPosition(gameB);
                        //int row = (int)((p.Y - 45) / 45);
                        //int col = (int)((p.X) / 45);
                        ////MessageBox.Show((col - 13) +"ujo" + row);
                        if (col > 12)
                        {
                            if (logic.IsLegalPlace(col - 13 + 1, row + 1, tempSubarine.Length, tempSubarine.PositionSub, 1))
                            {
                                SubImage[] imageArr = tempSubarine.CreateCropedImageArray();

                                for (int i = 0; i < imageArr.Length; i++)
                                {
                                    //Console.WriteLine("WWWWW " + imageArr[i].SubParts);
                                    Grid.SetColumn(imageArr[i], col + (tempSubarine.PositionSub == Position.HORIZONTAL ? i : 0));
                                    Grid.SetRow(imageArr[i], row + 1 + (tempSubarine.PositionSub == Position.VERTICAL ? i : 0));
                                    gameB.Children.Add(imageArr[i]);
                                }
                                //TextBlock t = new TextBlock();
                                //t.Text = "X";
                                //t.FontSize = 40;
                                //t.FontWeight = FontWeights.UltraBold;
                                //t.FontStretch = FontStretches.ExtraExpanded;
                                //t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                                //t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                //Grid.SetColumn(t, col);
                                //Grid.SetRow(t, row + 1);
                                //gameB.Children.Add(t);
                                logic.SubsLocationProcess(imageArr.Length);
                                playerCounter--;
                                if (playerCounter == 0)
                                {
                                    logic.CollectFreeHumanSpots();
                                    //logic.Print();
                                }
                                //MessageBox.Show("the player counter: " + playerCounter + "computer counter: " + computerCounter);
                            }
                            else
                            {
                                subsAmount[tempSubarine.Length]++;
                            }


                        }
                        else
                        {
                            subsAmount[tempSubarine.Length]++;
                        }
                        tempSubarine = null;
                    }
                }
                else
                    if(isStartClicked==false)
                {
                    //Console.WriteLine("ghghgh " + (row + 1) + " " + (col - 12));
                    var element = gameB.Children.Cast<UIElement>().
                        FirstOrDefault(e1 => Grid.GetColumn(e1) == col - 13 + 1 && Grid.GetRow(e1) == row + 1);
                    if (logic.CheckHit(col - 13 + 1, row + 1, 1))
                    {
                        //Console.WriteLine("uygu " + (row + 1) + " " + (col - 12));
                        //var element = gameB.Children.Cast<UIElement>().
                        //FirstOrDefault(e1 => Grid.GetColumn(e1) == col - 13 + 1 && Grid.GetRow(e1) == row + 1);
                        //Console.WriteLine("uygu " + (row + 1) + " " + (col - 12));
                        Console.WriteLine("jkjkjk " + ((SubImage)element).SubParts);
                        sound1.Stop();
                        sound1.Play();
                        ((SubImage)element).Explode();
                        ((SubImage)element).Opacity = 1;
                        ((SubImage)element).SubParts = 1;
                        Console.WriteLine("fgfgfg " + ((SubImage)element).SubParts);
                        if (((SubImage)element).SubParts == 0)
                        {
                            numOfSubs--;
                            sound2.Stop();
                            sound2.Play();
                            //s1.PlaySync();
                            //s1.Play();
                            ////MediaPlayer Sound1 = new MediaPlayer();
                            //Sound1.Open(new Uri(@"/explode.wav"));
                            //Sound1.Play(); 
                            if (numOfSubs == 0)
                            {
                                MessageBox.Show("Human player won!");
                            }
                        }
                    }
                    else if (element == null)
                    {
                        //Console.WriteLine("uygu " + (row + 1) + " " + (col - 12));
                        TextBlock t = GetTextB();
                        //t.Text = "X";
                        //t.FontSize = 40;
                        //t.FontWeight = FontWeights.UltraBold;
                        //t.FontStretch = FontStretches.ExtraExpanded;
                        //t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        //Grid.SetColumn(t, col);
                        //Grid.SetRow(t, row + 1);
                        //gameB.Children.Add(t);
                        TextBlock t2 = GetTextB();
                        Grid.SetColumn(t2, col - 12);
                        Grid.SetRow(t2, row + 1);
                        gameB.Children.Add(t2);
                    }
                    logic.ComputerMove();
                }
            }
            else
            {
                if (col > 0 && col < 11)
                {
                    MessageBox.Show("you clicked on the wrong board");
                }
            }
        }

        private TextBlock GetTextB()
        {
            TextBlock t = new TextBlock();
            t.Text = "X";
            t.FontSize = 40;
            t.FontWeight = FontWeights.UltraBold;
            t.FontStretch = FontStretches.ExtraExpanded;
            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            return (t);
        }
       

        private void ListView_buttonD(object sender, MouseButtonEventArgs e)
        {

        }

        private void listView1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void listView1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (startOn == false)
            {
                var item = (sender as ListViewItem);
                if (item != null)
                {
                    int length = int.Parse(item.Tag.ToString());
                    if (subsAmount[length] > 0)
                    {

                        //MessageBox.Show(length + " g");
                        var result = MessageBox.Show("?האם צוללת אנכית", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //int index = listView1.SelectedIndex;
                        // Console.WriteLine("kjb " + index);
                        if (result == MessageBoxResult.Yes)
                        {

                            SubTypes type = (length == 5) ? SubTypes.sv5 : (length == 4) ? SubTypes.sv4 : (length == 3) ? SubTypes.sv3 : SubTypes.sv2;
                            tempSubarine = new submarine(type.ToString());
                            subsAmount[length]--;
                        }
                        else
                        {

                            SubTypes type = (length == 5) ? SubTypes.sh5 : (length == 4) ? SubTypes.sh4 : (length == 3) ? SubTypes.sh3 : SubTypes.sh2;
                            tempSubarine = new submarine(type.ToString());
                            subsAmount[length]--;

                        }
                    }

                }
            }
        }

        private void Start_Button(object sender, RoutedEventArgs e)
        {
            if (playerCounter != 0)
            {
                MessageBox.Show("you didn't place all the submarines");
            }
            else
            {
                isStartClicked = false;
                var b = (sender as Button);
                b.Visibility = Visibility.Hidden;

            }

        }
    }
}
