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
    public class AdminServuiceTest : BusinessTestBase
    {

      

        [TestInitialize]
        public void ClassInitialise()
        {

            SetupTestEnvironment();

        }







        private void SetupTestEnvironment()
        {

            
        }


        [TestCleanup]
        public void ClassCleanUp()
        {
            _TestRepository = null;
        }


    }
}
