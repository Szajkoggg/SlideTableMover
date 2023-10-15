using System;
using System.Timers;
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
        private double motorX = 0;         // Motor X position
        private double motorY = 0;         // Motor Y position
        private double motorStepSize = 5;  // Size of each motor step
        private int motorXDirection = 1;   // Motor X direction (1 for right, -1 for left)
        private int motorYDirection = 1;   // Motor Y direction (1 for down, -1 for up)
        private int motorXInterval = 100;  // Interval for X motor timer (milliseconds)
        private int motorYInterval = 100;  // Interval for Y motor timer (milliseconds)
        private Timer motorXTimer;         // Timer for X motor
        private Timer motorYTimer;         // Timer for Y motor

        public MainWindow()
        {
            InitializeComponent();

            motorXTimer = new Timer(motorXInterval);
            motorXTimer.Elapsed += MotorXTimer_Elapsed;

            motorYTimer = new Timer(motorYInterval);
            motorYTimer.Elapsed += MotorYTimer_Elapsed;

            motorXTimer.Start();
            motorYTimer.Start();
        }

        private void MotorXTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            motorX += motorXDirection * motorStepSize;
            MoveRectangleTo(motorX, motorY);
        }

        private void MotorYTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            motorY += motorYDirection * motorStepSize;
            MoveRectangleTo(motorX, motorY);
        }
        private void StartMotors()
        {
            motorXTimer.Start();
            motorYTimer.Start();
        }

        private void StopMotors()
        {
            motorXTimer.Stop();
            motorYTimer.Stop();
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
            double currentX = 0;
            double currentY = 0;

            Dispatcher.Invoke(() =>
            {
                currentX = Canvas.GetLeft(movingRectangle);
                currentY = Canvas.GetTop(movingRectangle);
            });

            double deltaX = newX - currentX;
            double deltaY = newY - currentY;

            double xDuration = Math.Abs(deltaX) / xSpeed;
            double yDuration = Math.Abs(deltaY) / ySpeed;

            
            // Use Dispatcher.Invoke to update UI elements from a background thread
            Dispatcher.Invoke(() =>
            {
                DoubleAnimation xAnimation = new DoubleAnimation(newX, TimeSpan.FromSeconds(xDuration));
                DoubleAnimation yAnimation = new DoubleAnimation(newY, TimeSpan.FromSeconds(yDuration));
                movingRectangle.BeginAnimation(Canvas.LeftProperty, xAnimation);
                movingRectangle.BeginAnimation(Canvas.TopProperty, yAnimation);
                currentXTextBox.Text = newX.ToString();
                currentYTextBox.Text = newY.ToString();
            });
        }

        private void WorkPane_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double xClicked = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            double yClicked = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;

            MoveRectangleTo(xClicked, yClicked);
        }
    }
}
