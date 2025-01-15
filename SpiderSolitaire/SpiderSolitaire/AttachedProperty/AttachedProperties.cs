using System;
using System.Reflection;
using System.Windows;

namespace SpiderSolitaire.AttachedProperty
{
    /// <summary>
    /// 表示自定义附加属性
    /// </summary>
    public sealed class AttachedProperties : DependencyObject
    {
        static AttachedProperties()
        {

        }

        public static readonly DependencyProperty ScaleHeightProperty =
            DependencyProperty.RegisterAttached("ScaleHeight", typeof(double), typeof(AttachedProperties), new PropertyMetadata(0D));

        public static readonly DependencyProperty ImageResourceKeyProperty =
            DependencyProperty.RegisterAttached("ImageResourceKey", typeof(string), typeof(AttachedProperties), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CardStatusProperty =
            DependencyProperty.RegisterAttached("CardStatus", typeof(Model.CardStatus), typeof(AttachedProperties), new PropertyMetadata(Model.CardStatus.Initial));

        public static double GetScaleHeight(DependencyObject obj)
        {
            return System.Convert.ToDouble(obj.GetValue(ScaleHeightProperty));
        }

        public static void SetScaleHeight(DependencyObject obj, double value)
        {
            obj.SetValue(ScaleHeightProperty, value);
        }

        public static string GetImageResourceKey(DependencyObject obj)
        {
            return (string)obj.GetValue(ImageResourceKeyProperty);
        }

        public static void SetImageResourceKey(DependencyObject obj, string value)
        {
            obj.SetValue(ImageResourceKeyProperty, value);
        }

        public static Model.CardStatus GetCardStatus(DependencyObject obj)
        {
            return (Model.CardStatus)obj.GetValue(CardStatusProperty);
        }

        public static void SetCardStatus(DependencyObject obj, Model.CardStatus value)
        {
            obj.SetValue(CardStatusProperty, value);
        }
    }
}
