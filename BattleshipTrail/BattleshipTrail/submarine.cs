using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace BattleshipTrail
{
    public class submarine
    {
        private string subname;
        private Position position;
        private int size;
        private BitmapImage subBitmap;
        public submarine(string subName)
        {

            subname = subName;
            position = (subname.ElementAt(subname.Length - 2) == 'h') ? Position.HORIZONTAL : Position.VERTICAL;
            size = (subname.ElementAt(subname.Length - 1)) - '0';

            // Create an Image element.
            subBitmap = new BitmapImage(new Uri(@"pack://application:,,,/Sub_picks/" + subname + ".png", UriKind.Absolute));
            if (subBitmap == null)
                Console.WriteLine("null");
            else
                Console.WriteLine("not null");
            
        }

        public SubImage[] CreateCropedImageArray()
        {
            SubImage[] arr = new SubImage[size];
            SubParts partsAmount = new SubParts();
            partsAmount.currentParts = size;
            for (int i = 0; i < size; i++)
            {
                CroppedBitmap cb = new CroppedBitmap(
               subBitmap,
               new Int32Rect((position == Position.HORIZONTAL) ? i * 45 : 0, (position == Position.VERTICAL) ? i * 45:0, 45, 45));
                arr[i] = new SubImage();
                arr[i].Source = cb;
                arr[i].subParts = partsAmount;
                //Console.WriteLine("TTTRRR "+arr[i].SubParts);
            }
            
            //arr[arr.Length - 1].SubParts = size;
            
            return (arr);
        }

        public BitmapImage getImage()
        {
            return (subBitmap);
        }
        public Position PositionSub
        {
            get
            {
                return (position);
            }
        }
        public int Length
        {
            get
            {
                return (size);
            }
        }

    }
}
