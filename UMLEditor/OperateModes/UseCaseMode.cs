using System.Windows.Controls;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class UseCaseMode : BaseMode
    {
        public UseCaseMode(DiagramCanvas canvas) : base(canvas)
        {
            
        }              

        public override void MouseDown()
        {
            TheCanvas.CleanSelectedObjects();

            var baseObject = new UseCaseObject($"#{TheCanvas.ObjectCounter} Use Case Object")
            {
                Width = 150,
                Height = 100
            };

            Canvas.SetLeft(baseObject, TheCanvas.StartPoint.X);
            Canvas.SetTop(baseObject, TheCanvas.StartPoint.Y);

            baseObject.StartPoint = TheCanvas.StartPoint;
            TheCanvas.ExistBaseObjects.Add(baseObject);
            TheCanvas.SelectedObject = baseObject;
            TheCanvas.SelectedRelativeObjects.Add(baseObject);
            TheCanvas.ObjectCounter++;
        }

        public override void MouseUp()
        {
            
        }
    }
}
