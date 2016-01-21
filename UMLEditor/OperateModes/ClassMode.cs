using System.Windows.Controls;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class ClassMode : BaseMode
    {
        public ClassMode(DiagramCanvas canvas) : base(canvas)
        {
            
        }              

        public override void MouseDown()
        {
            TheCanvas.CleanSelectedObjects();

            var baseObject = new ClassObject($"#{TheCanvas.ObjectCounter} Class Object")
            {
                Width = 150,
                Height = 100
            };

            Canvas.SetLeft(baseObject, TheCanvas.StartPoint.X);
            Canvas.SetTop(baseObject, TheCanvas.StartPoint.Y);

            baseObject.StartPoint = TheCanvas.StartPoint;
            TheCanvas.Children.Add(baseObject);
            TheCanvas.SelectedObject = baseObject;
            TheCanvas.SelectedRelativeObjects.Add(baseObject);
            TheCanvas.ObjectCounter++;
        }

        public override void MouseUp()
        {
            
        }
    }
}
