﻿using System;
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

            double deltaX = targetX - motorXCoordinate;
            double deltaY = targetY - motorYCoordinate;

            if (Math.Abs(deltaX) < motorStepSize && Math.Abs(deltaY) < motorStepSize)
            {
                StopMotor();
                return;
            }
            motorXDirection = deltaX > 0 ? 1 : -1;
            motorYDirection = deltaY > 0 ? 1 : -1;

            if (Math.Abs(deltaX) > motorStepSize)
                {
                    double newX = motorXCoordinate + motorXDirection * motorStepSize;
                Dispatcher.Invoke(() =>
                {
                    if (newX >= 0 && newX <= map.Width - movingRectangle.Width)
                    {
                        motorX = motorX + motorXDirection;
                        motorXCoordinate = newX;
                    }

                });
                }
                if (Math.Abs(deltaY) > motorStepSize)
                {
                    double newY = motorYCoordinate + motorYDirection * motorStepSize;
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
                currentX = Canvas.GetLeft(movingRectangle);
                currentY = Canvas.GetTop(movingRectangle);
            });
            MoveRectangleTo(motorXCoordinate, currentX, motorStepDurationms, Canvas.LeftProperty);
            MoveRectangleTo(motorYCoordinate, currentY, motorStepDurationms, Canvas.TopProperty);

            Dispatcher.Invoke(() =>
            {
                xPosTextBox.Text = motorX.ToString();
                yPosTextBox.Text = motorY.ToString();
                currentXTextBox.Text = motorXCoordinate.ToString();
                currentYTextBox.Text = motorYCoordinate.ToString();
            });

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
                StopMotor();
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
                targetX = newX;
                targetY = newY;

                StartMotor();
            }
            else
            {
                MessageBox.Show("Invalid coordinate values. Please enter valid numbers.");
            }
        }

        private void ApplyPositions_Click(object sender, RoutedEventArgs e)
        {
            StopMotor();

            if (double.TryParse(newXPosTextBox.Text, out double newX) && double.TryParse(newYPosTextBox.Text, out double newY))
            {
                targetX = newX*motorStepSize;
                targetY = newY*motorStepSize;

                StartMotor();
            }
            else
            {
                MessageBox.Show("Invalid position values. Please enter valid numbers.");
            }
        }

        private void WorkPane_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StopMotor();

            targetX = e.GetPosition(canvas).X - movingRectangle.ActualWidth / 2;
            targetY = e.GetPosition(canvas).Y - movingRectangle.ActualHeight / 2;
            
            StartMotor();
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
