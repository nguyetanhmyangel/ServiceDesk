using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.ViewModels
{
    public class MailSettings
    {
        public string Mail { get; set; } //ex: itcenter.vietsov@gmail.com       
        public string DisplayName { get; set; } // itcenter
        public string Password { get; set; }       
        public string Host { get; set; } //smtp.gmail.com       
        public int Port { get; set; } //587
    }
}
