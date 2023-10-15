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
        double currentX = 0;
        double currentY = 0;
        private double motorX = 0;
        private double motorY = 0;
        private double motorXCoordinate = 0;
        private double motorYCoordinate = 0;
        private double motorXStepSize = 0.3;
        private double motorYStepSize = 0.5;
        private int motorXDirection = 1;   // Motor X direction (1 for right, -1 for left)
        private int motorYDirection = 1;   // Motor Y direction (1 for down, -1 for up)
        private int motorXStepDurationms = 10;
        private int motorYStepDurationms = 10;
        private Timer motorXTimer;
        private Timer motorYTimer;
        private double targetX;
        private double targetY;

        public MainWindow()
        {
            InitializeComponent();

            motorXTimer = new Timer(motorXStepDurationms);
            motorXTimer.Elapsed += MotorXTimer_Elapsed;
            motorXTimer.AutoReset = true;

            motorYTimer = new Timer(motorYStepDurationms);
            motorYTimer.Elapsed += MotorYTimer_Elapsed;
            motorYTimer.AutoReset = true;

            movingRectangle.Width = 50;
            movingRectangle.Height = 50;
        }
        private void MotorYTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            double deltaY = targetY - motorYCoordinate;

            if (Math.Abs(deltaY) < motorYStepSize)
            {
                StopYMotor();
                return;
            }
            motorYDirection = deltaY > 0 ? 1 : -1;

            if (Math.Abs(deltaY) > motorYStepSize)
            {
                double newY = motorYCoordinate + motorYDirection * motorYStepSize;
                Dispatcher.Invoke(() =>
                {
                    if (newY >= 0 && newY <= map.Height - movingRectangle.Height)
                    {
                        motorY = motorY + motorYDirection;
                        motorYCoordinate = newY;
                    }
                });
            }
            Dispatcher.Invoke(() =>
            {
                currentY = Canvas.GetTop(movingRectangle);
            });
            MoveRectangleTo(motorYCoordinate, currentY, motorYStepDurationms, Canvas.TopProperty);
            UpdateTextBoxes();

        }
        private void MotorXTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            double deltaX = targetX - motorXCoordinate;

            if (Math.Abs(deltaX) < motorXStepSize)
            {
                StopXMotor();
                return;
            }
            motorXDirection = deltaX > 0 ? 1 : -1;

            if (Math.Abs(deltaX) > motorXStepSize)
            {
                double newX = motorXCoordinate + motorXDirection * motorXStepSize;
                Dispatcher.Invoke(() =>
                {
                    if (newX >= 0 && newX <= map.Width - movingRectangle.Width)
                    {
                        motorX = motorX + motorXDirection;
                        motorXCoordinate = newX;
                    }

                });
            }
            Dispatcher.Invoke(() =>
            {
                currentX = Canvas.GetLeft(movingRectangle);
            });
            MoveRectangleTo(motorXCoordinate, currentX, motorXStepDurationms, Canvas.LeftProperty);
            UpdateTextBoxes();

        }
        private void UpdateTextBoxes()
        {
            Dispatcher.Invoke(() =>
            {
                xPosTextBox.Text = motorX.ToString();
                yPosTextBox.Text = motorY.ToString();
                currentXTextBox.Text = motorXCoordinate.ToString();
                currentYTextBox.Text = motorYCoordinate.ToString();
            });
        }
        private void StartXMotor()
        {
            motorXTimer.Start();
        }
        private void StartYMotor()
        {
            motorYTimer.Start();
        }
        private void StopXMotor()
        {
            motorXTimer.Stop();
        }
        private void StopYMotor()
        {
            motorYTimer.Stop();
        }
        
        private void StartMotors()
        {
            StartXMotor();
            StartYMotor();

        }
        private void StopMotors()
        {
            StopXMotor();
            StopYMotor();
        }
       

        private void ApplySpeed_Click(object sender, RoutedEventArgs e)
        {
            
            if (double.TryParse(MotorXStepTextBox.Text, out double newMotorXStepSize) && double.TryParse(MotorXStepDurationTextBox.Text, out double newMotorXStepDurationms) && double.TryParse(MotorYStepTextBox.Text, out double newMotorYStepSize) && double.TryParse(MotorYStepDurationTextBox.Text, out double newMotorYStepDurationms))
            {
                StopMotors();
                MessageBox.Show("Motorsteps updated.");
                motorXStepSize = Math.Max(0, newMotorXStepSize);
                motorXStepDurationms = (int) Math.Max(0, newMotorXStepDurationms);
                motorXTimer.Interval = motorXStepDurationms;
                motorYStepSize = Math.Max(0, newMotorYStepSize);
                motorYStepDurationms = (int)Math.Max(0, newMotorYStepDurationms);
                motorYTimer.Interval = motorYStepDurationms;
            }
            else
            {
                MessageBox.Show("Invalid step values. Please enter valid numbers.");
            }
        }

        private void ApplyCoordinates_Click(object sender, RoutedEventArgs e)
        {
            StopMotors();

            if (double.TryParse(newXTextBox.Text, out double newX) && double.TryParse(newYTextBox.Text, out double newY))
            {
                targetX = newX;
                targetY = newY;

                StartMotors();
            }
            else
            {
                MessageBox.Show("Invalid coordinate values. Please enter valid numbers.");
            }
        }

        private void ApplyPositions_Click(object sender, RoutedEventArgs e)
        {
            StopMotors();

            if (double.TryParse(newXPosTextBox.Text, out double newX) && double.TryParse(newYPosTextBox.Text, out double newY))
            {
                targetX = newX*motorXStepSize;
                targetY = newY*motorYStepSize;

                StartMotors();
            }
            else
            {
                MessageBox.Show("Invalid position values. Please enter valid numbers.");
            }
        }

        private void WorkPane_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StopMotors();

            targetX = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            targetY = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;

            StartMotors();
        }

        private void MoveRectangleTo(double newCoordinate, double currentCoordinate, double animationDuration, DependencyProperty canvasProperty)
        {
            double delta = newCoordinate - currentCoordinate;

            Dispatcher.Invoke(() =>
            {
                DoubleAnimation animation = new DoubleAnimation(newCoordinate, TimeSpan.FromMilliseconds(animationDuration));
                movingRectangle.BeginAnimation(canvasProperty, animation);
            });
        }
    }
}
