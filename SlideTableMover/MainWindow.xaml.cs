using SlideTableMover.Model;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SlideTableMover
{

    public partial class MainWindow : Window
    {
        private Motor motorX;
        private Motor motorY;
        private Motor motorZ;
        private MovementService movementService;
        private SlideTable slideTable;
        private WorkArea workArea;
        private bool isMotorRunning = false;


        public MainWindow()
        {;
            InitializeComponent();
            workArea = new WorkArea(450, 800, 100);
            slideTable = new SlideTable(50, 50, slideTableElement, currentXTextBox, currentYTextBox);
            movementService = new MovementService(slideTable, workArea, Dispatcher, this);

            motorX = new Motor(0.3, 10, Axis.X, movementService, xPosTextBox);
            motorY = new Motor(0.5, 10, Axis.Y, movementService, yPosTextBox);
            motorZ = new Motor(0.5, 10, Axis.Z, movementService, zPosTextBox);

            slideTableElement.Width = slideTable.xSize;
            slideTableElement.Height = slideTable.ySize;
        }
        
        
        private void StartMotors()
        {
            motorX.Start();
            motorY.Start();
            motorZ.Start();

        }
        private void StopMotors()
        {
            motorX.Stop();
            motorY.Stop();
            motorZ.Stop();
        }
       

        private void ApplySpeed_Click(object sender, RoutedEventArgs e)
        {
            
            if (double.TryParse(MotorXStepTextBox.Text, out double newMotorXStepSize) && double.TryParse(MotorXStepDurationTextBox.Text, out double newMotorXStepDurationms) && double.TryParse(MotorYStepTextBox.Text, out double newMotorYStepSize) && double.TryParse(MotorYStepDurationTextBox.Text, out double newMotorYStepDurationms))
            {

                StopMotors();
                CheckCurrentCoordinates();
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

            if (double.TryParse(newXTextBox.Text, out double newX) && double.TryParse(newYTextBox.Text, out double newY) && double.TryParse(newZTextBox.Text, out double newZ))
            {
                CheckCurrentCoordinates();
                motorX.target = newX;
                motorY.target = newY;
                motorZ.target = newZ;
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
                CheckCurrentCoordinates();
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
            CheckCurrentCoordinates();
            motorX.target = e.GetPosition(canvas).X - slideTableElement.ActualWidth / 2;
            motorY.target = e.GetPosition(canvas).Y - slideTableElement.ActualHeight / 2;

            StartMotors();
        }

        private void CheckCurrentCoordinates()
        {
            Dispatcher.Invoke(() =>
            {
                slideTable.xCoordinate = Canvas.GetLeft(slideTable.element);
                slideTable.yCoordinate = Canvas.GetTop(slideTable.element);
            });
            motorX.currentCoordinate = slideTable.xCoordinate;
            motorY.currentCoordinate = slideTable.yCoordinate;
        }
        private void MoveXPlusButton_MouseDown(object sender, RoutedEventArgs e)
        {
            if(!isMotorRunning)
            {
                motorX.target = workArea.xSize;
                StartMotors();
                isMotorRunning = true;
            }
            
        }
        private void MoveXMinusButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hallo");
            if (!isMotorRunning)
            {
                motorX.target = 0;
                StartMotors();
                isMotorRunning = true;
            }
        }
        private void MoveXMotorButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            motorX.Stop();
            isMotorRunning = false;
        }
        private void MoveYPlusButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            motorY.target = workArea.ySize;
            motorY.Start();
        }
        private void MoveYMinusButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            motorY.target = 0;
            motorY.Start();
        }
        private void MoveYMotorButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            motorY.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StopMotors();
        }
    }
}
