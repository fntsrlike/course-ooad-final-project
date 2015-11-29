using System.Windows;

namespace UMLEditort.Entities
{
    public interface ISelectableObject
    {
        bool Selected
        {
            get;
            set;
        }

        CompositeObject Compositer
        {
            get;
            set;
        }

        CompositeObject GetOutermostCompositer();
    }
}
