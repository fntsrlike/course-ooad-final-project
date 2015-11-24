using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    internal abstract class ConnetionLine
    {
        protected ConnetionLine(IBaseObject from, IBaseObject to)
        {
            From = from;
            To = to;
        }

        public IBaseObject From { get; }
        public IBaseObject To { get; }

        public abstract Line GetLine();
    }
}