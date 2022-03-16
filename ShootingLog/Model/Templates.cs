using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootingLog
{
    class Templates
    {
        public int templatesCount { get; private set; }
        public int height { get; private set; }
        public int width { get; private set; }
        public string type { get; private set; }
        public string fileName { get; private set; }
        public Bitmap extracted { get; private set; }

        public List<Bitmap> templates;

        public Bitmap this[int index] => templates[index];
        public int Count => templates.Count;
        public Templates(string type)
        {
            this.type = type;
            templates = new List<Bitmap>();

            if (type.Equals( "bold"))
            {
                this.fileName = @"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\boldchars.png";
                this.height = 100;
                this.width = 60;
                this.templatesCount = 14;
            }
            else
            {
                this.fileName = @"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\chars.png";
                this.height = 78;
                this.width = 40;
                this.templatesCount = 12;
            }
            ExtractTemplates();
        }

        public void ExtractTemplates()
        {
            if (fileName!=null)
            {
                Bitmap chars = new Bitmap(fileName);
                for (int i = 0; i < templatesCount; i++)
                {
                    templates.Add(chars.Clone(new Rectangle(i * width, 0, width, height), chars.PixelFormat));
                }
            }
            
        }
    }
}
