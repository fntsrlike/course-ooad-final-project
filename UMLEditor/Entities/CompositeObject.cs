using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    public class CompositeObject : ISelectableObject
    {
        private bool _selected;

        public CompositeObject(List<ISelectableObject> members)
        {
            Members = new List<ISelectableObject>();
            Members.AddRange(members);
        }
        
        public bool Selected
        {
            get
            {
                return _selected;
                
            }
            set
            {
                foreach (var member in Members)
                {
                    member.Selected = value;
                }
                _selected = value;
            }
        }
        public CompositeObject Compositer { get; set; }

        public List<ISelectableObject> Members { get; }

        public List<BaseObject> GetAllBaseObjects()
        {
            var list = new List<BaseObject>();

            foreach (var member in Members)
            {
                if (member is CompositeObject)
                {
                    var compositeObject = member as CompositeObject;
                    var subList = compositeObject.GetAllBaseObjects();

                    list.AddRange(subList);
                }
                else if (member is BaseObject)
                {
                    list.Add( (BaseObject) member);
                }
            }
            return list;
        }

        public CompositeObject GetOutermostCompositer()
        {
            var compositer = this;
            while (compositer.Compositer != null)
            {
                compositer = compositer.Compositer;
            }
            return compositer;
        }
    }
}
