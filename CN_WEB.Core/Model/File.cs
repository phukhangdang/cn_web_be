using System;
using System.Collections.Generic;

namespace CN_WEB.Core.Model
{
    public partial class File
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocationPath { get; set; }
        public string Extension { get; set; }
        public float? Size { get; set; }
        public string Module { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
