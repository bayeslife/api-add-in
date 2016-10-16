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
    public class DependencyManagerTest
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
            Assert.AreEqual("B", dependencies.ElementAt(0));
            Assert.AreEqual("C", dependencies.ElementAt(1));
            Assert.AreEqual("A", dependencies.ElementAt(2));
        }

        [TestMethod]
        public void TestCyclicGraph2()
        {
            dependencyManager.setDependency("A", "B");            
            dependencyManager.setDependency("B", "A");
            List<string> dependencies = dependencyManager.getDependencies();
            Assert.AreEqual(2, dependencies.Count);
            Assert.AreEqual("B", dependencies.ElementAt(0));
            Assert.AreEqual("A", dependencies.ElementAt(1));            
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

        [TestMethod]
        public void TestQuote()
        {
            dependencyManager.setDependency("CoverageComponentType", "Claim");
            dependencyManager.setDependency("Status", "Claim");
            dependencyManager.setDependency("PerilCategory", "ContractSpecification");
            dependencyManager.setDependency("PerilCategory", "ContractSpecification");
            dependencyManager.setDependency("PerilCategory", "ContractSpecification");
            dependencyManager.setDependency("CustomerFacingSyntheticTransaction", "Quote");
            dependencyManager.setDependency("TransactionType", "Quote");
            dependencyManager.setDependency("InsurancePolicy", "Quote");
            dependencyManager.setDependency("QuestionAnswer", "Quote");
            dependencyManager.setDependency("AssessmentResults", "Quote");
            dependencyManager.setDependency("Brand", "Quote");
            dependencyManager.setDependency("ProviderParty", "Quote");
            dependencyManager.setDependency("ProviderParty", "Quote");
            dependencyManager.setDependency("PhysicalConditionType", "PhysicalCondition");
            dependencyManager.setDependency("AddressReference", "Address");
            dependencyManager.setDependency("GeoLocation", "Address");
            dependencyManager.setDependency("ExposureGrade", "CoverageComponent");
            dependencyManager.setDependency("CoverageComponent", "CoverageComponent");
            dependencyManager.setDependency("Place", "CoverageComponent");
            dependencyManager.setDependency("InterestedParty", "CoverageComponent");
            dependencyManager.setDependency("Vehicle", "CoverageComponent");
            dependencyManager.setDependency("MoneyProvisionInvolvedInContract", "CoverageComponent");
            dependencyManager.setDependency("ContractSpecification", "CoverageComponent");
            dependencyManager.setDependency("Structure", "CoverageComponent");
            dependencyManager.setDependency("Liability", "CoverageComponent");
            dependencyManager.setDependency("CoverageComponentReference", "CoverageComponent");
            dependencyManager.setDependency("MaterialDamage", "CoverageComponent");
            dependencyManager.setDependency("CoverageComponentType", "CoverageComponent");
            dependencyManager.setDependency("InterestedParty", "CoverageComponent");
            dependencyManager.setDependency("VehicleActivity", "VehicleActivity");
            dependencyManager.setDependency("VehicleActivityType", "VehicleActivity");
            dependencyManager.setDependency("VehicleReference", "Vehicle");
            dependencyManager.setDependency("PhysicalCondition", "Vehicle");
            dependencyManager.setDependency("VehicleActivity", "Vehicle");
            dependencyManager.setDependency("StructureActivityType", "StructureActivity");
            dependencyManager.setDependency("StructureActivity", "StructureActivity");
            dependencyManager.setDependency("Account", "InsurancePolicy");
            dependencyManager.setDependency("ContractSpecification", "InsurancePolicy");
            dependencyManager.setDependency("CoverageComponent", "InsurancePolicy");
            dependencyManager.setDependency("Claim", "InsurancePolicy");
            dependencyManager.setDependency("InterestedParty", "InsurancePolicy");
            dependencyManager.setDependency("MoneyProvisionInvolvedInContract", "InsurancePolicy");
            dependencyManager.setDependency("ClaimFolder", "InsurancePolicy");
            dependencyManager.setDependency("InsurancePolicyReference", "InsurancePolicy");
            dependencyManager.setDependency("Status", "InsurancePolicy");
            dependencyManager.setDependency("ClosingSlip", "InsurancePolicy");
            dependencyManager.setDependency("LiabilityReference", "Liability");
            dependencyManager.setDependency("LiabilityActivity", "Liability");
            dependencyManager.setDependency("LiabilityType", "Liability");
            dependencyManager.setDependency("GeneralLiability", "Liability");
            dependencyManager.setDependency("EmployersLiability", "Liability");
            dependencyManager.setDependency("GeneralCover", "Liability");
            dependencyManager.setDependency("StatutoryLiability", "Liability");
            dependencyManager.setDependency("ActivityReference", "LiabilityActivity");
            dependencyManager.setDependency("Place", "LiabilityActivity");
            dependencyManager.setDependency("LiabilityActivityType", "LiabilityActivity");
            dependencyManager.setDependency("InsurancePolicy", "Account");
            dependencyManager.setDependency("ProviderParty", "Account");
            dependencyManager.setDependency("AccountReference", "Account");
            dependencyManager.setDependency("Limit", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("Premium", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("MoneyProvisionType", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("Rate", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("Adjustment", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("Deductable", "MoneyProvisionInvolvedInContract");
            dependencyManager.setDependency("Place", "GeneralLiability");
            dependencyManager.setDependency("PhysicalCondition", "Structure");
            dependencyManager.setDependency("Strengthened", "Structure");
            dependencyManager.setDependency("StructureActivity", "Structure");
            dependencyManager.setDependency("StructureReference", "Structure");
            dependencyManager.setDependency("Place", "Structure");
            dependencyManager.setDependency("ActivityOccurence", "Structure");
            dependencyManager.setDependency("Address", "Place");
            dependencyManager.setDependency("PlaceType", "Place");
            dependencyManager.setDependency("PartyReference", "InterestedParty");
            dependencyManager.setDependency("PartyRoleType", "InterestedParty");
            dependencyManager.setDependency("CoverageComponentType", "ClaimFolder");
            dependencyManager.setDependency("CoverageComponentType", "ClosingSlip");
            dependencyManager.setDependency("PercentStrengtheningCompliant", "Strengthened");

            List<string> dependencies = dependencyManager.getDependencies();
            Assert.IsTrue(dependencies.IndexOf("PerilCategory") < dependencies.IndexOf("ContractSpecification"));
            
        }

    }
}
