using ShootingLog.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootingLog
{
    class Extractor
    {
        //Calculated from c1
        const int WidthRatio= 1035;
        const int HeightRatio = 1337;

        public List<int> XCoordinates;
        public List<int> YCoordinates;
        public List<int> XBoldCoordinates;

        public List<Coardinate> points;

        public Bitmap ROI { get; private set; }

        public Extractor( Bitmap roi)
        {
            this.ROI = roi;
            XCoordinates = new List<int>();
            YCoordinates = new List<int>();
            XBoldCoordinates = new List<int>();
            LoadYs();
        }
        public List<Coardinate> Coardinates()
        {
            points = new List<Coardinate>();
           string s = BoldChars() + "\n\n" + NormalChars();

            using (StreamWriter writer = new StreamWriter(@"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\coardinates.txt"))
            {
                foreach (Coardinate point in points)
                {
                    writer.WriteLine(point);
                }
            }

            return points;
        }
        public List<Coardinate> Results()
        {
            points = new List<Coardinate>();
            string s = BoldResults() + "\n\n" + NormalResults();
            using (StreamWriter writer = new StreamWriter(@"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\results.txt"))
            {
                foreach (Coardinate r in points)
                {
                    writer.WriteLine(r);
                }
            }

            return points;
        }

        private string BoldResults()
        {
            Recognizer recognizer = new Recognizer("bold");
            string message = "";
            char bestMatch;
            XDetector xDetector = new XDetector(ROI, 38, "bold","r");
            XCoordinates = xDetector.coordinates;
            for (int x = 0; x < XCoordinates.Count; x++)
            {
                if (x == 2)
                {
                    message = message + ".";
                }
                else
                {
                    bestMatch = recognizer.CompareToTemplates(Extract(XCoordinates[x], 38, "bold"));
                    message = message + bestMatch;
                }
            }
            points.Add(new Coardinate(1,float.Parse(message)));
            return message;
        }

        public string BoldChars()
        {
            Recognizer recognizer = new Recognizer("bold");
            string message = "";
            char bestMatch;
            XDetector xDetector = new XDetector(ROI, 38, "bold","c");
            XCoordinates = xDetector.coordinates;
            for (int x = 0; x < XCoordinates.Count; x++)
            {
                if (x == 4)
                {
                    message = message + ",";
                }
                bestMatch = recognizer.CompareToTemplates(Extract(XCoordinates[x], 38, "bold"));
                message = message + bestMatch;
            }
            points.Add(NumericMethods.lineToPoint(message));
            return message;
        }

        private string NormalResults()
        {
            Recognizer recognizer = new Recognizer("normal");
            string message = "";
            string line = "";
            char bestMatch;

            for (int y = 0; y < YCoordinates.Count; y++)
            {
                XDetector xDetector = new XDetector(ROI, YCoordinates[y], "normal","r");
                XCoordinates = xDetector.coordinates;

                for (int x = 0; x < XCoordinates.Count; x++)
                {
                    if (x == 2)
                    {
                        line = line + ".";
                    }
                    else
                    {
                        bestMatch = recognizer.CompareToTemplates(Extract(XCoordinates[x], YCoordinates[y], "normal"));
                        line = line + bestMatch;
                    }
                }
                message = message + line +  "\n";
                points.Add(new Coardinate(y+2,float.Parse(line)));
                line = "";
            }
            return message;
        }

        public string NormalChars()
        {

            Recognizer recognizer = new Recognizer("normal");
            string message = "";
            string line = "";
            char bestMatch;

            for (int y = 0; y < YCoordinates.Count; y++)
            {
                XDetector xDetector = new XDetector(ROI, YCoordinates[y],"normal","c");
                XCoordinates = xDetector.coordinates;

                for (int x = 0; x < XCoordinates.Count; x++)
                {
                    if (x == 4)
                    {
                        line = line + ",";
                    }
                    bestMatch = recognizer.CompareToTemplates(Extract(XCoordinates[x], YCoordinates[y], "normal"));
                    line = line + bestMatch;
                }
                points.Add(NumericMethods.lineToPoint(line));
                message = message+line+ "\n";
                line = "";
            }

            
            return message;
        }

        public Bitmap Extract(int x, int y, string type)//use point instead of x,y, use enum not string (size)
        {
            int height, width;

            if (type == "bold")//x is from 0 to 7 and y is always 38
            {
                height = 100;
                width = 60;
            }
            else // both x and y are the real coordinates
            {
                height = 78;
                width = 40;
            }
            Bitmap extracted = ROI.Clone(new Rectangle(x, y, width, height), ROI.PixelFormat);
            Adjust adjust = new Adjust(extracted);
            extracted = ROI.Clone(new Rectangle(x - adjust.Horizental(), y - adjust.Vertical(), width, height), ROI.PixelFormat);
            return extracted;
        }

        private void LoadYs()
        {
            YCoordinates.Add((int)(171 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(249 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(327 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(405 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(483 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(561 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(639 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(717 * ROI.Height / HeightRatio));
            YCoordinates.Add((int)(795 * ROI.Height / HeightRatio));
        }
    }
}
