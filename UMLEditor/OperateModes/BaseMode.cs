using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    public abstract class BaseMode
    {
        protected DiagramCanvas TheCanvas;

        public BaseMode(DiagramCanvas canvas)
        {
            TheCanvas = canvas;
        }

        public abstract void MouseDown();
        public abstract void MouseUp();
    }
}
