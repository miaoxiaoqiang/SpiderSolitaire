using System.Windows.Controls;
using System.Windows;

namespace SpiderSolitaire.Components
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CardControl))]
    public sealed class CardItemsControl : ItemsControl
    {
        //static CardItemsControl()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(CardItemsControl), new FrameworkPropertyMetadata(typeof(CardItemsControl)));
        //}

        public CardItemsControl() : base()
        {
            
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CardControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CardControl;
        }
    }
}
