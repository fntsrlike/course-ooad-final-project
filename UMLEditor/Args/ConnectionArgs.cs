using System;
using System.Windows;
using UMLEditort.Entities;

namespace UMLEditort.Args
{
    public class ConnectionArgs
    {
        public IBaseObject TargetObject
        {
            get;
            set;
        }

        public Ports TargetPort
        {
            get;
            set;
        }

        public Point TargetPoint
        {
            get
            {
                switch (TargetPort)
                {
                    case Ports.Top:
                        return TargetObject.TopPoint;

                    case Ports.Right:
                        return TargetObject.RightPoint;

                    case Ports.Bottom:
                        return TargetObject.BottomPoint;

                    case Ports.Left:
                        return TargetObject.LeftPoint;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
