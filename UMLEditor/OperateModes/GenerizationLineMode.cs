using System.Windows.Controls;
using UMLEditort.Args;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class GenerizationLineMode : LineMode
    {
        public GenerizationLineMode(DiagramCanvas canvas) : base(canvas)
        {
            
        }

        protected override ConnectionLine GetLine(ConnectionArgs startArgs, ConnectionArgs endArgs)
        {
            return new GeneralizationLine(startArgs, endArgs);
        }
    }
}
