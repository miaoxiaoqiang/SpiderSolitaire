using System.Windows;

namespace SpiderSolitaire.Model
{
    /// <summary>
    /// 纸牌移动记录
    /// </summary>
    public readonly struct Move
    {
        public Move(int gamedeckindex, int oldzindex, int newzindex, Point point)
        {
            GameDeckIndex = gamedeckindex;
            OldZIndex = oldzindex;
            NewZIndex = newzindex;
            Point = point;
        }

        public int GameDeckIndex
        {
            get;
        }

        public int OldZIndex
        {
            get;
        }

        public int NewZIndex
        {
            get;
        }

        public Point Point
        {
            get;
        }
    }
}
