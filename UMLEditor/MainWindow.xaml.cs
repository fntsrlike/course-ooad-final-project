using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLEditort.Entities;

namespace UMLEditort
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Class ? Modes.Undefined : Modes.Class;
        }

        private void UseCaseButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.UseCase ? Modes.Undefined : Modes.UseCase;
        }

        private void DiagramCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(DiagramCanvas);

            if (_vm.Mode == Modes.Class)
            {
                var baseObject = new ClassObject("Class Object")
                {
                    Width = 100,
                    Height = 100
                };

                Canvas.SetLeft(baseObject, point.X);
                Canvas.SetTop(baseObject, point.Y);

                DiagramCanvas.Children.Add(baseObject);
            }
            else if (_vm.Mode == Modes.UseCase)
            {
                var baseObject = new UseCaseObject("Use Case Object")
                {
                    Width = 150,
                    Height = 100
                };

                Canvas.SetLeft(baseObject, point.X);
                Canvas.SetTop(baseObject, point.Y);

                DiagramCanvas.Children.Add(baseObject);
            }
        }
    }
}
