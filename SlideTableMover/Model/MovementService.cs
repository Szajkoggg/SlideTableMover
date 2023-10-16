using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SlideTableMover.Model
{
    public class MovementService
    {
        private SlideTable slideTable;
        private WorkArea workArea;
        private readonly Dispatcher dispatcher;
        private MainWindow window;
        public MovementService(SlideTable slideTable, WorkArea workArea, Dispatcher dispatcher, MainWindow window)
        {
            this.slideTable = slideTable;
            this.workArea = workArea;
            this.dispatcher = dispatcher;
            this.window = window;
        }
        public bool motorCanMoveTo(double newPosition, Axis axis)
        {
            if (newPosition >= 0 && newPosition <= workArea.maxValueOnAxis(axis) - slideTable.maxValueOnAxis(axis))
            {
                return true;
            }
            return false;
        }
        public void moveSlideTable(Axis axis, double newCoordinate, double animationDuration)
        {
            if(axis == Axis.Y)
            {
                dispatcher.Invoke(() =>
                {
                    slideTable.yCoordinate = Canvas.GetTop(slideTable.element);
                });
                MoveSlideTableTo(newCoordinate, slideTable.yCoordinate, animationDuration, Canvas.TopProperty);
                updateSlideTableCoordinateTextBox();
            }
            if(axis == Axis.X) 
            {
                dispatcher.Invoke(() =>
                {
                    slideTable.xCoordinate = Canvas.GetLeft(slideTable.element);
                });
                MoveSlideTableTo(newCoordinate, slideTable.xCoordinate, animationDuration, Canvas.LeftProperty);
                updateSlideTableCoordinateTextBox();
            }
            if(axis == Axis.Z) 
            {
                double newXSize = slideTable.xSize * (1 + newCoordinate / workArea.zSize);
                double newYSize = slideTable.ySize * (1 + newCoordinate / workArea.zSize);

                ResizeSlideTable(newXSize, newYSize, animationDuration);
            }
           
        }
        private void ResizeSlideTable(double newXSize, double newYSize, double animationDuration)
        {
            dispatcher.Invoke(() =>
            {
                
                DoubleAnimation widthAnimation = new DoubleAnimation(newYSize, TimeSpan.FromMilliseconds(animationDuration));
                DoubleAnimation heightAnimation = new DoubleAnimation(newXSize, TimeSpan.FromMilliseconds(animationDuration));

                slideTable.element.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
                slideTable.element.BeginAnimation(FrameworkElement.HeightProperty, heightAnimation);
                
                double currentX = Canvas.GetLeft(slideTable.element);
                double currentY = Canvas.GetTop(slideTable.element);

                double newX = currentX - (newXSize - window.slideTableElement.ActualWidth) / 2;
                double newY = currentY - (newYSize - window.slideTableElement.ActualHeight) / 2;

                MoveSlideTableTo(newX, currentX, animationDuration, Canvas.LeftProperty);
                MoveSlideTableTo(newY, currentY, animationDuration, Canvas.TopProperty);
               
            });
        }
        private void MoveSlideTableTo(double newCoordinate, double currentCoordinate, double animationDuration, DependencyProperty canvasProperty)
        {
            double delta = newCoordinate - currentCoordinate;

            dispatcher.Invoke(() =>
            {
                DoubleAnimation animation = new DoubleAnimation(newCoordinate, TimeSpan.FromMilliseconds(animationDuration));
                slideTable.element.BeginAnimation(canvasProperty, animation);
            });

        }

        public void updateSlideTableCoordinateTextBox()
        { 
            slideTable.updateTextBox(dispatcher);
        }

        public void updateMotorPosTextBox(TextBox textBox, string text)
        {
            dispatcher.Invoke(() =>
            {
                textBox.Text = text;
            });
        }
    }
}
