using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpiderSolitaire
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel.MainViewModel();

            Loaded += WindowLoaded;

            gamehelp.AddHandler(UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(RichTextboxPreviewMouseWheel), true);
            OptionsSelect.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OptionClick), true);
            LanguageSelect.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(LanguageClick), true);
            Record.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(ShowRecord), true);
            About.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(ShowAbout), true);
            CloseButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(HideInfo), true);

            PreviewKeyDown += (s, e) =>
            {
                if(DataContext is ViewModel.MainViewModel viewModel)
                {
                    if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        ExtraBorder.Visibility = Visibility.Collapsed;
                        gamehelp.Visibility = Visibility.Collapsed;
                        infoback.Visibility = Visibility.Collapsed;
                        infofront.Visibility = Visibility.Collapsed;
                    }
                    else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                    {
                        viewModel.NewGameCommand.Execute(null);
                    }
                    else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.H))
                    {
                        ShowAbout(null, null);
                    }
                    else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D) && Keyboard.IsKeyDown(Key.I))
                    {
                        //ShowRecord(null, null);
                    }
                }
            };
        }

        private void RichTextboxPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = Utils.Helper.FindVisualChild<ScrollViewer>(sender as DependencyObject);
            if (e.Delta > 0)
            {
                scrollviewer.LineUp();
                scrollviewer.LineUp();
            }
            else
            {
                scrollviewer.LineDown();
                scrollviewer.LineDown();
            }

            e.Handled = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void OptionClick(object obj, MouseButtonEventArgs e)
        {
            menuPop1.IsOpen = true;
        }

        private void LanguageClick(object obj, MouseButtonEventArgs e)
        {
            menuPop2.IsOpen = true;
        }

        private void HideInfo(object sender, RoutedEventArgs e)
        {
            ExtraBorder.Visibility = Visibility.Collapsed;
            infofront.Visibility = Visibility.Collapsed;
            infoback.Visibility = Visibility.Collapsed;
        }

        private void ShowRecord(object obj, MouseButtonEventArgs e)
        {
            //if (ExtraBorder.Visibility == Visibility.Collapsed)
            //{
            //    ExtraBorder.Visibility = Visibility.Visible;
            //    infoback.Visibility = Visibility.Visible;
            //    infofront.Visibility = Visibility.Visible;
            //    gamehelp.Visibility = Visibility.Collapsed;
            //}
        }

        private void ShowAbout(object obj, MouseButtonEventArgs e)
        {
            if (ExtraBorder.Visibility == Visibility.Collapsed)
            {
                ExtraBorder.Visibility = Visibility.Visible;
                gamehelp.Visibility = Visibility.Visible;
                infoback.Visibility = Visibility.Collapsed;
                infofront.Visibility = Visibility.Collapsed;
            }
        }

        private void InlineUIContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://github.com/miaoxiaoqiang/SpiderSolitaire.git")).Dispose();
        }
    }
}
