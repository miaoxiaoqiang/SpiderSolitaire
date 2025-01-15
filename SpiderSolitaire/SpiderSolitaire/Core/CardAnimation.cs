using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 纸牌动画处理
    /// </summary>
    internal sealed class CardAnimation
    {
        private static readonly Stack<DoubleAnimation> scalebackAnimations;
        private static readonly Stack<DoubleAnimation> scalefrontAnimations;

        private static readonly Stack<Int32AnimationUsingKeyFrames> zIndexAnimations;
        private static readonly Stack<DoubleAnimation> translateXAnimations;
        private static readonly Stack<DoubleAnimation> translateYAnimations;

        static CardAnimation()
        {
            translateXAnimations = new Stack<DoubleAnimation>();
            translateYAnimations = new Stack<DoubleAnimation>();
            zIndexAnimations = new Stack<Int32AnimationUsingKeyFrames>();

            scalebackAnimations = new Stack<DoubleAnimation>();
            scalefrontAnimations = new Stack<DoubleAnimation>();
            foreach (var item in Enumerable.Range(0, 30))
            {
                scalebackAnimations.Push(new DoubleAnimation
                {
                    FillBehavior = FillBehavior.Stop
                });
                scalefrontAnimations.Push(new DoubleAnimation
                {
                    FillBehavior = FillBehavior.Stop
                });
            }
            foreach (var item in Enumerable.Range(0, 50))
            {
                translateXAnimations.Push(new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    FillBehavior = FillBehavior.Stop
                });
                translateYAnimations.Push(new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    FillBehavior = FillBehavior.Stop
                });
                zIndexAnimations.Push(new Int32AnimationUsingKeyFrames
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    FillBehavior = FillBehavior.Stop
                });
            }
        }

        public static void DealCard(ScaleTransform scaleback, ScaleTransform scalefront, double toScale, UIElement parentUI, double toLeft, double toTop, int toZIndex = 0)
        {
            ReverseCard(scaleback, scalefront, toScale);
            TranslateCard(parentUI, toLeft, toTop);
        }

        /// <summary>
        /// 纸牌翻转动画
        /// </summary>
        /// <param name="scaleback">背面在二维 x-y 坐标系统内缩放对象</param>
        /// <param name="scalefront">正面在二维 x-y 坐标系统内缩放对象</param>
        /// <param name="toScale">缩放比例</param>
        public static void ReverseCard(ScaleTransform scaleback, ScaleTransform scalefront, double toScale)
        {
            DoubleAnimation _scalebackAnimation;
            DoubleAnimation _scalefrontAnimation;

            void ScaleBackCompletedEvent(object obj, EventArgs e)
            {
                _scalebackAnimation.Completed -= ScaleBackCompletedEvent;
                scalebackAnimations.Push(_scalebackAnimation);
                _scalebackAnimation.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                scaleback.ScaleX = Math.Abs(1 - toScale);
            }

            _scalebackAnimation = scalebackAnimations.Pop();
            _scalebackAnimation.BeginTime = TimeSpan.FromMilliseconds(0);
            _scalebackAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            _scalebackAnimation.To = Math.Abs(1 - toScale);
            _scalebackAnimation.Completed += ScaleBackCompletedEvent;
            scaleback.BeginAnimation(ScaleTransform.ScaleXProperty, _scalebackAnimation, HandoffBehavior.SnapshotAndReplace);

            void ScaleFrontCompletedEvent(object obj, EventArgs e)
            {
                _scalefrontAnimation.Completed -= ScaleFrontCompletedEvent;
                scalebackAnimations.Push(_scalefrontAnimation);
                _scalefrontAnimation.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                scalefront.ScaleX = toScale;
            }

            _scalefrontAnimation = scalebackAnimations.Pop();
            _scalefrontAnimation.BeginTime = TimeSpan.FromMilliseconds(150);
            _scalefrontAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            _scalefrontAnimation.To = toScale;
            _scalefrontAnimation.Completed += ScaleFrontCompletedEvent;
            scalefront.BeginAnimation(ScaleTransform.ScaleXProperty, _scalefrontAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// 纸牌平移动画
        /// </summary>
        public static void TranslateCard(UIElement dob, double toLeft, double toTop, int toZIndex = -1)
        {
            DoubleAnimation _translateXAnimation;
            DoubleAnimation _translateYAnimation;
            Int32AnimationUsingKeyFrames _zIndexAnimation = null;

            void TranslateXCompletedEvent(object obj, EventArgs e)
            {
                _translateXAnimation.Completed -= TranslateXCompletedEvent;
                translateXAnimations.Push(_translateXAnimation);
                dob.BeginAnimation(Canvas.LeftProperty, null);
                Canvas.SetLeft(dob, toLeft);
            }

            void TranslateYCompletedEvent(object obj, EventArgs e)
            {
                _translateYAnimation.Completed -= TranslateYCompletedEvent;
                translateYAnimations.Push(_translateYAnimation);
                dob.BeginAnimation(Canvas.TopProperty, null);
                Canvas.SetTop(dob, toTop);
            }

            void ZIndexCompletedEvent(object obj, EventArgs e)
            {
                _zIndexAnimation.KeyFrames.Clear();
                _zIndexAnimation.Completed -= ZIndexCompletedEvent;
                zIndexAnimations.Push(_zIndexAnimation);
                dob.BeginAnimation(Canvas.ZIndexProperty, null);
                Canvas.SetZIndex(dob, toZIndex);
            }

            _translateXAnimation = translateXAnimations.Pop();
            _translateXAnimation.Completed += TranslateXCompletedEvent;
            _translateXAnimation.To = toLeft;

            _translateYAnimation = translateYAnimations.Pop();
            _translateYAnimation.Completed += TranslateYCompletedEvent;
            _translateYAnimation.To = toTop;

            if(toZIndex > 0)
            {
                _zIndexAnimation = zIndexAnimations.Pop();
                _zIndexAnimation.KeyFrames.Add(new DiscreteInt32KeyFrame()
                {
                    KeyTime = TimeSpan.FromMilliseconds(300),
                    Value = toZIndex
                });
                _zIndexAnimation.Completed += ZIndexCompletedEvent;
            }
            

            dob.BeginAnimation(Canvas.LeftProperty, _translateXAnimation, HandoffBehavior.SnapshotAndReplace);
            dob.BeginAnimation(Canvas.TopProperty, _translateYAnimation, HandoffBehavior.SnapshotAndReplace);

            if(toZIndex > 0)
            {
                dob.BeginAnimation(Canvas.ZIndexProperty, _zIndexAnimation, HandoffBehavior.SnapshotAndReplace);
            }
        }
    }
}
