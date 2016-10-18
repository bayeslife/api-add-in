using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIAddIn;

namespace UnitTestProject1
{
    [TestClass]
    public class SampleManagerTests
    {
        [TestMethod]
        public void TestConvertEATypeToValue_Positive()
        {
            {
                object o = SampleManager.convertEATypeToValue(APIAddinClass.EA_TYPE_NUMBER, "5.23");
                Assert.IsTrue(o is float);
            }
            {
                object o = SampleManager.convertEATypeToValue(APIAddinClass.EA_TYPE_BOOLEAN, "true");
                Assert.IsTrue(o is bool);
            }
            {
                object o = SampleManager.convertEATypeToValue(APIAddinClass.EA_TYPE_STRING, "John");
                Assert.IsTrue(o is string);
            }
        }

        [TestMethod]
        public void TestConvertEATypeToValue_Negative()
        {
            {
                //Not a great test
                object o = SampleManager.convertEATypeToValue(APIAddinClass.EA_TYPE_NUMBER, "true");
                Assert.IsFalse(o is float);
            }
           
        }
    }
}
