using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUploaderAndExporter.Models
{
    public class FileDetails
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
    }
}