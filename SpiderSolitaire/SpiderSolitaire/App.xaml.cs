using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpiderSolitaire
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public sealed partial class App : Application
    {
        public App()
        {
            MvvmLight.Threading.DispatcherHelper.Initialize();
        }
    }
}
