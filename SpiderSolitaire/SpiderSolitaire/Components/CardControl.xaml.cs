using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SpiderSolitaire.Core;
using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Components
{
    public sealed partial class CardControl : UserControl, ICardContext
    {
        public static readonly DependencyProperty CardStatusProperty =
            DependencyProperty.Register("CardStatus", typeof(Model.CardStatus), typeof(CardControl), new PropertyMetadata(Model.CardStatus.Initial, OnCardStatusChanged));
        public static readonly DependencyProperty ShowFrontProperty =
            DependencyProperty.Register("ShowFront", typeof(bool), typeof(CardControl), new PropertyMetadata(false, OnShowFrontChanged));
        public static readonly DependencyProperty CanHoverProperty =
            DependencyProperty.Register("CanHover", typeof(bool), typeof(CardControl), new PropertyMetadata(false));

        private Canvas _canvas;

        static CardControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardControl), new FrameworkPropertyMetadata(typeof(CardControl)));
        }

        public CardControl()
        {
            InitializeComponent();
            Loaded += CardControlLoaded;
        }

        private void CardControlLoaded(object sender, RoutedEventArgs e)
        {
            _canvas = Utils.Helper.FindVisualParent<Canvas>(this, null);
        }

        public Model.CardStatus CardStatus
        {
            get
            {
                return (Model.CardStatus)GetValue(CardStatusProperty);
            }
            set
            {
                SetValue(CardStatusProperty, value);
            }
        }

        public bool ShowFront
        {
            get
            {
                return Convert.ToBoolean(GetValue(ShowFrontProperty));
            }
            set
            {
                SetValue(ShowFrontProperty, value);
            }
        }

        public bool CanHover
        {
            get
            {
                return Convert.ToBoolean(GetValue(CanHoverProperty));
            }
            set
            {
                SetValue(CanHoverProperty, value);
            }
        }

        private static void OnShowFrontChanged(DependencyObject dpobj, DependencyPropertyChangedEventArgs e)
        {
            bool oldstatus = Convert.ToBoolean(e.OldValue);
            bool newstatus = Convert.ToBoolean(e.NewValue);

            CardControl control = dpobj as CardControl;

            if (oldstatus && !newstatus) //此事件与重置游戏有关，纸牌默认显示背面，隐藏正面
            {
                Image _image1 = control.Template.FindName("ImageCard", control) as Image;
                Image _image2 = control.Template.FindName("ImageFrontCard", control) as Image;
                TransformGroup transforms1 = _image1.RenderTransform as TransformGroup;
                TransformGroup transforms2 = _image2.RenderTransform as TransformGroup;

                (transforms1.Children[0] as ScaleTransform).ScaleX = 1;
                (transforms2.Children[0] as ScaleTransform).ScaleX = 0;
            }
        }

        private static void OnCardStatusChanged(DependencyObject dpobj, DependencyPropertyChangedEventArgs e)
        {
            Model.CardStatus oldstatus = (Model.CardStatus)Enum.ToObject(typeof(Model.CardStatus), e.OldValue);
            Model.CardStatus newstatus = (Model.CardStatus)Enum.ToObject(typeof(Model.CardStatus), e.NewValue);

            if (oldstatus == newstatus && newstatus != Model.CardStatus.DealFront)
            {
                return;
            }

            CardControl control = dpobj as CardControl;
            ICardContext context = dpobj as ICardContext;
            
            if (oldstatus == Model.CardStatus.Initial)//纸牌默认开始为初始状态
            {
                if (context.GetCard.ShowFront && newstatus == Model.CardStatus.DealFront)//发牌显示正面
                {
                    Image _image1 = control.Template.FindName("ImageCard", control) as Image;
                    Image _image2 = control.Template.FindName("ImageFrontCard", control) as Image;
                    TransformGroup transforms1 = _image1.RenderTransform as TransformGroup;
                    TransformGroup transforms2 = _image2.RenderTransform as TransformGroup;

                    CardAnimation.DealCard(
                        transforms1.Children[0] as ScaleTransform,
                        transforms2.Children[0] as ScaleTransform,
                        1,
                        control,
                        CardOffestData.GetOffsetXInDeck(true, context.GetCard.GameDeckNumber),
                        CardOffestData.GetOffsetYGameDeck(true, context.GetCard.IndexInDeck),
                        context.GetCard.ZIndex);
                }
                else if (!context.GetCard.ShowFront && newstatus == Model.CardStatus.Residue)//发牌后将剩余的纸牌移到准备区
                {
                    CardAnimation.TranslateCard(control, 11.5, 0.5);
                }
                else if (!context.GetCard.ShowFront && newstatus == Model.CardStatus.DealBack)//发牌显示背面
                {
                    CardAnimation.TranslateCard(
                        control,
                        CardOffestData.GetOffsetXInDeck(true, context.GetCard.GameDeckNumber),
                        CardOffestData.GetOffsetYGameDeck(true, context.GetCard.IndexInDeck),
                        -1);
                }
            }
            else if (context.GetCard.ShowFront && oldstatus == Model.CardStatus.DealBack && newstatus == Model.CardStatus.DealFront)//背面转正面
            {
                Image _image1 = control.Template.FindName("ImageCard", control) as Image;
                Image _image2 = control.Template.FindName("ImageFrontCard", control) as Image;
                TransformGroup transforms1 = _image1.RenderTransform as TransformGroup;
                TransformGroup transforms2 = _image2.RenderTransform as TransformGroup;

                CardAnimation.ReverseCard(
                    transforms1.Children[0] as ScaleTransform,
                    transforms2.Children[0] as ScaleTransform,
                    1);
            }
            else if (oldstatus == Model.CardStatus.Residue && context.GetCard.ShowFront && newstatus == Model.CardStatus.DealFront)//剩余的纸牌发牌显示正面
            {
                Image _image1 = control.Template.FindName("ImageCard", control) as Image;
                Image _image2 = control.Template.FindName("ImageFrontCard", control) as Image;
                TransformGroup transforms1 = _image1.RenderTransform as TransformGroup;
                TransformGroup transforms2 = _image2.RenderTransform as TransformGroup;

                CardAnimation.DealCard(
                    transforms1.Children[0] as ScaleTransform,
                    transforms2.Children[0] as ScaleTransform,
                    1,
                    control,
                    CardOffestData.GetOffsetXInDeck(true, context.GetCard.GameDeckNumber),
                    CardOffestData.GetOffsetYGameDeck(true, context.GetCard.IndexInDeck),
                    context.GetCard.IndexInDeck + 1);
            }
            else if (context.GetCard.ShowFront
                && (oldstatus == Model.CardStatus.MoveTarget
                     || oldstatus == Model.CardStatus.DealFront
                     || oldstatus == Model.CardStatus.Keeping
                     || oldstatus == Model.CardStatus.MoveBack)
                && (newstatus == Model.CardStatus.MoveBack
                     || newstatus == Model.CardStatus.MoveTarget
                     || newstatus == Model.CardStatus.Keeping))//移动纸牌（包括退回到原来位置和移动到目标处）
            {
                CardAnimation.TranslateCard(
                    control,
                    CardOffestData.GetOffsetXInDeck(true, context.GetCard.GameDeckNumber),
                    CardOffestData.GetOffsetYGameDeck(true, context.GetCard.IndexInDeck),
                    context.GetCard.IndexInDeck + 1);
            }
            else if(context.GetCard.ShowFront
                && (oldstatus == Model.CardStatus.MoveTarget
                     || oldstatus == Model.CardStatus.DealFront
                     || oldstatus == Model.CardStatus.Keeping
                     || oldstatus == Model.CardStatus.MoveBack)
                && newstatus == Model.CardStatus.Complete)
            {
                CardAnimation.TranslateCard(
                    control,
                    CardOffestData.GetOffsetXInDeck(false, context.GetCard.CompletedDeckNumber),
                    CardOffestData.GetOffsetYGameDeck(false, context.GetCard.IndexInDeck),
                    context.GetCard.IndexInDeck + 1);
            }
        }

        public Card GetCard
        {
            get
            {
                return DataContext as Card;
            }
        }

        public bool MouseCaptureElement()
        {
            return this.CaptureMouse();
        }

        public void ReleaseMouseCaptureElement()
        {
            this.ReleaseMouseCapture();
        }

        public IInputElement GetCanvasInputElement
        {
            get
            {
                return _canvas;
            }
        }

        public Size GetCanvasSize
        {
            get
            {
                return _canvas.RenderSize;
            }
        }

        public double GetCanvasLeft
        {
            get
            {
                return Canvas.GetLeft(this);
            }
        }

        public double GetCanvasTop
        {
            get
            {
                return Canvas.GetTop(this);
            }
        }

        public (double Width, double Height) GetCardSize
        {
            get
            {
                return (this.ActualWidth, this.ActualHeight);
            }
        }
    }
}
