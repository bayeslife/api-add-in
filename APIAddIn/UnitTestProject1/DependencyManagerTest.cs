using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;

namespace UnitTestProject1
{
    [TestClass]
    class DependencyManagerTest
    {

        DependencyManager dependencyManager = new DependencyManager();

        [TestMethod]
        public void TestSimpleParentChildDependence()
        {
            dependencyManager.setDependency("A", "B");
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(2, dependencies.Count);
            Assert.AreEqual("A", dependencies.ElementAt(0));
            Assert.AreEqual("B", dependencies.ElementAt(1));
        }

        [TestMethod]
        public void TestCyclicGraph3()
        {
            dependencyManager.setDependency("A", "B");
            dependencyManager.setDependency("B", "C");
            dependencyManager.setDependency("C", "A");            
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(3, dependencies.Count);
            Assert.AreEqual("A", dependencies.ElementAt(0));
            Assert.AreEqual("B", dependencies.ElementAt(1));
            Assert.AreEqual("C", dependencies.ElementAt(2));
        }

        [TestMethod]
        public void TestCyclicGraph2()
        {
            dependencyManager.setDependency("A", "B");            
            dependencyManager.setDependency("B", "A");
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(2, dependencies.Count);
            Assert.AreEqual("A", dependencies.ElementAt(0));
            Assert.AreEqual("B", dependencies.ElementAt(1));            
        }

        [TestMethod]
        public void TestSelfReference()
        {
            dependencyManager.setDependency("A", "A");
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(1,dependencies.Count);

        }

        [TestMethod]
        public void TestOutOfOrder()
        {
            dependencyManager.setDependency("A", "B");
            dependencyManager.setDependency("A", "C");
            dependencyManager.setDependency("B", "C");
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(3, dependencies.Count);
            Assert.AreEqual("A", dependencies.ElementAt(0));
            Assert.AreEqual("B", dependencies.ElementAt(1));
            Assert.AreEqual("C", dependencies.ElementAt(2));
        }

    }
}
