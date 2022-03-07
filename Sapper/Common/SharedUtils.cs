using System;
using System.Windows.Media.Imaging;

namespace Minesweeper.Common
{
    static public class SharedUtils
    {
        static public readonly BitmapImage MineImage = new(new Uri("pack://application:,,,/Images/mine.png"));

        static public readonly BitmapImage SmileImage = new(new Uri("pack://application:,,,/Images/sm.png"));

        static public readonly BitmapImage DeadSmileImage = new(new Uri("pack://application:,,,/Images/dead.png"));

        static public readonly BitmapImage FlagImage = new(new Uri("pack://application:,,,/Images/flag.png"));
    }
}
