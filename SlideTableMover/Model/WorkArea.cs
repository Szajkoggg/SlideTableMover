using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideTableMover.Model
{
    public class WorkArea
    {
        public int height;
        public int width;
        public WorkArea(int height, int width) { this.height = height; this.width = width; }
        public int maxValueOnAxis(Axis axis)
        {
            if(axis.Equals(Axis.X))
            {
                return width;
            }if(axis.Equals(Axis.Y))
            {
                return height;
            }
            return 0;
        }
    }
}
