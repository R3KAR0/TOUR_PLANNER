using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.DAL
{
    public class IOHelper
    {
        public void SaveTourImage(byte?[] image, string name)
        {
            if(image != null)
            {
                byte[] result = Array.ConvertAll(image, x => x ?? 0);
                using (var ms = new MemoryStream(result))
                {
                    using (var fs = new FileStream($"./img/{name}.jpg", FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }
            }
        }
    }
}
