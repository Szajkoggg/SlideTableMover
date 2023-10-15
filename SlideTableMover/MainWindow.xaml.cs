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
        private double motorX = 0;
        private double motorY = 0;
        private double motorStepSize = 0.3;  
        private int motorXDirection = 1;   // Motor X direction (1 for right, -1 for left)
        private int motorYDirection = 1;   // Motor Y direction (1 for down, -1 for up)
        private int motorStepDurationms = 10;
        private Timer motorTimer;
        private double targetX;
        private double targetY;

        public MainWindow()
        {
            InitializeComponent();

            motorTimer = new Timer(motorStepDurationms);
            motorTimer.Elapsed += MotorTimer_Elapsed;
            motorTimer.AutoReset = true;

        }

        private void MotorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
               
            double deltaX = targetX - motorX;
            double deltaY = targetY - motorY;

            if (Math.Abs(deltaX) < motorStepSize && Math.Abs(deltaY) < motorStepSize)
            {
                StopMotor();
                return;
            }

                if (deltaX > 0)
                {
                    motorXDirection = 1;
                }
                else
                {
                    motorXDirection = -1;
                }
                if (deltaY > 0)
                {
                    motorYDirection = 1;
                }
                else
                {
                    motorYDirection = -1;
                }
                if (Math.Abs(deltaX) > motorStepSize)
                {
                    motorX = motorX + motorXDirection * motorStepSize;
                }
                if (Math.Abs(deltaY) > motorStepSize)
                {
                    motorY = motorY + motorYDirection * motorStepSize;
                }
                MoveRectangleTo(motorX, motorY, motorStepDurationms);
                
            
        }
        

        private void StartMotor()
        {
            motorTimer.Start();

        }
        private void StopMotor()
        {
            motorTimer.Stop();
        }
       

        private void ApplySpeed_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(MotorstepTextBox.Text, out double newMotorStepSize) && double.TryParse(MotorstepDurationTextBox.Text, out double newMotorStepDurationms))
            {
                MessageBox.Show("Motorstep updated.");
                motorStepSize = Math.Max(0, newMotorStepSize);
                motorStepDurationms = (int) Math.Max(0, newMotorStepDurationms);
                motorTimer.Interval = motorStepDurationms;

            }
            else
            {
                MessageBox.Show("Invalid step values. Please enter valid numbers.");
            }
        }

        private void ApplyCoordinates_Click(object sender, RoutedEventArgs e)
        {
            StopMotor();

            if (double.TryParse(newXTextBox.Text, out double newX) && double.TryParse(newYTextBox.Text, out double newY))
            {
                MoveRectangleTo(newX, newY, 5);
            }
            else
            {
                MessageBox.Show("Invalid coordinate values. Please enter valid numbers.");
            }
        }

        private void MoveRectangleTo(double newX, double newY, double animationDuration)
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
            bool xAnimationComplete = false;
            bool yAnimationComplete = false;

            Dispatcher.Invoke(() =>
            {
                DoubleAnimation xAnimation = new DoubleAnimation(newX, TimeSpan.FromMilliseconds(animationDuration));
                DoubleAnimation yAnimation = new DoubleAnimation(newY, TimeSpan.FromMilliseconds(animationDuration));
                
                movingRectangle.BeginAnimation(Canvas.LeftProperty, xAnimation);
                movingRectangle.BeginAnimation(Canvas.TopProperty, yAnimation);
                currentXTextBox.Text = newX.ToString();
                currentYTextBox.Text = newY.ToString();
            });
        }

        private void WorkPane_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StopMotor();

            targetX = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            targetY = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;
            
            StartMotor();
        }
    }
}
