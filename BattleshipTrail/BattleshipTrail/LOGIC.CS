using System.Windows;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;




namespace BattleshipTrail
{
    class Logic
    {
        private MediaPlayer sound3;
        private int numOfSubs;
        private List<Point> availableEnemySpots;
        private List<Point> humanList;
        private int[,] GameBoard;
        private int[,] EnemyGameBoard;
        private const int H = 10;
        private const int W = 10;
        public static Random rnd;
        private Grid _gameGrid;
        public Logic(Grid gameGrid)
        {
            _gameGrid = gameGrid;
            GameBoard = new int[H + 2, W + 2];
            EnemyGameBoard = new int[H + 2, W + 2];
            ZeroBoardMat();
            availableEnemySpots = new List<Point>();
            humanList = new List<Point>();
            CollectEnemySpots();
            rnd = new Random();
            numOfSubs = 5;
            sound3 = new MediaPlayer();
            sound3.Open(new Uri(@"F:\משחק צוללות 15.5.19 נסיונות\BattleshipTrail\BattleshipTrail\Sad_Trombon.mp3"));
        }
        public bool CheckHit( int col,int row, int player)
        {
            int[,] tempGameBoard = (player == 1) ? EnemyGameBoard : GameBoard;
            return ((tempGameBoard[row, col] == 1) ? true : false);
        }
        public void CollectEnemySpots()
        {
            for (int i = 0; i < (H) * (W); i++)
            {
                availableEnemySpots.Add( new Point((i%W)+1,(i/W)+1));
            }
        }
        public void CollectFreeHumanSpots()
        {
            for (int i = 0; i < (H) * (W); i++)
            {
                humanList.Add(new Point((i % W) + 1, (i / W) + 1));
            }
            int removeTriger = 0;
            for (int i = 0; i < humanList.Count; )
            {
                if (GameBoard[(int)humanList.ElementAt(i).Y,(int)humanList.ElementAt(i).X] == 0)
                {
                    if (removeTriger == 0)
                    {
                        humanList.RemoveAt(i);
                        removeTriger = (removeTriger + 1) % 2;
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }

            //for (int i = 0; i < (H) * (W); i++)
            //{
            //    if (GameBoard[(i / W) + 1, (i % W) + 1] == 1)
            //    {
            //        humanList.Add(new Point((i % W) + 1, (i / W) + 1));
                    
            //    }
            //}
            //int removeTriger = 0;
            //for (int i = 0; i < (H) * (W); i++)
            //{
            //    if (GameBoard[(i / W) + 1, (i % W) + 1] == 0)
            //    {
            //        if (removeTriger == 0)
            //        {
            //            humanList.Add(new Point((i % W) + 1, (i / W) + 1));
            //            removeTriger = (removeTriger + 1) % 2;
            //        }
            //    }
            //}

        }
        public void SubsLocationProcess(int length)
        {
            submarine tempSubarine;
            int inRange = rnd.Next(availableEnemySpots.Count);
            Point p = availableEnemySpots[inRange];
            Position p2 = ((rnd.Next(2)==0)?Position.HORIZONTAL: Position.VERTICAL);
            while (!IsLegalPlace((int)p.X, (int)p.Y, length, p2,2))
            {
                inRange = rnd.Next(availableEnemySpots.Count);
                p = availableEnemySpots[inRange];
            }
            SubTypes type = (p2 == Position.VERTICAL) ?(length==5)? SubTypes.sv5 :(length==4)? SubTypes.sv4:(length==3)?SubTypes.sv3:SubTypes.sv2:
                (length == 5) ? SubTypes.sh5 : (length == 4) ? SubTypes.sh4 : (length == 3) ? SubTypes.sh3 : SubTypes.sh2;
            tempSubarine = new submarine(type.ToString());
            SubImage[] imageArr = tempSubarine.CreateCropedImageArray();

            for (int i = 0; i < imageArr.Length; i++)
            {
                imageArr[i].Opacity = .0;
                Grid.SetColumn(imageArr[i], (((int)p.X)) + (tempSubarine.PositionSub == Position.HORIZONTAL ? i : 0));
                Grid.SetRow(imageArr[i], ((int)p.Y) + (tempSubarine.PositionSub == Position.VERTICAL ? i : 0));
                _gameGrid.Children.Add(imageArr[i]);
            }
        }
        private void ZeroBoardMat()
        {
            for (int i = 0; i < (H + 2) * (W + 2); i++)
            {
                GameBoard[i / (W + 2), i % (W + 2)] = 0;
                EnemyGameBoard[i / (W + 2), i % (W + 2)] = 0;
            }
        }
        public bool IsLegalPlace(int col, int row, int length, Position p,int player)
        {
            int rangeY, rangeX;
            int[,] tempGameBoard = (player == 1) ? GameBoard : EnemyGameBoard;
            bool legal = true;
            int xoffset = col-1;
            int yoffset = row-1;
            rangeY = (p == Position.VERTICAL) ? (yoffset + length + 2) : (yoffset + 3);
            rangeX = (p == Position.HORIZONTAL) ? (xoffset + length + 2) : (xoffset + 3);
            if (rangeY <13 && rangeX < 13)
            {
                for (int i = yoffset; i < ((p == Position.HORIZONTAL) ? yoffset + 3 : (yoffset + length + 2)) && legal; i++)
                {
                    for (int j = xoffset; j < ((p == Position.HORIZONTAL) ? (xoffset + length + 2) : xoffset + 3) && legal; j++)
                    {
                        legal = (tempGameBoard[i, j] != 0) ? false : true;
                    }
                }
            }
            else
            {
                legal = false;
            }

            if (legal)
            {
                xoffset++;
                yoffset++;
                for (int i = 0; i < length; i++)
                {
                    int r = (p == Position.HORIZONTAL) ? yoffset : (yoffset + i);
                    int c = (p == Position.HORIZONTAL) ? (xoffset + i) : xoffset;
                    tempGameBoard[r, c] = 1;
                    if (player == 2)
                    {
                        bool found = false;
                        for (int j = 0; j < availableEnemySpots.Count&&!found; )
                        {
                            Point spot = availableEnemySpots[j];
                            if (spot.X == c && spot.Y == r)
                            {
                                availableEnemySpots.Remove(spot);
                                found = true;
                            }
                            else
                            {
                                j++;
                            }
                        }
                    }
                }
                //Console.WriteLine("OkJHGFD " + availableEnemySpots.Count);
            }
            return legal;
        }
        //public void Print()
        //{
        //    for (int i = 0; i < 10 * 10; i++)
        //    {
        //        Console.Write("{0} ", EnemyGameBoard[(i / 10)+1, (i % 10)+1]);
        //        if (i % 10 == 9)
        //            Console.WriteLine();
        //    }
        //}

        public void ComputerMove()
        {
            int pos = rnd.Next(humanList.Count);
            Point p = humanList[pos];
            int col = (int)p.X;
            int row = (int)p.Y;
            var element = _gameGrid.Children.Cast<UIElement>().
                    FirstOrDefault(e1 => Grid.GetColumn(e1) == col+12 && Grid.GetRow(e1) == row);
                if (CheckHit(col, row , 2))
                {
                    //Console.WriteLine("uygu " + (row + 1) + " " + (col - 12));
                    //var element = gameB.Children.Cast<UIElement>().
                    //FirstOrDefault(e1 => Grid.GetColumn(e1) == col - 13 + 1 && Grid.GetRow(e1) == row + 1);
                    //Console.WriteLine("uygu " + (row + 1) + " " + (col - 12));
                    //Console.WriteLine("jkjkjk " + ((SubImage)element).SubParts);
                    ((SubImage)element).Explode();
                    ((SubImage)element).SubParts = 1;
                    //Console.WriteLine("fgfgfg " + ((SubImage)element).SubParts);
                    if (((SubImage)element).SubParts == 0)
                    {
                        numOfSubs--;
                        //Console.WriteLine("fgfgfg " + ((SubImage)element).SubParts);
                        if (numOfSubs == 0)
                        {
                            sound3.Play();
                            MessageBox.Show("Computer player won!");
                            
                        }
                    }
                }
                humanList.Remove(p);

        }
    }
}
