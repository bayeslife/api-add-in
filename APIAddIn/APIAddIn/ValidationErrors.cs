using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAddIn
{
    public class ValidationErrors
    {

        public List<string> messages = new List<string>();

        public void add(String validation)
        {
            messages.Add(validation);            
        }


        public Boolean hasAny()
        {
            return messages.Count > 0;
        }
        
    }
}
