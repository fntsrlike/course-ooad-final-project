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

        protected int ArrowEndpointHeight;

        internal ConnectionArgs StartConnectionArgs { get; set; }
        internal ConnectionArgs EndConnectionArgs { get; set; }
        public Point StartPort { get; }
        public Point EndPort { get; }


        protected double XDiff => EndPort.X - StartPort.X;
        protected double YDiff => EndPort.Y - StartPort.Y;
        protected double Angle => Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI;
        protected double LineLenght => Math.Sqrt(XDiff * XDiff + YDiff * YDiff);
    }
}