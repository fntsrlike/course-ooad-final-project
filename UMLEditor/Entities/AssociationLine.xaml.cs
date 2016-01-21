using System;
using System.Windows;
using System.Windows.Media;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    /// <summary>
    /// GeneralizationLine.xaml 的互動邏輯
    /// </summary>
    public partial class AssociationLine
    {
        public AssociationLine(ConnectionArgs from, ConnectionArgs to) : base(from, to)
        {
            InitializeComponent();
            ArrowEndpointHeight = 0;
            TheArrowLine = ArrowLine;
            TheArrowCanvas = ArrowCanvas;
            Update();
        }
    }
}
