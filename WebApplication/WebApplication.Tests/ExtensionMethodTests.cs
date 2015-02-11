using System;
using MyApp.Business.DomainObjects.Models;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Collections.Generic;
using Rhino.Mocks;
using MyApp.Business.Services;
using OP.General.Dal;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Reflection;
using OP.General.Extensions;

namespace MyApp.Tests
{

    [TestClass]
    public class ExtensionMethodTests : BusinessTestBase
    {



        [TestInitialize]
        public void ClassInitialise()
        {

           // SetupTestEnvironment();

        }



        [TestMethod]
        public void WIllPleuraliseCorrectly_1()
        {
            string singular = "Record";
            string pleural = "Records";

            Assert.IsTrue(singular.PleuraliseByAddingS(1) == singular);
        }

        [TestMethod]
        public void WIllPleuraliseCorrectly_2()
        {
            string singular = "Record";
            string pleural = "Records";

            Assert.IsTrue(singular.PleuraliseByAddingS(2) == pleural);
        }


        [TestMethod]
        public void DateDayFormatting_ZeroFormatted_1()
        {
            DateTime dt = new DateTime(1969, 1, 2);

            Assert.IsTrue(dt.ZeroFormattedDD() == "02" && dt.ZeroFormattedMM() == "01" && dt.FormattedYYYY() == "1969");
        }

        [TestMethod]
        public void DateDayFormatting_ZeroFormatted_2()
        {
            DateTime dt = new DateTime(1969, 11, 12);

            Assert.IsTrue(dt.ZeroFormattedDD() == "12" && dt.ZeroFormattedMM() == "11" && dt.FormattedYYYY() == "1969");
        }


         [TestMethod]
        public void PostCodeTest_1()
        {
            string postcode = "DD21RJ";

            Assert.IsTrue(postcode.ToFormattedPostCode() == "DD2 1RJ");
        }

         [TestMethod]
         public void PostCodeTest_2()
         {
             string postcode = "DD2 1RJ";

             Assert.IsTrue(postcode.ToFormattedPostCode() == "DD2 1RJ");
         }

         [TestMethod]
         public void PostCodeTest_3()
         {
             string postcode = "EH223NA";

             Assert.IsTrue(postcode.ToFormattedPostCode() == "EH22 3NA");
         }

         [TestMethod]
         public void PostCodeTest_4()
         {
             string postcode = "EH22 3NA";

             Assert.IsTrue(postcode.ToFormattedPostCode() == "EH22 3NA");
         }



        private void SetupTestEnvironment()
        {

          

        }


        [TestCleanup]
        public void ClassCleanUp()
        {
          
        }


    }
}
