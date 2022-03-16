using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    static class BitmapMethods
    {
        public static bool PointIsBlack(int x, int y, Bitmap bitmap)
        {
            try
            {
                Color color = bitmap.GetPixel(x, y);
                if (color.GetBrightness() < 0.1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
            /*if(x>0 &&y>0 && x<bitmap.Width && y < bitmap.Height)
            {
                Color color = bitmap.GetPixel(x, y);
                if (color.GetBrightness() < 0.1)
                {
                    return true;
                }
                return false;
            }
            return false;
            */
        }

        public static bool verticalLineIsBlank(int x , int minY, int extra, Bitmap image)
        {
            int maxY = minY + extra -20;
            for (int y = minY; y < maxY; y++)
            {
                if (BitmapMethods.PointIsBlack(x, y, image))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool horizentalLineIsBlank(int y, int minx, int maxx, Bitmap image)
        {
            for (int x = minx; x < maxx; x++)
            {
                if (BitmapMethods.PointIsBlack(x, y, image))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isCoardinates(Bitmap roi)
        {
            if (horizentalLineIsBlank(1150,400,670,roi))
            {
                return true;
            }
            return false;
        }

    }
}
