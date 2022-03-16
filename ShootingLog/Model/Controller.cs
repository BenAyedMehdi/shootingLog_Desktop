using ShootingLog.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootingLog
{
    class Controller
    {
        private static float threshold = 0.4f;
        public Bitmap image { get; private set; }
        public Bitmap roi { get; private set; }
        public bool isCoardinates { get; private set; }


        public Controller(Bitmap image)
        {
            this.image = image;
            	roi = PrepareImage();
            isCoardinates = IsCoardinates(roi);
        }

        public Bitmap PrepareImage()
        {
            //Prepare image
            this.image = ConvertToGrayScale(image);
            this.image = Threshold(image, threshold);
            //Rotate Image
            RoiDetector roiDetector = new RoiDetector(image);
            Bitmap rotated = roiDetector.RotateImage(roiDetector.Angle());
            rotated.Save("rotated.png", ImageFormat.Png);
            RoiDetector roiDetectorNew = new RoiDetector(rotated);
            //ROI of rotated image
            RectangleF cloneRect = roiDetectorNew.GetRectangle();
            System.Drawing.Imaging.PixelFormat format = image.PixelFormat;
            Bitmap rotatedROI = image.Clone(cloneRect, format);
            //Resize ROI
            Resizer resizer = new Resizer(rotatedROI);
            Bitmap resizedROI = resizer.ResizeImage(1038, 1340);
            resizedROI = Threshold(resizedROI, threshold);
            resizedROI.Save("resizedROI.png", ImageFormat.Png);
            return resizedROI;

        }

        public Bitmap ConvertToGrayScale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                         new float[] {.3f, .3f, .3f, 0, 0},
                         new float[] {.59f, .59f, .59f, 0, 0},
                         new float[] {.11f, .11f, .11f, 0, 0},
                         new float[] {0, 0, 0, 1, 0},
                         new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }

        public Bitmap Threshold(Bitmap image, float threshold)
        {
            Bitmap bm = new Bitmap(image.Width, image.Height);

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetThreshold(threshold);

            System.Drawing.Point[] points =
            {
                new System.Drawing.Point(0, 0),
                new System.Drawing.Point(image.Width, 0),
                new System.Drawing.Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }
            return bm;
        }

        public bool IsCoardinates(Bitmap ROI)
        {
            if (BitmapMethods.isCoardinates(ROI))
            {
                return true ;
            }
            return false;
        }

        public List<Coardinate> ReadCoardinates()
        {
            Extractor extractor = new Extractor(roi);
            return extractor.Coardinates();
        }

        public List<Coardinate> ReadResults()
        {
            Extractor extractor = new Extractor(roi);
            return extractor.Results();
        }
    }
}
