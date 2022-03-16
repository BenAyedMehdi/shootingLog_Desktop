using ShootingLog.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog
{
    class Recognizer
    {
        public string type { get; private set; }

        public Templates Templates;
        
        public Recognizer(string type)
        {
            this.type = type;

            this.Templates = new Templates(type);
        }

        public char CompareToTemplates(Bitmap extracted)
        {
            
            int[] matches = new int[Templates.Count];
            for (int i = 0; i < Templates.Count; i++)
            {
                matches[i] = GetMatchRate(Templates[i], extracted);
            }
            int max = matches.Max();

            if (matches[11] == max)
            {
                return ' ';
            }
            int pos = Array.IndexOf(matches, max);

            return ArrayValue(pos);
        }

        private int GetMatchRate(Bitmap template, Bitmap extracted)
        {
            int countBlack = 0;
            for (int x = 0; x < template.Width; x++)
            {
                for (int y = 0; y < template.Height; y++)
                {
                    if (BitmapMethods.PointIsBlack(x, y, template) && BitmapMethods.PointIsBlack(x, y, extracted))
                    {
                        countBlack++;
                    }
                }
            }
            return countBlack;
        }

        private char ArrayValue(int pos)
        {
            switch (pos)
            {
                case 0: return '0';
                case 1: return '1';
                case 2: return '2';
                case 3: return '3';
                case 4: return '4';
                case 5: return '5';
                case 6: return '6';
                case 7: return '7';
                case 8: return '8';
                case 9: return '9';
                case 10: return '-';
                case 11: return ' ';
                case 12: return ':';
                case 13: return '.';

            }
            return 'n';
        }
    }
}
