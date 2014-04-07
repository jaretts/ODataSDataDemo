using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    public class SDataDiagnosis
    {
        public string severity { get; set; }
        public string sdataCode { get; set; }
        public string applicationCode { get; set; }
        public string message { get; set; }
        public string stackTrace { get; set; }
    }
}
