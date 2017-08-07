using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest.Core
{
    public class ValidationStatus
    {
        public ValidationStatus()
        {
            Messages = new List<string>();
        }

        public bool Validated
        {
            get
            {
                return Messages.Count() == 0;
            }
        }

        public List<string> Messages { get; set; }
    }
}
