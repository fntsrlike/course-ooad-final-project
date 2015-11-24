using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UMLEditort.Entities;

namespace UMLEditort
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBaseObject _startObjetct;
        private IBaseObject _endObject;
        private bool _lineFlag = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Select ? Modes.Undefined : Modes.Select;
            
        }

        private void AssociateButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Associate ? Modes.Undefined : Modes.Associate;
        }

        private void GeneralizeButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Generalize ? Modes.Undefined : Modes.Generalize;
        }

        private void CompositionButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Composition ? Modes.Undefined : Modes.Composition;
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

                baseObject.StartPoint = point;
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

                baseObject.StartPoint = point;
                DiagramCanvas.Children.Add(baseObject);
            }
            else if (_vm.Mode == Modes.Associate || _vm.Mode == Modes.Composition || _vm.Mode == Modes.Generalize)
            {
                foreach (var baseObject in DiagramCanvas.Children.OfType<IBaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    _lineFlag = true;
                    _startObjetct = baseObject;
                    break;
                }
            }
        }

        private void DiagramCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(DiagramCanvas);

            if ((_vm.Mode == Modes.Associate || _vm.Mode == Modes.Composition || _vm.Mode == Modes.Generalize) && _lineFlag)
            {
                foreach (var baseObject in DiagramCanvas.Children.OfType<IBaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    _endObject = baseObject;
                    break;
                }

                ConnetionLine connectionLine;
                switch (_vm.Mode)
                {
                    case Modes.Associate:
                        connectionLine = new AssociationLine(_startObjetct, _endObject);
                        break;
                    case Modes.Composition:
                        connectionLine = new CompositionLine(_startObjetct, _endObject);
                        break;
                    case Modes.Generalize:
                        connectionLine = new GeneralizationLine(_startObjetct, _endObject);
                        break;
                    default:
                        return;
                }


                var line = connectionLine.GetLine();
                Canvas.SetLeft(line, 0);
                Canvas.SetTop(line, 0);

                DiagramCanvas.Children.Add(line);
                _lineFlag = false;
            }
        }
    }
}
