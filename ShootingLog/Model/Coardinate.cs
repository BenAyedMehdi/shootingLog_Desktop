using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    class Coardinate
    {
        public Single x { get; set; }

        //[ColumnName("Label")]
        public Single y { get; set; }

        public Coardinate(int x, float y)
        {
            this.x = x;
            this.y = (Single)y;
        }

        public override string ToString()
        {
            return x+" "+y;
        }
    }
}
