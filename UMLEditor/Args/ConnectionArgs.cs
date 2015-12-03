using System;
using System.Diagnostics;
using System.Windows;
using UMLEditort.Entities;

namespace UMLEditort.Args
{
    public class ConnectionArgs
    {
        public ConnectionArgs()
        {
            TargetPort = Ports.Undefined;
        }

        /// <summary>
        /// 基本物件
        /// </summary>
        public BaseObject TargetObject
        {
            get;
            set;
        }

        /// <summary>
        /// 對應的 Port
        /// </summary>
        public Ports TargetPort
        {
            get;
            set;
        }

        /// <summary>
        /// 根據
        /// </summary>
        public Point TargetPoint
        {
            get
            {   
                Debug.Assert(TargetObject != null && TargetPort != Ports.Undefined);

                // ReSharper disable once SwitchStatementMissingSomeCases
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
