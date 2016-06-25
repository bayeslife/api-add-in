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

        public void setDependency(string dependsUpon1, string dependedUpon2)
        {
            if (dependedUpon2 == null)
            {
                dependencies.Add(dependsUpon1);
            }
            else if (dependsUpon1 == dependedUpon2)
            {
                return;
            }
            else
            {
                int dependsUpon1Index = dependencies.IndexOf(dependsUpon1);
                int dependedUpon2Index = dependencies.IndexOf(dependedUpon2);
                if (dependedUpon2Index == -1 && dependsUpon1Index == -1)
                {
                    dependencies.Add(dependsUpon1);
                    dependencies.Add(dependedUpon2);
                }
                else if (dependedUpon2Index == -1)
                {
                    dependencies.Insert(dependsUpon1Index, dependedUpon2);
                }
                else if (dependsUpon1Index == -1)
                {
                    dependencies.Insert(0, dependsUpon1);
                }
                else
                {
                    if (dependsUpon1Index > dependedUpon2Index)
                    {
                        dependencies.RemoveAt(dependsUpon1Index);
                        dependencies.Insert(dependedUpon2Index, dependsUpon1);
                    }
                }

            }
        }

    }
}
