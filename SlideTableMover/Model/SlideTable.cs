using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SlideTableMover.Model
{
    public class SlideTable
    {
        public int xSize;
        public int ySize;
        public double xCoordinate = 0;
        public double yCoordinate = 0;
        public double zCoordinate = 0;
        public UIElement element;
        public TextBox xCoordinateTextBox;
        public TextBox yCoordinateTextBox;
        public TextBox zCoordinateTextBox;

        public SlideTable(int ySize, int xSize, UIElement element, TextBox xCoordinateTextBox, TextBox yCoordinateTextBox, TextBox zCoordinateTextBox)
        {
            this.ySize = ySize;
            this.xSize = xSize;
            this.element = element;
            this.xCoordinateTextBox = xCoordinateTextBox;
            this.yCoordinateTextBox = yCoordinateTextBox;
            this.zCoordinateTextBox = zCoordinateTextBox;
        }

        public int maxValueOnAxis(Axis axis)
        {
            if (axis == Axis.X)
            {
                return xSize;
            }
            if (axis == Axis.Y)
            {
                return ySize;
            }
            return 0;
        }
        public void updateTextBox(Dispatcher dispatcher)
        {
            dispatcher.Invoke(() =>
            {
                xCoordinateTextBox.Text = xCoordinate.ToString();
                yCoordinateTextBox.Text = yCoordinate.ToString();
                zCoordinateTextBox.Text = zCoordinate.ToString();
            });
            }
    }
}
