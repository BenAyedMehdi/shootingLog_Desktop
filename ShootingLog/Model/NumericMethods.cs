using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    static class NumericMethods
    {
         public static Coardinate lineToPoint(string line)
        {
            string[] parts = line.Trim().Split(',');
            return new Coardinate(Int16.Parse(parts[0].Trim()), Int16.Parse(parts[1].Trim()) );
        }
    }
}
