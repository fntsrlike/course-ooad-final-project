using UMLEditort.Args;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class AssociationLineMode : LineMode
    {
        public AssociationLineMode(DiagramCanvas canvas) : base(canvas)
        {
        }

        protected override ConnectionLine GetLine(ConnectionArgs startArgs, ConnectionArgs endArgs)
        {
            return new AssociationLine(startArgs, endArgs);
        }
    }
}
