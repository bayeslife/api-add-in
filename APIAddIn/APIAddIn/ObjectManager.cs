using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace APIAddIn
{
    public class RunState
    {
        public string key;
        public string value;
        public string reference;
    }
    /* This class deals with serialization of UML Object diagrams to Json */
    public class ObjectManager
    {


        static public Dictionary<string, RunState> parseRunState(String runstate)
        {

            Dictionary<string, RunState> result = new Dictionary<string, RunState>();
            if (runstate == null || runstate.Length == 0)
            {
                return result;
            }

            //string runstatePattern = "@VAR;Variable=(.+);Value=(.+);Note=(.+);Op=(.+);@ENDVAR;";
            string runstatePattern = @"Variable=(?<var>[^;]*);Value=(?<val>[^;]*);Note=(?<note>[^;]*)";
            
            Match m = Regex.Match(runstate, runstatePattern, RegexOptions.IgnoreCase);            
            while (m.Success)
            {
                string variable = m.Result("${var}");
                string value = m.Result("${val}");
                string note = m.Result("${note}");
                if (!result.ContainsKey(variable))
                {
                    RunState rsi = new RunState();
                    rsi.key = variable;
                    rsi.value = value;
                    rsi.reference = note;
                    result.Add(variable, rsi);
                }
                    
                m = m.NextMatch();
            }

            string runstatePattern2 = @"Variable=(?<var>[^;]*);Value=(?<val>[^;]*);Op=(?<op>[^;]*)";
            m = Regex.Match(runstate, runstatePattern2, RegexOptions.IgnoreCase);
            while (m.Success)
            {
                string variable = m.Result("${var}");
                string value = m.Result("${val}");                
                if (!result.ContainsKey(variable))
                {
                    RunState rsi = new RunState();
                    rsi.key = variable;
                    rsi.value = value;                    
                    result.Add(variable, rsi);
                }
                m = m.NextMatch();
            }
            return result;
        }

        public static string renderRunState(Dictionary<string, RunState> values)
        {
            string result = "";
            foreach (string key in values.Keys)
            {
                RunState rs = values[key];
                result += "@VAR;Variable=" + rs.key + ";Value=" + rs.value + ";Note=" + rs.reference + ";Op==;@ENDVAR;";
            }
            return result;
        }
        public static string addRunState(string runstate, string k, string v,int sourceId)
        {

            Dictionary<string,RunState> rsd = parseRunState(runstate);
            if (rsd.ContainsKey(k))
            {
                rsd[k].value = v;
            }
            else
            {
                RunState rs = new RunState();
                rs.key = k;
                rs.value = v;
                rs.reference = "" + sourceId;
                rsd.Add(k,rs);
            }
            return renderRunState(rsd); 
        }


 
    }
}
