using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SlideTableMover
{

    public partial class MainWindow : Window
    {
  
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double xClicked = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            double yClicked = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;

            double currentX = Canvas.GetLeft(movingRectangle);
            double currentY = Canvas.GetTop(movingRectangle);

            double deltaX = xClicked - currentX;
            double deltaY = yClicked - currentY;

            double xSpeed = 50; // Speed in the X direction (pixels per second)
            double ySpeed = 100; // Speed in the Y direction (pixels per second)

            double xDuration = Math.Abs(deltaX) / xSpeed;
            double yDuration = Math.Abs(deltaY) / ySpeed;

            // Create and configure DoubleAnimations for X and Y movement
            DoubleAnimation xAnimation = new DoubleAnimation(xClicked, TimeSpan.FromSeconds(xDuration));
            DoubleAnimation yAnimation = new DoubleAnimation(yClicked, TimeSpan.FromSeconds(yDuration));

            // Set the target properties for the animations
            movingRectangle.BeginAnimation(Canvas.LeftProperty, xAnimation);
            movingRectangle.BeginAnimation(Canvas.TopProperty, yAnimation);
        }
    }
}
