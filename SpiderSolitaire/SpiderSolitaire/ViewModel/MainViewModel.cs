using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MvvmLight;
using MvvmLight.CommandWpf;
using SpiderSolitaire.Core;
using SpiderSolitaire.Core.PlayingCard;
using SpiderSolitaire.Model;

namespace SpiderSolitaire.ViewModel
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly System.Threading.Timer _gameThreadingTimer;
        private readonly System.Diagnostics.Stopwatch sw;

        private bool _mousedown = false;
        private Point beginMousePosition;//触发鼠标移动事件的最初位置，
        private Point movingPosition;//记录鼠标移动中的位置
        private Point selectedPoint;//鼠标按下时获取卡牌的最初坐标
        private double dertTop;
        private double dertLeft;
        private ICardContext selectdcontext;
        private bool gamestart = false;
        private IList<Suit> _suitlist;
        private IList<Card> _selectedcards;
        private bool isfirstdrop = true;

        public MainViewModel()
        {
            sw = new();
            _gameThreadingTimer = new Timer(GameThreadTimerCallback, null, Timeout.Infinite, 1000);
            Scores = 500;
            Steps = 0;
            Duration = 0;
            ShowWin = false;
            _selectedcards = new List<Card>();
            GameDeck = new GameZone();
            CompletedDeck = new CompletedZone();
            Record = new PlayRecord();

            _suitlist = new List<Suit>();
            Start = Visibility.Hidden;
            Level = GameLevel.OneSuit;
            Cards = new ObservableCollection<Card>();

            Suit suit = (Suit)Utils.Helper.GenerateRandom(0, 4);
            foreach (int i in Enumerable.Range(0, 8 / Convert.ToInt32(Math.Pow(2, (int)Level))))
            {
                foreach (CardNumber cardnumber in Enum.GetValues(typeof(CardNumber)).OfType<CardNumber>())
                {
                    Cards.Add(new Card(suit, cardnumber));
                }
            }
        }

        public ObservableCollection<Card> Cards
        {
            get;
        }

        private Visibility start;
        public Visibility Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
                RaisePropertyChanged();
            }
        }

        private GameLevel level;
        public GameLevel Level
        {
            get
            {
                return level;
            }
            set
            {
                if (level != value)
                {
                    level = value;
                    RaisePropertyChanged();

                    ExecuteNewGame();
                }
            }
        }

        private bool initgame;
        public bool InitGame
        {
            get
            {
                return initgame;
            }
            set
            {
                initgame = value;
                RaisePropertyChanged();
            }
        }

        private bool showscore;
        public bool ShowScore
        {
            get
            {
                return showscore;
            }
            set
            {
                showscore = value;
                RaisePropertyChanged();
            }
        }

        private bool showwin;
        public bool ShowWin
        {
            get
            {
                return showwin;
            }
            set
            {
                showwin = value;
                RaisePropertyChanged();
            }
        }

        private long duration;
        public long Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                RaisePropertyChanged();
            }
        }

        private int scores;
        public int Scores
        {
            get
            {
                return scores;
            }
            set
            {
                scores = value;
                RaisePropertyChanged();
            }
        }

        private ushort steps;
        public ushort Steps
        {
            get
            {
                return steps;
            }
            set
            {
                steps = value;
                RaisePropertyChanged();
            }
        }

        public PlayRecord Record
        {
            get;
            set;
        }

        internal GameZone GameDeck
        {
            get;
        }

        internal CompletedZone CompletedDeck
        {
            get;
        }

        #region 开始新游戏命令
        private RelayCommand newgamecommand;
        public RelayCommand NewGameCommand
        {
            get
            {
                newgamecommand ??= new RelayCommand(ExecuteNewGame);

                return newgamecommand;
            }
            set
            {
                newgamecommand = value;
            }
        }

        private async void ExecuteNewGame()
        {
            Scores = 500;
            Steps = 0;
            Duration = 0;
            Start = Visibility.Collapsed;
            InitGame = true;

            GameDeck.Clear();
            CompletedDeck.Clear();
            _suitlist.Clear();

            foreach (var item in Cards)
            {
                item.Reset();
            }

            #region 重选花色和洗牌
            switch (Level)
            {
                case GameLevel.TwoSuit:
                    int _num1 = Utils.Helper.GenerateRandom(0, 2);
                    int _num2 = Utils.Helper.GenerateRandom(2, 4);
                    _suitlist.Add((Suit)_num1);
                    _suitlist.Add((Suit)_num2);
                    break;
                case GameLevel.FourSuit:
                    _suitlist = Enum.GetValues(typeof(Suit)).OfType<Suit>().ToList();
                    break;
                default:
                    _suitlist.Add((Suit)Utils.Helper.GenerateRandom(0, 4));
                    break;
            }

            List<Card> order = Cards.OrderBy(p => p.Suit).ThenBy(p => p.CardNumber).ToList();

            foreach (int i in Enumerable.Range(1, 8 / Convert.ToInt32(Math.Pow(2, (int)Level))))
            {
                for (int k = 0; k < _suitlist.Count; k++)
                {
                    for (int j = 13 * i * k; j < 13 * i * (k + 1); j++)
                    {
                        order[j].Suit = _suitlist[k];
                    }
                }
            }

            Shuffle(Cards);
            #endregion

            Start = Visibility.Visible;

            await Task.Run(async () =>
            {
                int _zindex = 0;
                int _count = 0;
                int _deckindex = 0;

                foreach (Card card in Cards)
                {
                    await Task.Delay(50);

                    card.GameDeckNumber = _deckindex;
                    GameDeck.AddCard(card);

                    if (_count <= 43)
                    {
                        card.CardStatus = CardStatus.DealBack;
                    }
                    else
                    {
                        card.ShowFront = true;
                        card.CardStatus = CardStatus.DealFront;
                    }

                    _deckindex++;
                    _count++;

                    if (_deckindex == 10)
                    {
                        _zindex++;
                        _deckindex = 0;
                    }

                    if (_count == 54)
                    {
                        break;
                    }
                }

                await Task.Delay(50);

                for (int i = 54; i < Cards.Count; i++)
                {
                    await Task.Delay(10);
                    Cards[i].CardStatus = CardStatus.Residue;
                }

                await Task.Delay(330);

                InitGame = false;
                gamestart = true;
            });
        }
        #endregion

        #region 鼠标点击纸牌命令
        private RelayCommand<MouseButtonEventArgs> cardmousedowncommand;
        public RelayCommand<MouseButtonEventArgs> CardMouseDownCommand
        {
            get
            {
                cardmousedowncommand ??= new RelayCommand<MouseButtonEventArgs>(ExecuteCardMouseDown);
                return cardmousedowncommand;
            }
            set
            {
                cardmousedowncommand = value;
            }
        }

        private void ExecuteCardMouseDown(MouseButtonEventArgs e)
        {
            if (!gamestart)
            {
                return;
            }

            //System.Windows.Media.HitTestResult htr = System.Windows.Media.VisualTreeHelper.HitTest(e.Source as System.Windows.Media.Visual, Mouse.GetPosition(Mouse.DirectlyOver));

            selectdcontext = (e.OriginalSource as FrameworkElement).TemplatedParent as ICardContext;
            if (selectdcontext != null)
            {
                CardStatus status = selectdcontext.GetCard.CardStatus;
                if (status == CardStatus.Residue) //发牌
                {
                    int _count = 0;
                    int _deckindex = 0;
                    int dealedcount = GameDeck.GetAllCardsCount() + CompletedDeck.GetAllCardsCount();
                    for (int i = dealedcount; i < Cards.Count; i++)
                    {
                        if (Cards[i].CardStatus == CardStatus.Residue)
                        {
                            Cards[i].GameDeckNumber = _deckindex;
                            GameDeck.AddCard(Cards[i]);
                            Cards[i].ShowFront = true;
                            Cards[i].CardStatus = CardStatus.DealFront;

                            _deckindex++;
                            _count++;

                            if (_count == 10)
                            {
                                break;
                            }
                        }
                    }
                }
                else if (status == CardStatus.DealFront || status == CardStatus.MoveBack
                    || status == CardStatus.Keeping || status == CardStatus.MoveTarget) //选择纸牌
                {
                    Tuple<bool, IList<Card>> tuple = GameDeck.CanMove(selectdcontext.GetCard);
                    if (tuple.Item1)
                    {
                        if (selectdcontext.MouseCaptureElement())
                        {
                            selectdcontext.GetCard.ZIndex = 117;
                            selectdcontext.GetCard.IsSelected = true;
                            beginMousePosition = e.GetPosition(selectdcontext.GetCanvasInputElement);
                            selectedPoint = new Point(selectdcontext.GetCanvasLeft, selectdcontext.GetCanvasTop);

                            if (tuple.Item2 != null)
                            {
                                _selectedcards = tuple.Item2;
                                int _zindex = 118;
                                foreach (Card _card in _selectedcards)
                                {
                                    _card.ZIndex = _zindex;
                                    _card.IsSelected = true;
                                    _zindex++;
                                }
                            }

                            _mousedown = true;
                        }
                    }
                }
            }

            e.Handled = true;
        }
        #endregion

        #region 鼠标释放纸牌命令
        private RelayCommand<MouseButtonEventArgs> cardmouseupcommand;
        public RelayCommand<MouseButtonEventArgs> CardMouseUpCommand
        {
            get
            {
                cardmouseupcommand ??= new RelayCommand<MouseButtonEventArgs>(ExecuteCardMouseUp);
                return cardmouseupcommand;
            }
            set
            {
                cardmouseupcommand = value;
            }
        }

        private async void ExecuteCardMouseUp(MouseButtonEventArgs e)
        {
            if (!gamestart)
            {
                return;
            }

            selectdcontext.ReleaseMouseCaptureElement();

            if (_mousedown)
            {
                var result = GameDeck.CanDrop(selectdcontext.GetCard, e.GetPosition(selectdcontext.GetCanvasInputElement));
                if (result.CanDrop) //表示鼠标处可以释放纸牌
                {
                    Scores--;
                    Steps++;

                    if(isfirstdrop)
                    {
                        isfirstdrop = false;
                        _gameThreadingTimer.Change(0, 1000);
                        sw.Start();
                    }

                    Card prevcardbeforemove = GameDeck.GetPrevCard(selectdcontext.GetCard);

                    _selectedcards.Insert(0, selectdcontext.GetCard);
                    GameDeck.RemoveCards(selectdcontext.GetCard.GameDeckNumber, _selectedcards);//移除原牌堆的被选择的纸牌数据
                    GameDeck.AddCards(result.NewGameDeckIndex, _selectedcards);//在新的牌堆添加已选择的纸牌数据

                    foreach (Card _card in _selectedcards)
                    {
                        //_card.ZIndex = GameDeck.GetCardIndex(_card) + 1;
                        _card.IndexInDeck = GameDeck.GetCardIndex(_card);
                        _card.IsSelected = false;

                        if(_card.CardStatus == CardStatus.MoveTarget)
                        {
                            _card.CardStatus = CardStatus.Keeping;
                        }
                        else
                        {
                            _card.CardStatus = CardStatus.MoveTarget;
                        }
                    }

                    if (prevcardbeforemove != null && prevcardbeforemove.CardStatus == CardStatus.DealBack) //移动纸牌后，纸牌原来位置的背面图片要翻转显示正面图片
                    {
                        prevcardbeforemove.ShowFront = true;
                        prevcardbeforemove.CardStatus = CardStatus.DealFront;
                    }

                    int newdeckindex = selectdcontext.GetCard.GameDeckNumber;
                    selectdcontext = null;

                    await Task.Run(async () =>
                    {
                        await Task.Delay(310);

                        Tuple<bool, IList<Card>> tuple = GameDeck.CanMerge(newdeckindex); //纸牌移动到新的牌堆。判断新的牌堆是否有构成一组完整的序列牌组
                        if (tuple.Item1)//有则移除当前牌堆的完整序列牌组数据，同时将序列牌组移到已完成区牌堆（有空的牌堆，没有则说明8个牌堆已填满，游戏胜利了）
                        {
                            GameDeck.RemoveCards(newdeckindex, tuple.Item2);
                            int newprevaftermove = tuple.Item2.First().IndexInDeck;
                            CompletedDeck.AddCards(CompletedDeck.GetEmptyDeck(), tuple.Item2);

                            for (int i = 0; i < tuple.Item2.Count; i++)
                            {
                                if (i != 0)
                                {
                                    await Task.Delay(150);
                                }

                                tuple.Item2[i].IndexInDeck = CompletedDeck.GetCardIndex(tuple.Item2[i]);
                                tuple.Item2[i].CardStatus = CardStatus.Complete;
                            }

                            MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() =>
                            {
                                Scores += 100 + 1;
                                ShowScore = false;
                                ShowScore = true;
                            });

                            Card prevbackcardaftermerge = GameDeck.GetLastBackCard(newdeckindex);
                            if (prevbackcardaftermerge != null && prevbackcardaftermerge.IndexInDeck - newprevaftermove == -1)
                            {
                                prevbackcardaftermerge.ShowFront = true;
                                prevbackcardaftermerge.CardStatus = CardStatus.DealFront;
                            }

                            int emptyindex = CompletedDeck.GetEmptyDeck();
                            if (emptyindex == -1)
                            {
                                _gameThreadingTimer.Change(-1, Timeout.Infinite);
                                sw.Stop();
                            }

                            if (emptyindex == -1)
                            {
                                await Task.Delay(1400);
                                MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() =>
                                {
                                    ShowWin = true;
                                });

                                await Task.Delay(3600);

                                MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() =>
                                {
                                    ShowWin = false;
                                });
                            }
                        }
                    });
                }
                else //表示在鼠标处不可以释放纸牌，纸牌退回到原来的位置
                {
                    _selectedcards.Insert(0, selectdcontext.GetCard);

                    foreach (Card _card in _selectedcards)
                    {
                        _card.IsSelected = false;
                        //_card.ZIndex = GameDeck.GetCardIndex(_card) + 1;

                        if (_card.CardStatus == CardStatus.DealFront)
                        {
                            _card.CardStatus = CardStatus.MoveBack;
                        }
                        else if (_card.CardStatus == CardStatus.Keeping)
                        {
                            _card.CardStatus = CardStatus.MoveBack;
                        }
                        else if (_card.CardStatus == CardStatus.MoveBack)
                        {
                            _card.CardStatus = CardStatus.Keeping;
                        }
                        else if (_card.CardStatus == CardStatus.MoveTarget)
                        {
                            _card.CardStatus = CardStatus.MoveBack;
                        }
                    }

                    selectdcontext = null;
                }
            }

            _mousedown = false;
            _selectedcards.Clear();

            e.Handled = true;
        }
        #endregion

        #region 鼠标拖动纸牌命令
        private RelayCommand<MouseEventArgs> cardmousemovecommand;
        public RelayCommand<MouseEventArgs> CardMouseMoveCommand
        {
            get
            {
                cardmousemovecommand ??= new RelayCommand<MouseEventArgs>(ExecuteCardMouseMove);
                return cardmousemovecommand;
            }
            set
            {
                cardmousemovecommand = value;
            }
        }

        private void ExecuteCardMouseMove(MouseEventArgs e)
        {
            if (gamestart && _mousedown && selectdcontext != null)
            {
                Card card = selectdcontext.GetCard;

                movingPosition = e.GetPosition(selectdcontext.GetCanvasInputElement);
                dertTop = movingPosition.Y - beginMousePosition.Y;
                dertLeft = movingPosition.X - beginMousePosition.X;

                if (selectedPoint.X + dertLeft <= 0)
                {
                    card.Left = 0;
                }
                else if (selectedPoint.X + selectdcontext.GetCardSize.Width + dertLeft >= selectdcontext.GetCanvasSize.Width)
                {
                    card.Left = selectdcontext.GetCanvasSize.Width - selectdcontext.GetCardSize.Width;
                }
                else
                {
                    card.Left = selectedPoint.X + dertLeft;
                }

                if (selectedPoint.Y + dertTop <= 0)
                {
                    card.Top = 0;
                }
                else if (selectedPoint.Y + selectdcontext.GetCardSize.Height + dertTop >= selectdcontext.GetCanvasSize.Height)
                {
                    card.Top = selectdcontext.GetCanvasSize.Height - selectdcontext.GetCardSize.Height;
                }
                else
                {
                    card.Top = selectedPoint.Y + dertTop;
                }

                if (_selectedcards != null)
                {
                    for (int i = 0; i < _selectedcards.Count; i++)
                    {
                        _selectedcards[i].Top = card.Top + CardOffestData.CardSpaceInGameDeck * (i + 1);
                        _selectedcards[i].Left = card.Left;
                    }
                }
            }

            e.Handled = true;
        }
        #endregion

        #region 撤销命令
        private RelayCommand<MouseButtonEventArgs> drawbackcommand;
        public RelayCommand<MouseButtonEventArgs> DrawBackCommand
        {
            get
            {
                drawbackcommand ??= new RelayCommand<MouseButtonEventArgs>(ExecuteDrawBack);
                return drawbackcommand;
            }
            set
            {
                drawbackcommand = value;
            }
        }

        private void ExecuteDrawBack(MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("此功能尚未开发", "提示");
        }
        #endregion

        /// <summary>
        /// 洗牌
        /// </summary>
        private void Shuffle(IList<Card> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = Utils.Helper.GenerateRandom(0, i);

                Suit suit = cards[i].Suit;
                CardNumber number = cards[i].CardNumber;

                cards[i].Suit = cards[j].Suit;
                cards[i].CardNumber = cards[j].CardNumber;
                cards[j].Suit = suit;
                cards[j].CardNumber = number;
            }
        }

        private void GameThreadTimerCallback(object state)
        {
            Duration += 1;
        }
    }
}
