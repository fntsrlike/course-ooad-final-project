﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UMLEditort.Entities;

namespace UMLEditort.OperateModes
{
    class SelectMode : BaseMode
    {
        public SelectMode(DiagramCanvas canvas) : base(canvas)
        {
        }

        public override void MouseDown()
        {
            TheCanvas.IsSelectModeDragging = true;

            // Move Action
            if (TheCanvas.SelectedRelativeObjects.Select(selectedObject => selectedObject).Any(baseObject => baseObject.IsContainPoint(TheCanvas.StartPoint)))
            {
                return;
            }

            TheCanvas.CleanSelectedObjects();

            // Select
            foreach (var baseObject in TheCanvas.ExistBaseObjects.Select(child => child).Where(baseObject => baseObject.IsContainPoint(TheCanvas.StartPoint)))
            {
                TheCanvas.SelectedObject = baseObject;

                if (baseObject.GetOutermostCompositer() != null)
                {
                    var compositer = baseObject.GetOutermostCompositer();

                    compositer.Selected = true;
                    var members = compositer.GetAllBaseObjects();
                    foreach (var o in members)
                    {
                        TheCanvas.SelectedRelativeObjects.Add(o);
                    }
                }
                else
                {
                    baseObject.Selected = true;
                    TheCanvas.SelectedRelativeObjects.Add(baseObject);
                }
            }
        }

        public override void MouseUp()
        {
            var displacementX = TheCanvas.StartPoint.X - TheCanvas.EndPoint.X;
            var displacementY = TheCanvas.StartPoint.Y - TheCanvas.EndPoint.Y;

            if (Math.Abs(displacementX) < 1 && Math.Abs(displacementY) < 1)
            {
                // 不做事，但仍要執行下方的選單啟用狀態檢查
            }
            else if (TheCanvas.SelectedRelativeObjects.Count == 0)
            {
                // 選取範圍模式
                SelectAreaAction();
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
        private void SelectAreaAction()
        {
            var width = TheCanvas.EndPoint.X - TheCanvas.StartPoint.X;
            var height = TheCanvas.EndPoint.Y - TheCanvas.StartPoint.Y;
            var x = width > 0 ? TheCanvas.StartPoint.X : TheCanvas.EndPoint.X;
            var y = height > 0 ? TheCanvas.StartPoint.Y : TheCanvas.EndPoint.Y;
            var rectPoint = new Point(x, y);
            var rectSize = new Size(Math.Abs(width), Math.Abs(height));
            var selectedArea = new Rect(rectPoint, rectSize);

            foreach (var baseObject in from baseObject in TheCanvas.ExistBaseObjects let rect = baseObject.GetRect() where selectedArea.Contains(rect) select baseObject)
            {
                if (baseObject.Compositer == null)
                {
                    TheCanvas.SelectedRelativeObjects.Add(baseObject);
                    baseObject.Selected = true;
                }
                else
                {
                    baseObject.Compositer.Selected = true;
                    foreach (var o in baseObject.Compositer.GetAllBaseObjects())
                    {
                        TheCanvas.SelectedRelativeObjects.Add(o);
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
            foreach (var selectedObject in TheCanvas.SelectedRelativeObjects)
            {
                var baseObject = selectedObject;
                Debug.Assert(baseObject != null);

                var newStartPoint = new Point(baseObject.StartPoint.X - displacementX, baseObject.StartPoint.Y - displacementY);
                baseObject.StartPoint = newStartPoint;
                Canvas.SetLeft(baseObject, newStartPoint.X);
                Canvas.SetTop(baseObject, newStartPoint.Y);

                var lines = new List<ConnectionLine>();
                lines.AddRange(TheCanvas.ExistLines.ToList());

                foreach (var line in lines.Where(line => line.StartConnectionArgs.TargetObject.Equals(selectedObject) || line.EndConnectionArgs.TargetObject.Equals(selectedObject)))
                {
                    line.Update();
                }
            }
        }
    }

}
