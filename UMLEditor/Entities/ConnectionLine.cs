using System;
using System.Windows;
using System.Windows.Controls;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    public class ConnectionLine : UserControl
    {
        protected ConnectionLine(ConnectionArgs startConnectionArgs, ConnectionArgs endConnectionArgs)
        {
            StartConnectionArgs = startConnectionArgs;
            EndConnectionArgs = endConnectionArgs;
            StartPort = StartConnectionArgs.TargetPoint;
            EndPort = EndConnectionArgs.TargetPoint;
        }

        /// <summary>
        /// 箭頭三角形的高
        /// </summary>
        protected int ArrowEndpointHeight;

        /// <summary>
        /// 起始連接端
        /// </summary>
        internal ConnectionArgs StartConnectionArgs { get; set; }

        /// <summary>
        /// 終點連接端
        /// </summary>
        internal ConnectionArgs EndConnectionArgs { get; set; }

        /// <summary>
        /// 起始連接端的連接阜對應的點
        /// </summary>
        public Point StartPort { get; }

        /// <summary>
        /// 終點連接端的連接阜對應的點
        /// </summary>
        public Point EndPort { get; }

        /// <summary>
        /// StartPort 和 EndPort 兩點間 X 座標的偏移量
        /// </summary>
        protected double XDiff => EndPort.X - StartPort.X;

        /// <summary>
        /// StartPort 和 EndPort 兩點間 Y 座標的偏移量
        /// </summary>
        protected double YDiff => EndPort.Y - StartPort.Y;

        /// <summary>
        /// 整個線應該選轉的角度
        /// </summary>
        protected double Angle => Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI;

        /// <summary>
        /// 整個線的長度的長度，等於StartPort 和 EndPort 兩點間的位移
        /// </summary>
        protected double LineLenght => Math.Sqrt(XDiff * XDiff + YDiff * YDiff);
    }
}