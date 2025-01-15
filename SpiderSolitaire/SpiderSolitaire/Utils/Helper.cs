using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SpiderSolitaire.Utils
{
    internal sealed class Helper
    {
        private static readonly RNGCryptoServiceProvider rngCSP;
        private static readonly RijndaelManaged rijndael;

        static Helper()
        {
            rngCSP = new RNGCryptoServiceProvider();
            rijndael = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = new byte[]
                {
                    0x56, 0x53, 0x7A, 0x71, 0x45, 0x3D, 0x54, 0x7C, 0x3F, 0x47, 0x61, 0x74, 0x37, 0x31, 0x2A, 0x55,
                    0x5B, 0x3B, 0x40, 0x4F, 0x51, 0x5F, 0x70, 0x47, 0x35, 0x5B, 0x5F, 0x22, 0x70, 0x5D, 0x75, 0x64
                },
                IV = new byte[]
                {
                    0x44, 0x72, 0x37, 0x68, 0x54, 0x6B, 0x5F, 0x40, 0x27, 0x5B, 0x33, 0x65, 0x36, 0x76, 0x49, 0x66
                }
            };
        }

        public byte[] Encrypt(byte[] rawData)
        {
            using (ICryptoTransform cTransform = rijndael.CreateEncryptor())
            {
                return cTransform.TransformFinalBlock(rawData, 0, rawData.Length);
            }
        }

        public byte[] Decrypt(byte[] rawData)
        {
            using (ICryptoTransform cTransform = rijndael.CreateDecryptor())
            {
                return cTransform.TransformFinalBlock(rawData, 0, rawData.Length);
            }
        }

        /// <summary>
        /// 在指定的连续范围生成随机数
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>
        /// <paramref name="minValue"/> &lt;= 随机数值 &lt; <paramref name="maxValue"/>
        /// </returns>
        public static int GenerateRandom(int minValue, int maxValue)
        {
            int m = maxValue - minValue;
            decimal _base = long.MaxValue;
            byte[] rndSeries = new byte[8];
            rngCSP.GetBytes(rndSeries);
            long l = BitConverter.ToInt64(rndSeries, 0);
            int rnd = (int)(Math.Abs(l) / _base * m);
            return minValue + rnd;
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> list = new();

            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }

                    List<T> childItems = FindVisualChildren<T>(child);
                    if (childItems != null && childItems.Count() > 0)
                    {
                        foreach (var item in childItems)
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        public static List<T> FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new();
                DependencyObject parent = VisualTreeHelper.GetParent(obj);
                if (parent != null && parent is T)
                {
                    TList.Add((T)parent);
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                else if (parent != null)
                {
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                return TList;
            }
            catch
            {
                return null;
            }
        }

        public static T FindVisualImmediateParent<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static T FindAncestor<T>(DependencyObject dObj) where T : DependencyObject
        {
            var uiElement = dObj;
            while (uiElement != null)
            {
                uiElement = uiElement is Visual || uiElement is Visual3D ? VisualTreeHelper.GetParent(uiElement) : LogicalTreeHelper.GetParent(uiElement);

                if (uiElement is T)
                {
                    return (T)uiElement;
                }
            }
            return null;
        }

        public static T GetLogicalParent<T>(DependencyObject p_oElement) where T : DependencyObject
        {
            DependencyObject oParent = p_oElement;
            Type oTargetType = typeof(T);
            do
            {
                var parentdo = LogicalTreeHelper.GetParent(oParent);
                oParent = parentdo != null ? parentdo : VisualTreeHelper.GetParent(oParent);// LogicalTreeHelper.GetParent(oParent);
            }
            while
            (
                !(
                    oParent == null
                    || oParent.GetType() == oTargetType
                    || oParent.GetType().IsSubclassOf(oTargetType)
                )
            );

            return oParent as T;
        }

        public static T FindVisualParent<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T && (string.IsNullOrEmpty(name) || ((T)parent).Name == name))
                {
                    return (T)parent;
                }

                // 在上一级父控件中没有找到指定名字的控件，就再往上一级找
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
