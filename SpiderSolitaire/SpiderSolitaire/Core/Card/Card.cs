using System;
using System.Collections;
using System.Collections.Generic;

using MvvmLight;
using SpiderSolitaire.Model;

namespace SpiderSolitaire.Core.PlayingCard
{
    /// <summary>
    /// 纸牌类
    /// </summary>
    public sealed class Card : ViewModelBase
    {
        public Card(Suit suit, CardNumber number)
        {
            Moves = new Stack<Move>();
            GameDeckNumber = -1;
            CompletedDeckNumber = -1;
            IndexInDeck = -1;
            Suit = suit;
            CardNumber = number;
            Left = 145.5; 
            Top = 0.5;
            ZIndex = 0;
            ShowFront = false;
            ScaleBack = 1;
            ScaleFront = 0;
        }

        private bool showfront;
        /// <summary>
        /// 指示卡牌是否显示正面图片
        /// </summary>
        /// <remarks>
        /// 所有卡牌背面图片都一样，正面图片与花色和数字有关
        /// </remarks>
        public bool ShowFront
        {
            get
            {
                return showfront;
            }
            set
            {
                showfront = value;
                RaisePropertyChanged();
            }
        }

        private Suit suit;
        /// <summary>
        /// 纸牌花色
        /// </summary>
        public Suit Suit
        {
            get
            {
                return this.suit;
            }
            set
            {
                this.suit = value;
                RaisePropertyChanged();
            }
        }

        private CardNumber cardnumber;
        /// <summary>
        /// 纸牌数字
        /// </summary>
        public CardNumber CardNumber
        {
            get
            {
                return this.cardnumber;
            }
            set
            {
                this.cardnumber = value;
                RaisePropertyChanged();
            }
        }

        private CardStatus cardstatus;
        public CardStatus CardStatus
        {
            get
            {
                return cardstatus;
            }
            set
            {
                cardstatus = value;
                RaisePropertyChanged();
            }
        }

        private bool isselected;
        /// <summary>
        /// 指示纸牌是否被选择
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                RaisePropertyChanged();
            }
        }

        private int gamedecknumber;
        /// <summary>
        /// 纸牌的所属游戏区域牌堆上的索引
        /// </summary>
        public int GameDeckNumber
        {
            get
            {
                return gamedecknumber;
            }
            set
            {
                if(value < -1 || value > 10)
                {
                    throw new IndexOutOfRangeException("超出索引了");
                }

                gamedecknumber = value;
                RaisePropertyChanged();
            }
        }

        private int completeddecknumber;
        /// <summary>
        /// 纸牌的所属已完成区域牌堆上的索引
        /// </summary>
        public int CompletedDeckNumber
        {
            get
            {
                return completeddecknumber;
            }
            set
            {
                if (value < -1 || value > 9)
                {
                    throw new IndexOutOfRangeException("超出索引了");
                }

                completeddecknumber = value;
                RaisePropertyChanged();
            }
        }

        private int indexindeck;
        /// <summary>
        /// 纸牌在指定牌堆上的索引位置（顺序位置）
        /// </summary>
        public int IndexInDeck
        {
            get
            {
                return indexindeck;
            }
            set
            {
                indexindeck = value;
                RaisePropertyChanged();
            }
        }

        private int zindex;
        /// <summary>
        /// 获取或设置纸牌在画布中的层级表示，让下方纸牌在上方纸牌显示
        /// </summary>
        public int ZIndex
        {
            get
            {
                return zindex;
            }
            set
            {
                zindex = value;
                RaisePropertyChanged();
            }
        }

        private double left;
        /// <summary>
        /// 平移偏移量的 X 轴值
        /// </summary>
        public double Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
                RaisePropertyChanged();
            }
        }

        private double top;
        /// <summary>
        /// 平移偏移量的 Y 轴值
        /// </summary>
        public double Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
                RaisePropertyChanged();
            }
        }

        private double scaleback;
        /// <summary>
        /// 背面缩放大小
        /// </summary>
        /// <remarks>
        /// 此属性与 <seealso cref="ScaleFront"/> 属性共同作用于翻转动画 
        /// </remarks>
        public double ScaleBack
        {
            get
            {
                return scaleback;
            }
            set
            {
                scaleback = value;
                RaisePropertyChanged();
            }
        }

        private double scalefront;
        /// <summary>
        /// 正面缩放大小
        /// </summary>
        /// <remarks>
        /// 此属性与 <seealso cref="ScaleBack"/> 属性共同作用于翻转动画 
        /// </remarks>
        public double ScaleFront
        {
            get
            {
                return scalefront;
            }
            set
            {
                scalefront = value;
                RaisePropertyChanged();
            }
        }

        private bool canhover;
        public bool CanHover
        {
            get
            {
                return canhover;
            }
            set
            {
                canhover = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 纸牌移动步骤，方便撤销
        /// </summary>
        public Stack<Move> Moves
        {
            get;
        }

        /// <summary>
        /// 获取当前纸牌相对于父元素的四角坐标集合（左上，右上，左下，右下）
        /// </summary>
        public IList<System.Windows.Point> GetCornersPoints()
        {
            IList<System.Windows.Point> points = new List<System.Windows.Point>()
            {
                new System.Windows.Point(Left, Top),
                new System.Windows.Point(Left + 117, Top),
                new System.Windows.Point(Left, Top + 164),
                new System.Windows.Point(Left + 117, Top + 164),
            };

            return points;
        }

        public void Reset()
        {
            Moves.Clear();
            ScaleBack = 1;
            ScaleFront = 0;
            GameDeckNumber = -1;
            CompletedDeckNumber = -1;
            IndexInDeck = 0;
            ZIndex = 0;
            CardStatus = CardStatus.Initial;
            ShowFront = false;

            //Left，Top偏移量请根据实际情况来设置
            Left = 148.5;
            Top = 0.5;
        }
    }
}
