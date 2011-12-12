using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities.Extensions;

namespace ITS.Domain.Helpers
{
    public static class GeometryHelper
    {
        public static Point GetPointFromGoogleMapPosition(string text)
        {
            Point p = new Point();
            string[] str = text.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            p.lat = float.Parse(str[0]);
            p.lng = float.Parse(str[1]);
            return p;
        }
    }
}
