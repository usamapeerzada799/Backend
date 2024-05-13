using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LernSpace.Models
{
    public class PracticeData
    {
        public int id { get; set; }
        public string uText { get; set; }
        public string eText { get; set; }
        public string type { get; set; }
        public string picPath { get; set; }
        public string C_group { get; set; }
        public string audioPath { get; set; }
        public string title { get; set; }
        public int pracId { get; set; }
        // Add your new member here
        public bool flag { get; set; }
    }
}