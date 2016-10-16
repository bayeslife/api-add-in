using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAddIn
{
    /* This class keeps track of dependencies to prevent infinite loops during serialization of graph */
    public class DependencyManager
    {
        List<string> dependencies = new List<string>();

        public List<string> getDependencies()
        {
            return dependencies;
        }

        public void setDependency(string dependedUpon1, string dependsUpon2)
        {
            if (dependsUpon2 == null)
            {
                dependencies.Add(dependedUpon1);
            }
            else if (dependedUpon1 == dependsUpon2)
            {
                int dependedUpon1Index = dependencies.IndexOf(dependedUpon1);
                if(dependedUpon1Index<0)
                    dependencies.Insert(0,dependedUpon1);
                else
                    return;
            }
            else
            {
                int dependedUpon1Index = dependencies.IndexOf(dependedUpon1);
                int dependsUpon2Index = dependencies.IndexOf(dependsUpon2);
                if (dependsUpon2Index == -1 && dependedUpon1Index == -1)
                {
                    dependencies.Add(dependedUpon1);
                    dependencies.Add(dependsUpon2);
                }
                else if (dependsUpon2Index == -1)
                {
                    dependencies.Insert(dependedUpon1Index+1, dependsUpon2);
                }
                else if (dependedUpon1Index == -1)
                {
                    dependencies.Insert(dependsUpon2Index, dependedUpon1);
                }
                else
                {
                    if (dependedUpon1Index > dependsUpon2Index)
                    {
                        //dependencies.RemoveAt(dependedUpon1Index);
                        //dependencies.Insert(dependsUpon2Index, dependedUpon1);

                        dependencies.RemoveAt(dependsUpon2Index);
                        dependencies.Insert(dependedUpon1Index, dependsUpon2);
                    }
                }

            }
        }

    }
}
