namespace SlideTableMover.Model
{
    public class WorkArea
    {
        public int xSize;
        public int ySize;
        public int zSize;
        public WorkArea(int ySize, int xSize, int zSize) { this.ySize = ySize; this.xSize = xSize; this.zSize = zSize; }
        public int maxValueOnAxis(Axis axis)
        {
            if(axis == (Axis.X))
            {
                return xSize;
            }if(axis == (Axis.Y))
            {
                return ySize;
            }if(axis == Axis.Z) 
            { 
                return zSize;
            }
            return 0;
        }
    }
}
