using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    class XDetector
    {
        public Bitmap image { get; private set; }
        public int y { get; private set; }
        public int distance { get; private set; }

        public int hight { get; private set; }
        
        public int width { get; private set; }
        public List<int> coordinates { get; private set; }
        public List<int> stratingXs { get;  set; }

        public XDetector( Bitmap image, int y, string size ,string type)
        {
            this.image = image;
            this.y = y;
            this.coordinates = new List<int>();
            this.stratingXs = new List<int>();
            if (size.Equals("bold"))
            {
                if (type.Equals("c"))
                {
                    stratingXs.Add(340);//For coordinates//290,300,337
                    stratingXs.Add(670);//For coordinates//625,640,660,
                }else
                    stratingXs.Add(550);//For results//513;526

                this.distance = 223;
                this.width = 60;
                this.hight = 78;
            }
            else
            {
                if (type.Equals("c"))
                {
                    stratingXs.Add(530);//For coordinates//507,503,519
                    stratingXs.Add(760);//For coordinates//740,734,750
                }
                else
                    stratingXs.Add(710);//For results//693,700

                this.distance = 149;
                this.width = 40;
                this.hight = 78;
            }
            for (int i = 0; i < stratingXs.Count; i++)
            {
                getCoordinates(stratingXs[i],distance);
            }
        }

        private void getCoordinates(int startingX, int distance)
        {
            int xToAdd;
            int firstBlack = FirstBlackPixel(startingX, y, image) ;
            startingX = firstBlack - distance;
            for (int i = 0; i < 4; i++)
            {
                xToAdd = CheckXToAdd(startingX + i * width);
                coordinates.Add(xToAdd);
                //coordinates.Add(startingX + i * width);
            }
        }
        private int FirstBlackPixel(int startingX, int minY, Bitmap pic)
        {
            minY += 20;
            while (BitmapMethods.verticalLineIsBlank(startingX, minY, hight, pic))
            {
                startingX--;
            }
            return startingX;
        }
        private int CheckXToAdd(int xToAdd)
        {
            int middle = MiddleVerticalLine(xToAdd);
            while (MiddleVerticalLine(xToAdd) != -1)
            {
                xToAdd--;
            }
            return xToAdd;
        }
        private int MiddleVerticalLine(int x)
        {
            Bitmap extracted = image.Clone(new Rectangle(x, y, width, hight), image.PixelFormat);
            extracted.Save("extracted.png", ImageFormat.Png);
            if (BitmapMethods.verticalLineIsBlank(1,0,hight,extracted)&& BitmapMethods.verticalLineIsBlank(width-1, 0, hight, extracted))
            {
                return -1;
            }
            return 0;
            /*
            int first = FirstBlackPixel(width-1, 0, extracted);
            while (first > 1 && !BitmapMethods.verticalLineIsBlank(first,0,hight,extracted))
            {
                first--;
            }
            return first;
            */
        }
        
    }
}
