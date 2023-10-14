using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SlideTableMover
{

    public partial class MainWindow : Window
    {
        private double xSpeed = 50;
        private double ySpeed = 100;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ApplySpeed_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(xSpeedTextBox.Text, out double newXSpeed) && double.TryParse(ySpeedTextBox.Text, out double newYSpeed))
            {
                MessageBox.Show("Speed updated.");
                xSpeed = Math.Max(0, newXSpeed);
                ySpeed = Math.Max(0, newYSpeed);

            }
            else
            {
                MessageBox.Show("Invalid speed values. Please enter valid numbers.");
            }
        }
        private void ApplyCoordinates_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(newXTextBox.Text, out double newX) && double.TryParse(newYTextBox.Text, out double newY))
            {
                MoveRectangleTo(newX, newY);
            }
            else
            {
                MessageBox.Show("Invalid coordinate values. Please enter valid numbers.");
            }
        }

        private void MoveRectangleTo(double newX, double newY)
        {
            double currentX = Canvas.GetLeft(movingRectangle);
            double currentY = Canvas.GetTop(movingRectangle);

            double deltaX = newX - currentX;
            double deltaY = newY - currentY;

            double xDuration = Math.Abs(deltaX) / xSpeed;
            double yDuration = Math.Abs(deltaY) / ySpeed;

            DoubleAnimation xAnimation = new DoubleAnimation(newX, TimeSpan.FromSeconds(xDuration));
            DoubleAnimation yAnimation = new DoubleAnimation(newY, TimeSpan.FromSeconds(yDuration));

            movingRectangle.BeginAnimation(Canvas.LeftProperty, xAnimation);
            movingRectangle.BeginAnimation(Canvas.TopProperty, yAnimation);

            currentXTextBox.Text = newX.ToString();
            currentYTextBox.Text = newY.ToString();
        }

        private void WorkPane_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double xClicked = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            double yClicked = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;

            MoveRectangleTo(xClicked, yClicked);
        }
    }
}
