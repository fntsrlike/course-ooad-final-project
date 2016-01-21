using System;
using System.Windows;
using System.Windows.Controls;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    public abstract class ConnectionLine : UserControl
    {
        protected ConnectionLine(ConnectionArgs startConnectionArgs, ConnectionArgs endConnectionArgs)
        {
            StartConnectionArgs = startConnectionArgs;
            EndConnectionArgs = endConnectionArgs;
            StartPort = StartConnectionArgs.TargetPoint;
            EndPort = EndConnectionArgs.TargetPoint;
            Update();
        }

        public void Update()
        {
            Draw();
        }

        private void Draw()
        {

            var angle = Angle % 360 + 360;
            var d = (long)angle / 90;

            var xDiff = 0.0;
            var yDiff = 0.0;

            d = d % 4;
            var e = Math.Abs(angle % 90) / 90;

            switch (d)
            {
                case 0:
                    xDiff = e;
                    yDiff = e - 1;
                    break;

                case 1:
                    xDiff = 1 - e;
                    yDiff = e;
                    break;

                case 2:
                    xDiff = -e;
                    yDiff = 1 - e;
                    break;

                case 3:
                    xDiff = e - 1;
                    yDiff = -e;
                    break;
            }

            Canvas.SetLeft(this, StartConnectionArgs.TargetPoint.X + xDiff * 15);
            Canvas.SetTop(this, StartConnectionArgs.TargetPoint.Y + yDiff * 15);
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
        public Point StartPort { get; private set; }

        /// <summary>
        /// 終點連接端的連接阜對應的點
        /// </summary>
        public Point EndPort { get; private set; }

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
        public double Angle => Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI;

        /// <summary>
        /// 整個線的長度的長度，等於StartPort 和 EndPort 兩點間的位移
        /// </summary>
        protected double LineLenght => Math.Sqrt(XDiff * XDiff + YDiff * YDiff);
    }
}