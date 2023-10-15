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
        public MovementService(SlideTable slideTable, WorkArea workArea, Dispatcher dispatcher)
        {
            this.slideTable = slideTable;
            this.workArea = workArea;
            this.dispatcher = dispatcher;
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
            if(axis == Axis.X) {
                dispatcher.Invoke(() =>
                {
                    slideTable.xCoordinate = Canvas.GetLeft(slideTable.element);
                });
                MoveSlideTableTo(newCoordinate, slideTable.xCoordinate, animationDuration, Canvas.LeftProperty);
                updateSlideTableCoordinateTextBox();
            }
           
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
