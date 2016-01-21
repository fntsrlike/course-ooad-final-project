using System.Linq;
using UMLEditort.Args;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    /// <summary>
    /// 作為關係線模式的基礎，需要時做不同的關係線就只需繼承這個模式然後覆寫 GetLine() 方法即可
    /// </summary>
    public abstract class LineMode : BaseMode
    {
        protected LineMode(DiagramCanvas canvas) : base(canvas)
        {
        }

        public sealed override void MouseDown()
        {
            foreach (var baseObject in TheCanvas.ExistBaseObjects.Select(child => child).Where(baseObject => baseObject.IsContainPoint(TheCanvas.StartPoint)))
            {
                TheCanvas.IsLineModeDragging = true;
                TheCanvas.StartObject = baseObject;
                break;
            }
        }

        public sealed override void MouseUp()
        {
            foreach (var baseObject in TheCanvas.ExistBaseObjects.Select(child => child).Where(baseObject => baseObject.IsContainPoint(TheCanvas.EndPoint)))
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

            TheCanvas.ExistLines.Add(connectionLine);
            TheCanvas.IsLineModeDragging = false;
        }
        
        protected abstract ConnectionLine GetLine(ConnectionArgs startArgs, ConnectionArgs endArgs);
    }
}
