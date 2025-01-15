using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Input;

using SpiderSolitaire.Components;

namespace SpiderSolitaire.Behaviors
{
    public sealed class CardMouseStatusBehavior : Behavior<CardControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObjectMouseDown;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObjectMouseUp;
            AssociatedObject.MouseLeave += AssociatedObjectMouseLeave;
            AssociatedObject.MouseEnter += AssociatedObjectMouseEnter;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObjectMouseDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= AssociatedObjectMouseUp;
            AssociatedObject.MouseEnter -= AssociatedObjectMouseEnter;
            AssociatedObject.MouseLeave -= AssociatedObjectMouseLeave;
        }

        private void AssociatedObjectMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AssociatedObject.Template.FindName("MouseHoverHighLight", AssociatedObject) is FrameworkElement element)
            {
                var input = Mouse.DirectlyOver;
                if (input != null
                    && (AssociatedObject.CardStatus == Model.CardStatus.DealFront || AssociatedObject.CardStatus == Model.CardStatus.MoveBack
                    || AssociatedObject.CardStatus == Model.CardStatus.MoveTarget || AssociatedObject.CardStatus == Model.CardStatus.Keeping))
                {
                    element.Visibility = Visibility.Visible;
                }
            }
        }

        private void AssociatedObjectMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AssociatedObject.Template.FindName("MouseHoverHighLight", AssociatedObject) is FrameworkElement element)
            {
                if (AssociatedObject.CardStatus == Model.CardStatus.DealFront || AssociatedObject.CardStatus == Model.CardStatus.MoveBack
                    || AssociatedObject.CardStatus == Model.CardStatus.MoveTarget || AssociatedObject.CardStatus == Model.CardStatus.Keeping)
                {
                    element.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AssociatedObjectMouseEnter(object sender, MouseEventArgs e)
        {
            if (AssociatedObject.Template.FindName("MouseHoverHighLight", AssociatedObject) is FrameworkElement element)
            {
                if ((AssociatedObject.DataContext as Core.PlayingCard.Card).IsSelected == false
                    && (AssociatedObject.CardStatus == Model.CardStatus.DealFront || AssociatedObject.CardStatus == Model.CardStatus.MoveBack
                    || AssociatedObject.CardStatus == Model.CardStatus.MoveTarget || AssociatedObject.CardStatus == Model.CardStatus.Keeping))
                {
                    element.Visibility = Visibility.Visible;
                }
            }
        }

        private void AssociatedObjectMouseLeave(object sender, MouseEventArgs e)
        {
            if (AssociatedObject.Template.FindName("MouseHoverHighLight", AssociatedObject) is FrameworkElement element)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }
    }
}
