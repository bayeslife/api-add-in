using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAddIn
{
    public class Logger
    {
        
        bool toggle = false;//false

        EA.Repository repository = null;

        public void setRepository(EA.Repository r){
            this.repository = r;
        }

        public void toggleLogging(EA.Repository r)
        {
            this.toggle = !this.toggle;
            this.repository = r;
            if (this.toggle)
            {
                enable(r);                            
            }
                
        }

        public void enable(EA.Repository r)
        {
            this.toggle = true;
            this.repository = r;
            if (this.toggle)
            {
                repository.CreateOutputTab("IAG");
                repository.EnsureOutputVisible("IAG");
                repository.ClearOutput("IAG");
                log("Logger is enabled");
            }
        }

        public void log(string msg)
        {
            if (toggle)
                repository.WriteOutput("IAG", msg, 0);
        }
    }

   
}
