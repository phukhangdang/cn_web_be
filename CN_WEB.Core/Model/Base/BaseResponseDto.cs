using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Core.Model
{
    public abstract class BaseResponseDto
    {
        public int? TotalItem { get; set; }
    }

    public abstract class BaseResponseDto<T>
    {
        public int? TotalItem { get; set; }
        public T DataSource { get; set; }
    }
}
