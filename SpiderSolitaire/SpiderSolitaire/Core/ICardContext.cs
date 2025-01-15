using System.Windows;

using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 定义纸牌的上下文
    /// </summary>
    public interface ICardContext
    {
        /// <summary>
        /// 获取元素参与数据绑定时的数据上下文
        /// </summary>
        /// <returns></returns>
        public Card GetCard
        {
            get;
        }

        /// <summary>
        /// 尝试将鼠标强制捕获到此元素
        /// </summary>
        /// <remarks>
        /// 捕获的元素类型应为 <seealso cref="SpiderSolitaire.Components.CardControl"/> 卡牌控件
        /// </remarks>
        /// <returns>
        /// 如果成功捕获了鼠标，则为 <see langword="true"/>；否则为 <see langword="false"/>
        /// </returns>
        public bool MouseCaptureElement();

        /// <summary>
        /// 如果此元素具有鼠标捕获，则释放该捕获
        /// </summary>
        public void ReleaseMouseCaptureElement();

        /// <summary>
        /// 获取承载纸牌的父元素容器对象
        /// </summary>
        public IInputElement GetCanvasInputElement
        {
            get;
        }

        /// <summary>
        /// 获取纸牌在画布的X偏移量
        /// </summary>
        public double GetCanvasLeft
        {
            get;
        }

        /// <summary>
        /// 获取纸牌在画布的Y偏移量
        /// </summary>
        public double GetCanvasTop
        {
            get;
        }

        /// <summary>
        /// 获取卡牌控件的实际呈现的高度和宽度
        /// </summary>
        public (double Width, double Height) GetCardSize
        {
            get;
        }

        /// <summary>
        /// 获取承载纸牌的画布控件呈现的实际大小
        /// </summary>
        public Size GetCanvasSize
        {
            get;
        }
    }
}
