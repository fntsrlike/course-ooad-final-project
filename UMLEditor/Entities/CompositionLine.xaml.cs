using UMLEditort.Args;

namespace UMLEditort.Entities
{
    /// <summary>
    /// GeneralizationLine.xaml 的互動邏輯
    /// </summary>
    public partial class CompositionLine
    {
        public CompositionLine(ConnectionArgs from, ConnectionArgs to) : base(from, to)
        {
            InitializeComponent();
            ArrowEndpointHeight = 30;
            TheArrowLine = ArrowLine;
            TheArrowCanvas = ArrowCanvas;
            Update();
        }
    }
}
