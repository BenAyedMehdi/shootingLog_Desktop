using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog
{
    class Adjust
    {
        Bitmap extracted;
        int highest, lowest, topRight, topLeft;

        public Adjust(Bitmap extracted)
        {
            this.extracted = extracted;
            highest = Highest();
            lowest = Lowest();
            topRight = TopRight();
            topLeft = TopLeft();
        }

        public int Vertical()
        {
            int low = extracted.Height - lowest;
            int averageInBothSides = (low + highest) / 2;
            return averageInBothSides - highest;
        }

        public int Horizental()
        {
            int toRight = extracted.Width - topRight;
            int averageInBothSides = (toRight + topLeft) / 2;
            return averageInBothSides - topLeft;
        }

        public int Highest()
        {
            Color color;
            for (int y = 0; y < extracted.Height-1; y++)
            {
                for (int x = 0; x < extracted.Width - 1; x++)
                {
                    color = extracted.GetPixel(x, y);
                    if (color.GetBrightness() < 0.1)
                    {
                        return y;
                    }
                }
            }
            return 0;
        }

        public int Lowest()
        {
            Color color;
            for (int y = extracted.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < extracted.Width - 1; x++)
                {
                    color = extracted.GetPixel(x, y);
                    if (color.GetBrightness() < 0.1)
                    {
                        return y;
                    }
                }
            }
            return extracted.Height;
        }

        internal int TopRight()
        {
            Color color;
            for (int x = extracted.Width - 1; x >=0; x--)
            {
                for (int y = 0; y< extracted.Height - 1; y++)
                {
                    color = extracted.GetPixel(x, y);
                    if (color.GetBrightness() < 0.1)
                    {
                        return x;
                    }
                }
            }
            return extracted.Width;
        }

        internal int TopLeft()
        {
            Color color;
            for (int x = 0; x <extracted.Width - 1; x++)
            {
                for (int y = 0; y < extracted.Height - 1; y++)
                {
                    color = extracted.GetPixel(x, y);
                    if (color.GetBrightness() < 0.1)
                    {
                        return x;
                    }
                }
            }
            return 0;
        }
    }
}
