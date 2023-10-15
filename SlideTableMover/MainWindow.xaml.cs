using SlideTableMover.Model;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SlideTableMover
{

    public partial class MainWindow : Window
    {
        private Motor motorX;
        private Motor motorY;
        private MovementService movementService;
        private SlideTable slideTable;
        private WorkArea workArea;


        public MainWindow()
        {;
            InitializeComponent();
            workArea = new WorkArea(450, 800);
            slideTable = new SlideTable(50, 50, slideTableElement, currentXTextBox, currentYTextBox);
            movementService = new MovementService(slideTable, workArea, Dispatcher);

            motorX = new Motor(0.3, 10, Axis.X, movementService, xPosTextBox);
            motorY = new Motor(0.5, 10, Axis.Y, movementService, yPosTextBox);

            slideTableElement.Width = slideTable.width;
            slideTableElement.Height = slideTable.height;
        }
        
        
        private void StartMotors()
        {
            motorX.Start();
            motorY.Start();

        }
        private void StopMotors()
        {
            motorX.Stop();
            motorY.Stop();
        }
       

        private void ApplySpeed_Click(object sender, RoutedEventArgs e)
        {
            
            if (double.TryParse(MotorXStepTextBox.Text, out double newMotorXStepSize) && double.TryParse(MotorXStepDurationTextBox.Text, out double newMotorXStepDurationms) && double.TryParse(MotorYStepTextBox.Text, out double newMotorYStepSize) && double.TryParse(MotorYStepDurationTextBox.Text, out double newMotorYStepDurationms))
            {
                StopMotors();
                motorX.updateSizeAndDuration(newMotorXStepSize, newMotorXStepDurationms);
                motorY.updateSizeAndDuration(newMotorYStepSize, newMotorYStepDurationms);
                MessageBox.Show("Motorsteps updated.");
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
                motorX.target = newX;
                motorY.target = newY;

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
                motorX.target = newX*motorX.stepSize;
                motorY.target = newY*motorY.stepSize;

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

            motorX.target = e.GetPosition(canvas).X - slideTableElement.ActualWidth / 2;
            motorY.target = e.GetPosition(canvas).Y - slideTableElement.ActualHeight / 2;

            StartMotors();
        }
    }
}
