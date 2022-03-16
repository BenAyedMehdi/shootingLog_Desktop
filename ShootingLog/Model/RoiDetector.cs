using System.Numerics;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Windows.Point;



namespace ShootingLog.Model
{
    class RoiDetector
    {
        public double ratio { get; private set; }
        public double length { get; set; }
        public int width { get; set; }
        public int top { get; private set; }
        public int right { get; private set; }
        public int down { get; private set; }
        public int left { get; private set; }

        Bitmap pic;

        public RoiDetector(Bitmap pic)
        {
            this.ratio = (double)1338 / 1036;
            this.pic = pic;
            right = Right();
            top = Top();
            down = Down();
            left = Left();
            this.length = VectorLength();
            this.width = (int)(length / ratio);
        }
        public double VectorLength()
        {
            return LowRight().Y - TopRight().Y;
        }

        public Bitmap RotateImage(float angle)
        {
            Bitmap rotatedImage = new Bitmap(pic.Width, pic.Height);
            rotatedImage.SetResolution(pic.HorizontalResolution, pic.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(pic.Width / 2, pic.Height / 2);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-pic.Width / 2, -pic.Height / 2);
                // Draw the image on the bitmap
                g.DrawImage(pic, new PointF(0, 0));
            }

            return rotatedImage;
        }
        public float Angle()
        {
            var vector = LowRight() - TopRight();
            double opposite = Math.Abs(LowRight().X - TopRight().X);
            double addjacent = LowRight().Y - TopRight().Y;
            double angleTan = opposite / addjacent;
            float result = (float)(Math.Atan2(vector.X, vector.Y) * (180 / Math.PI));
            return result;
        }

        public Point TopRight()//returns top Y
        {
            int x = right - 10;
            int y = pic.Height / 4*3;
            while (y > 0)
            {
                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return GoIfPossible(x, y, pic, 1);
                }
                y--;
            }
            return new Point { X = pic.Width, Y = 0 };
        }
        public Point LowRight()//returns lower Y
        {
            int x = right - 15;
            int y = pic.Height / 4 * 3;
            while (y < pic.Height)
            {
                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return GoIfPossible(x, y, pic, -1);
                }
                y++;
            }
            return new Point { X = pic.Width, Y = pic.Height };
        }
        private Point GoIfPossible(int x, int y, Bitmap pic, int sign)
        {
            y = y + 5 * sign;
            while (!BitmapMethods.PointIsBlack(x, y, pic))
            {
                x++;
            }
            x--;
            while (!BitmapMethods.PointIsBlack(x, y, pic))
            {
                y = y - 1 * sign;
            }

            return new Point { X = x + 1, Y = y };
        }

        public RectangleF GetRectangle()
        {
            int x1 = (int)(TopRight().X) - width, y1 = (int)(TopRight().Y), x2 = (int)(TopRight().X),
                y2 = (int)(LowRight().Y);
            x2 = right;
            x2--;
            y2 = pic.Height / 2;
            while (y1>0 && !BitmapMethods.PointIsBlack(x2, y1, pic))
            {
                y1--;
            }
            x2 = right + 2;
            y1++;
            x1 = x2 - width;
            y2 = y1 + (int)length;
            return new RectangleF(x1 - 2, y1 - 3, x2 - x1 + 3, y2 - y1 + 3);

        }
        private int Left()
        {
            int y = HorizentalWhiteLine();
            int x = pic.Width /4*3;
            while (x > 0)
            {
                x--;
                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return x + 1;
                }
            }
            return 0;
        }

        private int Down()
        {
            int x = right - 15;
            int y = pic.Height / 4*3;
            while (y < pic.Height)
            {

                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return y - 1;
                }
                y++;
            }
            return pic.Height - 1;
        }

        private int Top()//returns top Y
        {
            int x = right - 15;
            int y = pic.Height / 4*3;
            while (y > 0)
            {
                y--;
                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return y + 1;
                }
            }
            return 0;
        }

        private int Right()
        {
            int y = HorizentalWhiteLine()-5;
            int x = pic.Width / 2;
            while (x < pic.Width)
            {
                if (BitmapMethods.PointIsBlack(x, y, pic))
                {
                    return x - 1;
                }
                x++;
            }
            return pic.Width - 1;
        }//returns the right side X

        public int HorizentalWhiteLine()//return Y of empty line
        {
            int startX = pic.Width / 5;
            int endX = pic.Width - startX;
            int x, y = pic.Height /4* 3;
            bool blackCrossed = false;
            while (y < pic.Height)
            {
                y++;
                x = startX;
                while (!blackCrossed && x < endX)
                {
                    x++;
                    if (BitmapMethods.PointIsBlack(x, y, pic))
                    {
                        return y;
                    }
                }
            }
            return 0;
        }

        

    }
}
