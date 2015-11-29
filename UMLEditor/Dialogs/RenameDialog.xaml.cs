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

        public string ObjectName
        {
            get { return ObjectNameTextBox.Text; }
            set { ObjectNameTextBox.Text = value; }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
