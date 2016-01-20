using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    class DiagramCanvas : Canvas
    {
        public DiagramCanvas()
        {
            Background = Brushes.RoyalBlue;
            SelectedRelativeObjects = new ObservableCollection<ISelectableObject>();
            ObjectCounter = 0;
            MouseDown += DiagramCanvas_MouseDown;
            MouseUp += DiagramCanvas_MouseUp;
        }

        /// <summary>
        /// 在畫布上按下滑鼠的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CanvaseMouseDown(e.GetPosition(this));
        }

        /// <summary>
        /// 在畫布上按下滑鼠後鬆開的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
             CanvaseMouseUp(e.GetPosition(this));
        }



        public Modes Mode { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public BaseObject StartObject { get; set; }
        public BaseObject EndObject { get; set; }
        public ISelectableObject SelectedObject { get; set; }
        public ObservableCollection<ISelectableObject> SelectedRelativeObjects { get; set; }
        public bool PressingFlag { get; set; }

        public int ObjectCounter { get; set; }
        public bool LineFlag { get; set; }

        public void CleanSelectedObjects()
        {
            foreach (var baseObject in SelectedRelativeObjects)
            {
                baseObject.Selected = false;
            }
            SelectedRelativeObjects.Clear();
            SelectedObject = null;
        }

        /// <summary>
        /// 根據參數畫線
        /// </summary>
        /// <param name="startArgs"></param>
        /// <param name="endArgs"></param>
        /// <param name="mode"></param>
        public void DrawLine(ConnectionArgs startArgs, ConnectionArgs endArgs, Modes mode)
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

            var angle = connectionLine.Angle % 360 + 360;
            var d = (long)angle / 90;

            var xDiff = 0.0;
            var yDiff = 0.0;

            d = d % 4;
            var e = Math.Abs(angle % 90) / 90;

            switch (d)
            {
                case 0:
                    xDiff = e;
                    yDiff = e - 1;
                    break;

                case 1:
                    xDiff = 1 - e;
                    yDiff = e;
                    break;

                case 2:
                    xDiff = -e;
                    yDiff = 1 - e;
                    break;

                case 3:
                    xDiff = e - 1;
                    yDiff = -e;
                    break;
            }

            SetLeft(connectionLine, startArgs.TargetPoint.X + xDiff * 15);
            SetTop(connectionLine, startArgs.TargetPoint.Y + yDiff * 15);

            Children.Add(connectionLine);
            LineFlag = false;
        }

        public void CanvaseMouseUp(Point point)
        {

            EndPoint = point;

            // 連線模式
            if ((Mode == Modes.Associate || Mode == Modes.Composition || Mode == Modes.Generalize) && LineFlag)
            {
                LineModeMouseUp(point);
            }

            // 選取模式
            else if (Mode == Modes.Select && PressingFlag)
            {
                SelectModeMouseUp(point);
            }
        }

        public void CanvaseMouseDown(Point point)
        {
            StartPoint = point;
            EndPoint = StartPoint;
            StartObject = null;
            EndObject = null;

            // 插入 Class 模式
            if (Mode == Modes.Class)
            {
                CleanSelectedObjects();

                var baseObject = new ClassObject($"#{ObjectCounter} Class Object")
                {
                    Width = 150,
                    Height = 100
                };

                SetLeft(baseObject, point.X);
                SetTop(baseObject, point.Y);

                baseObject.StartPoint = point;
                Children.Add(baseObject);
                SelectedObject = baseObject;
                SelectedRelativeObjects.Add(baseObject);
                ObjectCounter++;
            }

            // 插入 Use Case 模式
            else if (Mode == Modes.UseCase)
            {
                CleanSelectedObjects();

                var baseObject = new UseCaseObject($"#{ObjectCounter} Use Case Object")
                {
                    Width = 150,
                    Height = 100
                };

                SetLeft(baseObject, point.X);
                SetTop(baseObject, point.Y);

                baseObject.StartPoint = point;
                Children.Add(baseObject);
                SelectedObject = baseObject;
                SelectedRelativeObjects.Add(baseObject);
                ObjectCounter++;
            }

            // 連線模式
            else if (Mode == Modes.Associate || Mode == Modes.Composition || Mode == Modes.Generalize)
            {
                foreach (var baseObject in Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    LineFlag = true;
                    StartObject = baseObject;
                    break;
                }
            }

            // 選取模式
            else if (Mode == Modes.Select)
            {
                PressingFlag = true;

                // Move Action
                if (SelectedRelativeObjects.Select(selectedObject => selectedObject as BaseObject).Any(baseObject => baseObject.IsContainPoint(point)))
                {
                    return;
                }

                CleanSelectedObjects();

                // Select
                foreach (var baseObject in Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    SelectedObject = baseObject;

                    if (baseObject.GetOutermostCompositer() != null)
                    {
                        var compositer = baseObject.GetOutermostCompositer();

                        compositer.Selected = true;
                        var members = compositer.GetAllBaseObjects();
                        foreach (var o in members)
                        {
                            SelectedRelativeObjects.Add(o);
                        }
                    }
                    else
                    {
                        baseObject.Selected = true;
                        SelectedRelativeObjects.Add(baseObject);
                    }
                }
            }
        }

        /// <summary>
        /// 在畫線模式下，滑鼠點擊後放開引發的事件
        /// </summary>
        /// <param name="point"></param>
        public void LineModeMouseUp(Point point)
        {
            foreach (var baseObject in Children.OfType<BaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
            {
                EndObject = baseObject;
                break;
            }

            if (StartObject != null && EndObject != null)
            {
                var startPort = GetNearestPort(StartPoint, StartObject);
                var endPort = GetNearestPort(EndPoint, EndObject);
                var startArgs = new ConnectionArgs()
                {
                    TargetObject = StartObject,
                    TargetPort = startPort
                };

                var endArgs = new ConnectionArgs()
                {
                    TargetObject = EndObject,
                    TargetPort = endPort
                };
                DrawLine(startArgs, endArgs, Mode);
            }
        }

        /// <summary>
        /// 取得某點在某物件上最近的 Port
        /// </summary>
        /// <param name="point"></param>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public Ports GetNearestPort(Point point, BaseObject baseObject)
        {
            var topDistance = CalculateTwoPointsDistance(point, baseObject.TopPoint);
            var rightDistance = CalculateTwoPointsDistance(point, baseObject.RightPoint);
            var bottomDistance = CalculateTwoPointsDistance(point, baseObject.BottomPoint);
            var leftDistance = CalculateTwoPointsDistance(point, baseObject.LeftPoint);

            var numbers = new[] { topDistance, rightDistance, bottomDistance, leftDistance };
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
        private static double CalculateTwoPointsDistance(Point aPoint, Point bPoint)
        {
            var xDiff = aPoint.X - bPoint.X;
            var yDiff = aPoint.Y - bPoint.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
        /// <summary>
        /// 在選取模式下，滑鼠點擊後放開引發的事件
        /// </summary>
        /// <param name="point"></param>
        public void SelectModeMouseUp(Point point)
        {
            var displacementX = StartPoint.X - point.X;
            var displacementY = StartPoint.Y - point.Y;

            if (Math.Abs(displacementX) < 1 && Math.Abs(displacementY) < 1)
            {
                // 不做事，但仍要執行下方的選單啟用狀態檢查
            }
            else if (SelectedRelativeObjects.Count == 0)
            {
                // 選取範圍模式
                SelectAreaAction(point);
            }
            else
            {
                // 移動模式
                MoveAction(displacementX, displacementY);
            }
        }

        /// <summary>
        /// 區域選取
        /// </summary>
        /// <param name="point"></param>
        public void SelectAreaAction(Point point)
        {
            var width = point.X - StartPoint.X;
            var height = point.Y - StartPoint.Y;
            var x = width > 0 ? StartPoint.X : point.Y;
            var y = height > 0 ? StartPoint.Y : point.Y;
            var rectPoint = new Point(x, y);
            var rectSize = new Size(Math.Abs(width), Math.Abs(height));
            var selectedArea = new Rect(rectPoint, rectSize);

            foreach (var baseObject in Children.OfType<BaseObject>())
            {
                var rect = baseObject.GetRect();

                if (selectedArea.Contains(rect))
                {
                    if (baseObject.Compositer == null)
                    {
                        SelectedRelativeObjects.Add(baseObject);
                        baseObject.Selected = true;
                    }
                    else
                    {
                        baseObject.Compositer.Selected = true;
                        foreach (var o in baseObject.Compositer.GetAllBaseObjects())
                        {
                            SelectedRelativeObjects.Add(o);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 移動物件
        /// </summary>
        /// <param name="displacementX"></param>
        /// <param name="displacementY"></param>
        public void MoveAction(double displacementX, double displacementY)
        {
            foreach (var selectedObject in SelectedRelativeObjects)
            {
                var userControl = selectedObject as UserControl;
                var baseObject = selectedObject as BaseObject;
                Debug.Assert(userControl != null);
                Debug.Assert(baseObject != null);



                var newStartPoint = new Point(baseObject.StartPoint.X - displacementX, baseObject.StartPoint.Y - displacementY);
                baseObject.StartPoint = newStartPoint;
                SetLeft(userControl, newStartPoint.X);
                SetTop(userControl, newStartPoint.Y);
                Children.Remove(userControl);
                Children.Add(userControl);

                var lines = new List<ConnectionLine>();
                lines.AddRange(Children.OfType<ConnectionLine>().ToList());

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
        /// 重劃指定的關係線
        /// </summary>
        /// <param name="line"></param>
        public void RedrawLine(ConnectionLine line)
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
            Children.Remove(line);
        }

        public void Group()
        {
            Debug.Assert(SelectedRelativeObjects.Count > 1);
            var baseObjectWithoutGroup = SelectedRelativeObjects.Where(selectObject => selectObject.Compositer == null).ToList();
            var compositeObjects = new List<ISelectableObject>();

            foreach (var selectObject in SelectedRelativeObjects.Where(selectObject => selectObject.Compositer != null))
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
        }

        public void UnGroup()
        {
            Debug.Assert(SelectedRelativeObjects.Count > 1);
            Debug.Assert(SelectedRelativeObjects[0].Compositer != null);

            var compositer = SelectedRelativeObjects[0].GetOutermostCompositer();
            Debug.Assert(compositer != null);

            foreach (var member in compositer.Members)
            {
                member.Compositer = null;
            }
        }
    }
}
