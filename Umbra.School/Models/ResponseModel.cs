using System;
using System.Collections.Generic;
using System.Text;

namespace Umbra.School.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
