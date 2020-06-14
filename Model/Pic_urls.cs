using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    
    public class Pic_urls
    {
        public List<string> thumbnail_pic { get; set; }
        public Pic_urls()
        {
            thumbnail_pic = new List<string>();
        }
    }
}
