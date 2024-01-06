using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;

namespace BattleshipTrail
{
    public class SubImage:Image
    {
        private BitmapImage subBitmap;
        public SubParts subParts;
        public SubImage()
        {
            subBitmap = new BitmapImage(new Uri(@"pack://application:,,,/Images/" + "explode.png", UriKind.Absolute));
        }
        public int SubParts
        {
            get
            {
                return subParts.currentParts;
            }
            set 
            {
                subParts.currentParts -= value;
                //Console.WriteLine("kjhbjy");
            }
        }
        public void Explode()
        {
            Source=subBitmap;
        }
    }
}
