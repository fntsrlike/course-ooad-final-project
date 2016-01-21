using System.Collections;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    /// <summary>
    /// 幫忙建立各種模式的物件，並且幫忙 Keep 住，而不需要重複建立
    /// </summary>
    class ModesFactory
    {
        private readonly Hashtable _modes; 

        public ModesFactory(DiagramCanvas theCanvas)
        {
            _modes = new Hashtable
            {
                {Modes.Select, new SelectMode(theCanvas)},
                {Modes.Associate, new AssociationLineMode(theCanvas)},
                {Modes.Generalize, new GenerizationLineMode(theCanvas)},
                {Modes.Composition, new CompositionLineMode(theCanvas)},
                {Modes.Class, new ClassMode(theCanvas)},
                {Modes.UseCase, new UseCaseMode(theCanvas)}
            };
        }

        public BaseMode GetMode(Modes mode)
        {
            return (BaseMode)_modes[mode];
        }
    }
}
