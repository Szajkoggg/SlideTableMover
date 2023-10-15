using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SlideTableMover.Model
{
    public class SlideTable
    {
        public int height;
        public int width;
        public double xCoordinate = 0;
        public double yCoordinate = 0;
        public UIElement element;
        public TextBox xCoordinateTextBox;
        public TextBox yCoordinateTextBox;

        public SlideTable(int height, int width, UIElement element, TextBox xCoordinateTextBox, TextBox yCoordinateTextBox)
        {
            this.height = height;
            this.width = width;
            this.element = element;
            this.xCoordinateTextBox = xCoordinateTextBox;
            this.yCoordinateTextBox = yCoordinateTextBox;
        }

        public int maxValueOnAxis(Axis axis)
        {
            if (axis == Axis.X)
            {
                return width;
            }
            if (axis == Axis.Y)
            {
                return height;
            }
            return 0;
        }
        public void updateTextBox(Dispatcher dispatcher)
        {
            dispatcher.Invoke(() =>
            {
                xCoordinateTextBox.Text = xCoordinate.ToString();
                yCoordinateTextBox.Text = yCoordinate.ToString();
            });
            }
    }
}
