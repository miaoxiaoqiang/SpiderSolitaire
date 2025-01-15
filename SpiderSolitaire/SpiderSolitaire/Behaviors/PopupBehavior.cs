using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace SpiderSolitaire.Behaviors
{
    /// <summary>
    /// 当点击 <see cref="RadioButton"/> 内部对应的 <see cref="TextBlock"/> 元素时，关闭 <see cref="Popup"/> 窗口
    /// </summary>
    public sealed class PopupBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += PopupMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= PopupMouseUp;
        }

        private void PopupMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var radio = Utils.Helper.FindVisualImmediateParent<RadioButton>(AssociatedObject);
            var popuo = Utils.Helper.GetLogicalParent<Popup>(AssociatedObject);
            radio.IsChecked = true;
            popuo.IsOpen = false;
        }
    }
}
