using System.Drawing;

namespace Visualizer
{
    class Graphic
    {
        public int maxX;
        public int maxY;
        public int minX;
        public int minY;
        public Bitmap graphic;

        public Graphic(int maxX, int maxY, int minX, int minY, Bitmap graphic)
        {
            this.maxX = maxX;
            this.maxY = maxY;
            this.minX = minX;
            this.minY = minY;
            this.graphic = graphic;
    }
    }
}
