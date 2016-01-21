using UMLEditort.Args;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class CompositionLineMode : LineMode
    {
        public CompositionLineMode(DiagramCanvas canvas) : base(canvas)
        {
        }

        protected override ConnectionLine GetLine(ConnectionArgs startArgs, ConnectionArgs endArgs)
        {
            return new CompositionLine(startArgs, endArgs);
        }
    }
}
