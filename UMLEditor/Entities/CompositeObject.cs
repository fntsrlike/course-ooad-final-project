using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    public class CompositeObject : ISelectableObject
    {
        private bool _selected;

        public CompositeObject(List<BaseObject> baseObjectWithoutGroup, List<CompositeObject> compositeObjects)
        {
            BaseObjectMembers = baseObjectWithoutGroup;
            CompositeObjectMembers = compositeObjects;
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

        public List<ISelectableObject> Members
        {
            get
            {
                var members = new List<ISelectableObject>();
                members.AddRange(BaseObjectMembers);
                members.AddRange(CompositeObjectMembers);
                return members;
            }
        } 
        public List<BaseObject> BaseObjectMembers { get; }
        public List<CompositeObject> CompositeObjectMembers { get; }

        public List<BaseObject> GetAllBaseObjects()
        {
            var list = BaseObjectMembers.ToList();

            foreach (var subList in CompositeObjectMembers.Select(compositeObject => compositeObject.GetAllBaseObjects()))
            {
                list.AddRange(subList);
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

        public void ClearComposite()
        {
            foreach (var member in Members)
            {
                member.Compositer = null;
            }
        }
    }
}
