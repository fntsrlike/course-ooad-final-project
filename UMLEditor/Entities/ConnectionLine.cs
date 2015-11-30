using System;
using System.Windows;
using System.Windows.Controls;

namespace UMLEditort.Entities
{
    public class ConnectionLine : UserControl
    {
        protected ConnectionLine(Point startPort, Point endPort)
        {
            StartPort = startPort;
            EndPort = endPort;
        }

        protected int ArrowEndpointHeight;

        public Point StartPort { get; }
        public Point EndPort { get; }


        protected double XDiff => EndPort.X - StartPort.X;
        protected double YDiff => EndPort.Y - StartPort.Y;
        protected double Angle => Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI;
        protected double LineLenght => Math.Sqrt(XDiff * XDiff + YDiff * YDiff);
    }
}