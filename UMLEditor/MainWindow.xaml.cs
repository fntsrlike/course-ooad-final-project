﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using UMLEditort.Entities;

namespace UMLEditort
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _pressingFlag;
        private bool _lineFlag;
        private Point _startPoint;
        private IBaseObject _startObjetct;
        private IBaseObject _endObject;
        private readonly List<ISelectableObject> _selectedObjects;

        public MainWindow()
        {
            InitializeComponent();
            _selectedObjects = new List<ISelectableObject>();
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

            // 插入 Class 模式
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

            // 插入 Use Case 模式
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

            // 連線模式
            else if (_vm.Mode == Modes.Associate || _vm.Mode == Modes.Composition || _vm.Mode == Modes.Generalize)
            {
                foreach (var baseObject in DiagramCanvas.Children.OfType<IBaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    _lineFlag = true;
                    _startObjetct = baseObject;
                    break;
                }
            }

            // 選取模式
            else if (_vm.Mode == Modes.Select)
            {
                _startPoint = point;
                _pressingFlag = true;

                // Cancle origin selected
                foreach (var baseObject in _selectedObjects)
                {
                    baseObject.Selected = false;
                }
                _selectedObjects.Clear();

                // Select
                foreach (var baseObject in DiagramCanvas.Children.OfType<IBaseObject>().Select(child => child).Where(baseObject => baseObject.IsContainPoint(point)))
                {
                    if (baseObject.Compositer != null)
                    {
                        var compositer = baseObject.Compositer;
                        compositer.Selected = true;
                        var members = baseObject.Compositer.GetAllBaseObjects();
                        _selectedObjects.AddRange(members);
                    }
                    else
                    {
                        baseObject.Selected = true;
                        _selectedObjects.Add(baseObject);
                    }
                }
            }
        }

        private void DiagramCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(DiagramCanvas);

            // 連線模式
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

            // 選取模式
            else if (_vm.Mode == Modes.Select && _pressingFlag)
            {
                var displacementX = _startPoint.X - point.X;
                var displacementY = _startPoint.Y - point.Y;

                if (Math.Abs(displacementX) < 1 && Math.Abs(displacementY) < 1)
                {
                    return;
                    
                }

                // 選取範圍模式
                if (_selectedObjects.Count == 0)
                {
                    var width = point.X - _startPoint.X;
                    var height = point.Y - _startPoint.Y;
                    var x = width > 0 ? _startPoint.X : point.Y;
                    var y = height > 0 ? _startPoint.Y : point.Y;
                    var rectPoint = new Point(x, y);
                    var rectSize = new Size(Math.Abs(width), Math.Abs(height));
                    var selectedArea = new Rect(rectPoint, rectSize);

                    foreach (var baseObject in DiagramCanvas.Children.OfType<IBaseObject>())
                    {
                        var rect = baseObject.GetRect();

                        if (selectedArea.IntersectsWith(rect))
                        {
                            if (baseObject.Compositer == null)
                            {
                                _selectedObjects.Add(baseObject);
                                baseObject.Selected = true;
                            }
                            else
                            {
                                baseObject.Compositer.Selected = true;
                                _selectedObjects.AddRange(baseObject.Compositer.GetAllBaseObjects());
                            }
                            
                        }
                    }
                }

                // 移動模式
                else
                {
                    foreach (var selectedObject in _selectedObjects)
                    {
                        var userControl = selectedObject as UserControl;
                        var baseObject = selectedObject as IBaseObject;
                        Debug.Assert(userControl != null);
                        Debug.Assert(baseObject != null);

                        DiagramCanvas.Children.Remove(userControl);

                        var newStartPoint = new Point(baseObject.StartPoint.X - displacementX, baseObject.StartPoint.Y - displacementY);
                        baseObject.StartPoint = newStartPoint;
                        Canvas.SetLeft(userControl, newStartPoint.X);
                        Canvas.SetTop(userControl, newStartPoint.Y);
                        DiagramCanvas.Children.Add(userControl);
                    }

                }
                
            }
        }

        private void GroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedObjects.Count < 1)
            {
                return;
            }

            var compositeObject = new CompositeObject(_selectedObjects);

            foreach (var selectedObject in _selectedObjects)
            {
                selectedObject.Compositer = compositeObject;
            }

            compositeObject.Selected = true;
        }

        private void UnGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedObjects.Count < 1 && _selectedObjects[0].Compositer == null)
            {
                return;
            }

            var compoister = _selectedObjects[0].Compositer;
            if (_selectedObjects.Any(selectedObject => !compoister.Equals(selectedObject.Compositer)))
            {
                return;
            }

            foreach (var selectedObject in _selectedObjects)
            {
                selectedObject.Compositer = null;
            }
        }
    }
}
