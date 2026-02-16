using System;
using System.Collections.Generic;
using System.Text;

namespace Umbra.School.Models
{
    public class AppSettingModel
    {
        public string Environment { get; set; } = "Development";
        public bool AllowRegistration { get; set; } 
    }
}
