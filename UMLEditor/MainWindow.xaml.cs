using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UMLEditort.Args;
using UMLEditort.Dialogs;
using UMLEditort.Entities;

namespace UMLEditort
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow
    {
        private bool _pressingFlag;
        private bool _lineFlag;
        private Point _startPoint;
        private Point _endPoint;
        private BaseObject _startObjetct;
        private BaseObject _endObject;
        private ISelectableObject _selectedObject;
        private readonly List<ISelectableObject> _selectedRelativeObjects;        
        private int _objectCounter;

        /// <summary>
        /// 初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _selectedRelativeObjects = new List<ISelectableObject>();
            _objectCounter = 0;
        }

        /// <summary>
        /// Select 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Select ? Modes.Undefined : Modes.Select;
            
        }

        /// <summary>
        /// Associate Line 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociateButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Associate ? Modes.Undefined : Modes.Associate;
        }

        /// <summary>
        /// Generalize Line 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralizeButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Generalize ? Modes.Undefined : Modes.Generalize;
        }

        /// <summary>
        /// Composition Line 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompositionButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Composition ? Modes.Undefined : Modes.Composition;
        }

        /// <summary>
        /// 插入 Class 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.Class ? Modes.Undefined : Modes.Class;
        }

        /// <summary>
        /// 插入 UseCase 模式按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseCaseButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Mode = _vm.Mode == Modes.UseCase ? Modes.Undefined : Modes.UseCase;
        }

        /// <summary>
        /// 在畫布上按下滑鼠的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            

            var point = e.GetPosition(DiagramCanvas);
            _startPoint = point;
            _endPoint = _startPoint;
            _startObjetct = null;
            _endObject = null;

            // 插入 Class 模式
            if (_vm.Mode == Modes.Class)
            {
                var baseObject = new ClassObject($"#{_objectCounter} Class Object")
                {
                    Width = 150,
                    Height = 100
                };

                Canvas.SetLeft(baseObject, point.X);
                Canvas.SetTop(baseObject, point.Y);

                baseObject.StartPoint = point;
                DiagramCanvas.Children.Add(baseObject);
                _selectedObject = baseObject;
                ChangeObjectName.IsEnabled = true;
                _objectCounter++;
            }

            // 插入 Use Case 模式
            else if (_vm.Mode == Modes.UseCase)
            {
                var baseObject = new UseCaseObject($"#{_objectCounter} Use Case Object")
                {
                    Width = 150,
                    Height = 100
                };

                Canvas.SetLeft(baseObject, point.X);
                Canvas.SetTop(baseObject, point.Y);

                baseObject.StartPoint = point;
                DiagramCanvas.Children.Add(baseObject);
                _selectedObject = baseObject;
                ChangeObjectName.IsEnabled = true;
                _objectCounter++;
            }

            // 連線模式
            else if (_vm.Mode == Modes.Associate || _vm.Mode == Modes.Composition || _vm.Mode == Modes.Generalize)
            {
                foreach (var baseObject in DiagramCanvas.Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    _lineFlag = true;
                    _startObjetct = baseObject;
                    break;
                }
            }

            // 選取模式
            else if (_vm.Mode == Modes.Select)
            {
                _pressingFlag = true;

                // Move Action
                if (_selectedRelativeObjects.Select(selectedObject => selectedObject as BaseObject).Any(baseObject => baseObject.IsContainPoint(point)))
                {
                    return;
                }

                // Cancle origin selected
                foreach (var baseObject in _selectedRelativeObjects)
                {
                    baseObject.Selected = false;
                }
                _selectedRelativeObjects.Clear();
                _selectedObject = null;
                ChangeObjectName.IsEnabled = false;

                // Select
                foreach (var baseObject in DiagramCanvas.Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    _selectedObject = baseObject;
                    ChangeObjectName.IsEnabled = true;

                    if (baseObject.GetOutermostCompositer() != null)
                    {
                        var compositer = baseObject.GetOutermostCompositer();                        

                        compositer.Selected = true;
                        var members = compositer.GetAllBaseObjects();
                        _selectedRelativeObjects.AddRange(members);
                    }
                    else
                    {
                        baseObject.Selected = true;
                        _selectedRelativeObjects.Add(baseObject);
                    }
                }
            }

        }

        /// <summary>
        /// 在畫布上按下滑鼠後鬆開的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(DiagramCanvas);
            _endPoint = point;

            // 連線模式
            if ((_vm.Mode == Modes.Associate || _vm.Mode == Modes.Composition || _vm.Mode == Modes.Generalize) && _lineFlag)
            {
                LineModeMouseUp(point);
            }

            // 選取模式
            else if (_vm.Mode == Modes.Select && _pressingFlag)
            {
                SelectModeMouseUp(point);
            }
        }

        /// <summary>
        /// Group 按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_selectedRelativeObjects.Count > 1);

            var baseObjectWithoutGroup = _selectedRelativeObjects.Where(selectObject => selectObject.Compositer == null).ToList();
            var compositeObjects = new List<ISelectableObject>();

            foreach (var selectObject in _selectedRelativeObjects.Where(selectObject => selectObject.Compositer != null))
            {
                var composite = selectObject.GetOutermostCompositer();
                
                if (!compositeObjects.Contains(composite))
                {
                    compositeObjects.Add(composite);
                }
            }

            var members = new List<ISelectableObject>();            
            members.AddRange(baseObjectWithoutGroup);
            members.AddRange(compositeObjects);

            var newCompositeObject = new CompositeObject(members);

            foreach (var baseObject in baseObjectWithoutGroup)
            {
                baseObject.Compositer = newCompositeObject;
            }

            foreach (var compositeObject in compositeObjects)
            {
                compositeObject.Compositer = newCompositeObject;
            }

            newCompositeObject.Selected = true;

            GroupMenuItem.IsEnabled = false;
            UnGroupMenuItem.IsEnabled = true;
        }

        /// <summary>
        /// Ungroup 按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_selectedRelativeObjects.Count > 1);
            Debug.Assert(_selectedRelativeObjects[0].Compositer != null);

            var compositer = _selectedRelativeObjects[0].GetOutermostCompositer();
            Debug.Assert(compositer != null);
            
            foreach (var member in compositer.Members)
            {
                member.Compositer = null;
            }

            GroupMenuItem.IsEnabled = true;
            UnGroupMenuItem.IsEnabled = false;
        }

        /// <summary>
        /// 在畫線模式下，滑鼠點擊後放開引發的事件
        /// </summary>
        /// <param name="point"></param>
        private void LineModeMouseUp(Point point)
        {
            foreach (var baseObject in DiagramCanvas.Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
            {
                _endObject = baseObject;
                break;
            }

            if (_startObjetct != null && _endObject != null)
            {
                var startPort = GetNearestPort(_startPoint, _startObjetct);
                var endPort = GetNearestPort(_endPoint, _endObject);
                var startArgs = new ConnectionArgs()
                {
                    TargetObject = _startObjetct,
                    TargetPort = startPort
                };

                var endArgs = new ConnectionArgs()
                {
                    TargetObject = _endObject,
                    TargetPort = endPort
                };
                DrawLine(startArgs, endArgs, _vm.Mode);
            }
        }

        /// <summary>
        /// 根據參數畫線
        /// </summary>
        /// <param name="startArgs"></param>
        /// <param name="endArgs"></param>
        /// <param name="mode"></param>
        private void DrawLine(ConnectionArgs startArgs, ConnectionArgs endArgs, Modes mode)
        {
            ConnectionLine connectionLine;
           
            switch (mode)
            {
                case Modes.Associate:
                    connectionLine = new AssociationLine(startArgs, endArgs);
                    break;
                case Modes.Composition:
                    connectionLine = new CompositionLine(startArgs, endArgs);
                    break;
                case Modes.Generalize:
                    connectionLine = new GeneralizationLine(startArgs, endArgs);
                    break;
                default:
                    return;
            }
            
            Canvas.SetLeft(connectionLine, startArgs.TargetPoint.X);
            Canvas.SetTop(connectionLine, startArgs.TargetPoint.Y-15);

            DiagramCanvas.Children.Add(connectionLine);
            _lineFlag = false;
        }

        /// <summary>
        /// 重劃指定的關係線
        /// </summary>
        /// <param name="line"></param>
        private void RedrawLine(ConnectionLine line)
        {
            Modes lindType;

            if (line is AssociationLine)
            {
                lindType = Modes.Associate;
            }
            else if (line is GeneralizationLine)
            {
                lindType = Modes.Generalize;
            }
            else if (line is CompositionLine)
            {
                lindType = Modes.Composition;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            DrawLine(line.StartConnectionArgs, line.EndConnectionArgs, lindType);
            DiagramCanvas.Children.Remove(line);
        }

        /// <summary>
        /// 取得某點在某物件上最近的 Port
        /// </summary>
        /// <param name="point"></param>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        private Ports GetNearestPort(Point point, BaseObject baseObject)
        {
            var topDistance = CalculateTwoPointsDistance(point, baseObject.TopPoint);
            var rightDistance = CalculateTwoPointsDistance(point, baseObject.RightPoint);
            var bottomDistance = CalculateTwoPointsDistance(point, baseObject.BottomPoint);
            var leftDistance = CalculateTwoPointsDistance(point, baseObject.LeftPoint);

            var numbers = new [] { topDistance, rightDistance, bottomDistance, leftDistance };
            var minimumNumber = numbers.Min();

            if (Math.Abs(topDistance - minimumNumber) < 0.001)
            {
                return Ports.Top;
            }

            if (Math.Abs(rightDistance - minimumNumber) < 0.001)
            {
                return Ports.Right;
            }

            if (Math.Abs(bottomDistance - minimumNumber) < 0.001)
            {
                return Ports.Bottom;
            }

            return Ports.Left;
        }

        /// <summary>
        /// 計算兩點之間的距離
        /// </summary>
        /// <param name="aPoint"></param>
        /// <param name="bPoint"></param>
        /// <returns></returns>
        private double CalculateTwoPointsDistance(Point aPoint, Point bPoint)
        {
            var xDiff = aPoint.X - bPoint.X;
            var yDiff = aPoint.Y - bPoint.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }

        /// <summary>
        /// 在選取模式下，滑鼠點擊後放開引發的事件
        /// </summary>
        /// <param name="point"></param>
        private void SelectModeMouseUp(Point point)
        {
            var displacementX = _startPoint.X - point.X;
            var displacementY = _startPoint.Y - point.Y;

            if (Math.Abs(displacementX) < 1 && Math.Abs(displacementY) < 1)
            {       
                // 不做事，但仍要執行下方的選單啟用狀態檢查
            }            
            else if (_selectedRelativeObjects.Count == 0)
            {
                // 選取範圍模式
                SelectAreaAction(point);
            }
            else
            {
                // 移動模式
                MoveAction(displacementX, displacementY);
            }

            CheckGroupMenuItemIsEnabled();
        }

        /// <summary>
        /// 確認 Group/Ungroup 按鈕是否被啟用
        /// </summary>
        private void CheckGroupMenuItemIsEnabled()
        {
            // 確認是否可以 GROUP
            var groupFlag = false;
            var ungroupFlag = false;

            // 
            if (_selectedRelativeObjects.Count > 1)
            {
                var isSame = false;

                // 取得作為標準 Compositer
                var compositer = _selectedRelativeObjects[0].GetOutermostCompositer();

                if (compositer != null)
                {
                    foreach (var selectedObject in _selectedRelativeObjects)
                    {
                        isSame = compositer.Equals(selectedObject.GetOutermostCompositer());
                        if (!isSame)
                        {
                            break;
                        }
                    }
                }

                groupFlag = !isSame;
                ungroupFlag = isSame;
            }

            GroupMenuItem.IsEnabled = groupFlag;
            UnGroupMenuItem.IsEnabled = ungroupFlag;
        }

        /// <summary>
        /// 區域選取
        /// </summary>
        /// <param name="point"></param>
        private void SelectAreaAction(Point point)
        {
            var width = point.X - _startPoint.X;
            var height = point.Y - _startPoint.Y;
            var x = width > 0 ? _startPoint.X : point.Y;
            var y = height > 0 ? _startPoint.Y : point.Y;
            var rectPoint = new Point(x, y);
            var rectSize = new Size(Math.Abs(width), Math.Abs(height));
            var selectedArea = new Rect(rectPoint, rectSize);

            foreach (var baseObject in DiagramCanvas.Children.OfType<BaseObject>())
            {
                var rect = baseObject.GetRect();

                if (selectedArea.Contains(rect))
                {
                    if (baseObject.Compositer == null)
                    {
                        _selectedRelativeObjects.Add(baseObject);
                        baseObject.Selected = true;
                    }
                    else
                    {
                        baseObject.Compositer.Selected = true;
                        _selectedRelativeObjects.AddRange(baseObject.Compositer.GetAllBaseObjects());
                    }

                }
            }
        }

        /// <summary>
        /// 移動物件
        /// </summary>
        /// <param name="displacementX"></param>
        /// <param name="displacementY"></param>
        private void MoveAction(double displacementX, double displacementY)
        {
            foreach (var selectedObject in _selectedRelativeObjects)
            {
                var userControl = selectedObject as UserControl;
                var baseObject = selectedObject as BaseObject;
                Debug.Assert(userControl != null);
                Debug.Assert(baseObject != null);

                

                var newStartPoint = new Point(baseObject.StartPoint.X - displacementX, baseObject.StartPoint.Y - displacementY);
                baseObject.StartPoint = newStartPoint;
                Canvas.SetLeft(userControl, newStartPoint.X);
                Canvas.SetTop(userControl, newStartPoint.Y);
                DiagramCanvas.Children.Remove(userControl);
                DiagramCanvas.Children.Add(userControl);

                var lines = new List<ConnectionLine>();
                lines.AddRange(DiagramCanvas.Children.OfType<ConnectionLine>().ToList());

                foreach (var line in lines)
                {
                    if (line.StartConnectionArgs.TargetObject.Equals(selectedObject))
                    {
                        RedrawLine(line);
                    }
                    else if (line.EndConnectionArgs.TargetObject.Equals(selectedObject))
                    {
                        RedrawLine(line);
                    }
                }
            }
        }

        /// <summary>
        /// 改變物件名稱按鈕的點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeObjectName_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_selectedObject != null);
            Debug.Assert(_selectedObject is BaseObject);

            var baseObject = (BaseObject) _selectedObject;

            var dialog = new RenameDialog()
            {
                ObjectName = baseObject.ObjectName,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (dialog.ShowDialog() == true)
            {
                baseObject.ObjectName = dialog.ObjectName;
            }
        }
    }
}
