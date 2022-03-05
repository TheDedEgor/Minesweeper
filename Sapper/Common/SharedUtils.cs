using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sapper.Common
{
    static public class SharedUtils
    {
        static public readonly BitmapImage MineImage = new(new Uri("Images/mine.png", UriKind.Relative));

        static public readonly BitmapImage SmileImage = new(new Uri("Images/sm.png", UriKind.Relative));

        static public readonly BitmapImage DeadSmileImage = new(new Uri("Images/dead.png", UriKind.Relative));

        static public readonly BitmapImage FlagImage = new(new Uri("Images/flag.png", UriKind.Relative));
    }
}
