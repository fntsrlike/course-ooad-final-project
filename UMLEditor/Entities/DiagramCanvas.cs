using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLEditort.OperateModes;

namespace UMLEditort.Entities
{
    public class DiagramCanvas : Canvas
    {
        public delegate void SelectedObjectChangedDelegate();
        public SelectedObjectChangedDelegate SelectedObjectChanged;
        private ISelectableObject _selectedObject;
        private ModesFactory _modesFactory;

        public DiagramCanvas()
        {
            Background = Brushes.RoyalBlue;
            SelectedRelativeObjects = new ObservableCollection<ISelectableObject>();
            ObjectCounter = 0;
            MouseDown += DiagramCanvas_MouseDown;
            MouseUp += DiagramCanvas_MouseUp;
            _modesFactory = new ModesFactory(this);
        }

        // 基本屬性
        public int ObjectCounter { get; set; }
        public bool IsSelectModeDragging { get; set; }
        public bool IsLineModeDragging { get; set; }
        public Modes Mode { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public BaseObject StartObject { get; set; }
        public BaseObject EndObject { get; set; }
        public ObservableCollection<ISelectableObject> SelectedRelativeObjects { get; set; }
        public ISelectableObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject == value) return;
                _selectedObject = value;
                SelectedObjectChanged();
            }
        }

        /// <summary>
        /// Group 選取中的 BaseObjects
        /// </summary>
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

        /// <summary>
        /// UnGroup 選取中的 BaseObjects
        /// </summary>
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
        
        /// <summary>
        /// 清除選取狀態
        /// </summary>
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
        /// 在畫布上按下滑鼠的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartPoint = e.GetPosition(this);
            EndPoint = StartPoint;
            StartObject = null;
            EndObject = null;

            _modesFactory.GetMode(Mode).MouseDown();
        }

        /// <summary>
        /// 在畫布上按下滑鼠後鬆開的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EndPoint = e.GetPosition(this);
            _modesFactory.GetMode(Mode).MouseUp();
        }

        /// <summary>
        /// 取得某點在某物件上最近的 Port
        /// </summary>
        /// <param name="point"></param>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public static Ports GetNearestPort(Point point, BaseObject baseObject)
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
        public static double CalculateTwoPointsDistance(Point aPoint, Point bPoint)
        {
            var xDiff = aPoint.X - bPoint.X;
            var yDiff = aPoint.Y - bPoint.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
    }
}
