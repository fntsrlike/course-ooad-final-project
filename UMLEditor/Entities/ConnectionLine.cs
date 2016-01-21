using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    public abstract class ConnectionLine : UserControl
    {
        protected ConnectionLine(ConnectionArgs startConnectionArgs, ConnectionArgs endConnectionArgs)
        {
            StartConnectionArgs = startConnectionArgs;
            EndConnectionArgs = endConnectionArgs;
        }

        protected Line TheArrowLine;
        protected Canvas TheArrowCanvas;
        
        public void Update()
        {
            StartPort = StartConnectionArgs.TargetPoint;
            EndPort = EndConnectionArgs.TargetPoint;
            Draw();
        }

        protected void Draw()
        {
            TheArrowLine.X1 = ArrowEndpointHeight;
            TheArrowLine.X2 = LineLenght;
            TheArrowCanvas.RenderTransform = new RotateTransform(Angle);
            ResetLocation();
        }

        private void ResetLocation()
        {
            var angle = Angle % 360 + 360;
            var d = (long)angle / 90;

            var xOffSet = 0.0;
            var yOffSet = 0.0;

            d = d % 4;
            var e = Math.Abs(angle % 90) / 90;

            switch (d)
            {
                case 0:
                    xOffSet = e;
                    yOffSet = e - 1;
                    break;

                case 1:
                    xOffSet = 1 - e;
                    yOffSet = e;
                    break;

                case 2:
                    xOffSet = -e;
                    yOffSet = 1 - e;
                    break;

                case 3:
                    xOffSet = e - 1;
                    yOffSet = -e;
                    break;
            }

            Canvas.SetLeft(this, StartConnectionArgs.TargetPoint.X + xOffSet * 15);
            Canvas.SetTop(this, StartConnectionArgs.TargetPoint.Y + yOffSet * 15);
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