using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Core.Model
{
    public abstract class BaseRequestDto
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
