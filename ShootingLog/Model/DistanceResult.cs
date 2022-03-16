using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    class DistanceResult: IComparable
    {
        public float x { get; set; }
        public float y { get; set; }
        public float distance { get; set; }
        public float result { get; set; }

        public DistanceResult(float x, float y, float result)
        {
            this.x = x;
            this.y = y;
            this.result = result;
            this.distance = Distance();
        }

        private float Distance()
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        public string ToLine()
        {
            return ((int)x).ToString() + ";" + ((int)y).ToString() +";"+ ((int)distance).ToString()+";" + result;
        }

        public override string ToString()
        {
            return ((int)distance).ToString() + " -> " + result.ToString();
        }

        public int CompareTo(object obj)
        {
            return (int)(this.distance - ((DistanceResult)obj).distance);
        }
    }
}
