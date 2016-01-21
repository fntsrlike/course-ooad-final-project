using System;
using System.Linq;
using System.Windows.Controls;
using UMLEditort.Args;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    public abstract class LineMode : BaseMode
    {
        protected LineMode(DiagramCanvas canvas) : base(canvas)
        {

        }

        public sealed override void MouseDown()
        {
            foreach (var baseObject in TheCanvas.Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(TheCanvas.StartPoint)))
            {
                TheCanvas.IsLineModeDragging = true;
                TheCanvas.StartObject = baseObject;
                break;
            }
        }

        public sealed override void MouseUp()
        {
            foreach (var baseObject in TheCanvas.Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(TheCanvas.EndPoint)))
            {
                TheCanvas.EndObject = baseObject;
                break;
            }

            if (TheCanvas.StartObject == null || TheCanvas.EndObject == null)
            {
                return;
            }

            var startPort = DiagramCanvas.GetNearestPort(TheCanvas.StartPoint, TheCanvas.StartObject);
            var endPort = DiagramCanvas.GetNearestPort(TheCanvas.EndPoint, TheCanvas.EndObject);
            var startArgs = new ConnectionArgs()
            {
                TargetObject = TheCanvas.StartObject,
                TargetPort = startPort
            };

            var endArgs = new ConnectionArgs()
            {
                TargetObject = TheCanvas.EndObject,
                TargetPort = endPort
            };

            var connectionLine = GetLine(startArgs, endArgs);

            TheCanvas.Children.Add(connectionLine);
            TheCanvas.IsLineModeDragging = false;
        }
        
        protected abstract ConnectionLine GetLine(ConnectionArgs startArgs, ConnectionArgs endArgs);
    }
}
