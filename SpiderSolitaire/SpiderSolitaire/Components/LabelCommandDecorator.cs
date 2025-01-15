using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpiderSolitaire.Components
{
    public sealed class LabelCommandDecorator : Decorator
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(LabelCommandDecorator), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public LabelCommandDecorator()
        {
            PreviewMouseLeftButtonUp += (s, e) =>
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };
        }
    }
}
