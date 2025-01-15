namespace SpiderSolitaire.Model
{
    /// <summary>
    /// 纸牌操作状态
    /// </summary>
    public enum CardStatus
    {
        /// <summary>
        /// 纸牌已初始化并准备发牌
        /// </summary>
        Initial,
        /// <summary>
        /// 待分发的剩余纸牌
        /// </summary>
        Residue,
        /// <summary>
        /// 显示背面
        /// </summary>
        DealBack,
        /// <summary>
        /// 显示正面
        /// </summary>
        DealFront,
        /// <summary>
        /// 继续退回或继续移动。即保持原样
        /// </summary>
        /// <remarks>
        /// 此枚举具有与枚举 <seealso cref="MoveBack"/> 和枚举 <seealso cref="MoveTarget"/> 的相同状态意思。<para/>
        /// </remarks>
        Keeping,
        /// <summary>
        /// 纸牌退回到原来的位置
        /// </summary>
        MoveBack,
        /// <summary>
        /// 移动纸牌移动到目标处
        /// </summary>
        MoveTarget,
        /// <summary>
        /// 撤销（尚未开发）
        /// </summary>
        Revoke,
        /// <summary>
        /// 在游戏区完成一组卡牌序列并发送到已完成区
        /// </summary>
        Complete
    }
}
