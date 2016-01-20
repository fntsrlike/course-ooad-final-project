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
        /// <summary>
        /// 初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            CanvasPanel.Children.Add(_vm.DiagramCanvas);
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
        /// Group 按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _vm.Group();
        }

        /// <summary>
        /// Ungroup 按鈕點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _vm.UnGroup();
        }
        
        /// <summary>
        /// 改變物件名稱按鈕的點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeObjectName_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_vm.DiagramCanvas.SelectedObject != null);
            Debug.Assert(_vm.DiagramCanvas.SelectedObject is BaseObject);

            var baseObject = (BaseObject)_vm.DiagramCanvas.SelectedObject;

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
