using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridProcessor.Configuration
{
    public class EmailRule
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string AttachmentName { get; set; }
        public string FolderName { get; set; }
    }

}
