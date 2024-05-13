using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LernSpace.Models
{
    public class TestData
    {
        
         public int Id { get; set; }
        public string UText { get; set; }
        public string EText { get; set; }
        public string Type { get; set; }
        public string PicPath { get; set; }
        public string CGroup { get; set; }
        public string AudioPath { get; set; }
        public string QuestionTitle { get; set; }
        public int Op1 { get; set; }
        public int Op2 { get; set; }
        public int Op3 { get; set; }
        public string Title { get; set; }
        public int TestId { get; set; }
        public string Op1ImagePath { get; set; }
        public string Op2ImagePath { get; set; }
        public string Op3ImagePath { get; set; }
        public bool flag { get; set; }
    }
}
