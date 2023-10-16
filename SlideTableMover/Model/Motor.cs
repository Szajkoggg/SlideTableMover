using System;
using System.Timers;
using System.Windows.Controls;

namespace SlideTableMover.Model
{
    public class Motor
    {
        private double currentPosition = 0;
        public double currentCoordinate = 0;
        public double stepSize;
        public int stepDurationMs;
        private int direction = 1;
        public double target = 0;
        private Axis axis;
        private Timer motorTimer;
        private MovementService service;
        private TextBox textBox;


        public Motor(double stepSize, int stepDurationMs, Axis axis, MovementService service, TextBox textBox)
        {
            this.stepDurationMs = Math.Max(1, stepDurationMs);
            this.stepSize = stepSize;
            motorTimer = new Timer(stepDurationMs);
            motorTimer.Elapsed += MotorTimer_Elapsed;
            motorTimer.AutoReset = true;
            this.axis = axis;
            this.service = service;
            this.textBox = textBox;
        }
        public void updateSizeAndDuration(double stepSize, double stepDurationMs)
        {
            this.stepSize = Math.Max(0, stepSize);
            this.stepDurationMs = (int) stepDurationMs;
            motorTimer.Interval = stepDurationMs;
            currentPosition = currentCoordinate / stepSize;
            service.updateMotorPosTextBox(textBox, currentCoordinate.ToString());
        }
        public double GetPosition()
        {
            return currentPosition;
        }public double GetCoordinate()
        {
            return currentCoordinate;
        }

        public void Start()
        {
            motorTimer.Start();
        }

        public void Stop()
        {
            motorTimer.Stop();
        }

        private void MotorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double delta = target - currentCoordinate;
            if (Math.Abs(delta) < stepSize)
            {
                Stop();
                return;
            }
            direction = delta > 0 ? 1 : -1;

            double newCoordinate = currentCoordinate + direction * stepSize;  
                    if (service.motorCanMoveTo(newCoordinate, axis))
                    {
                        currentPosition = currentPosition + direction;
                        currentCoordinate = newCoordinate;
                    }

            service.moveSlideTable(axis, currentCoordinate, stepDurationMs);
            service.updateMotorPosTextBox(textBox, currentPosition.ToString());
            
        }
    }
}
