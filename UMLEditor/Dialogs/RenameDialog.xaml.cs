using System.Windows;

namespace UMLEditort.Dialogs
{
    /// <summary>
    /// RenameDialog.xaml 的互動邏輯
    /// </summary>
    public partial class RenameDialog
    {
        public RenameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 物件名稱屬性
        /// </summary>
        public string ObjectName
        {
            get { return ObjectNameTextBox.Text; }
            set { ObjectNameTextBox.Text = value; }
        }

        /// <summary>
        /// Ok 按鈕的點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
