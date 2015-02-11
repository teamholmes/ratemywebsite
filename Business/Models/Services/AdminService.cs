using MyApp.Business.DomainObjects;
using OP.General.Extensions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OP.General.Dal;
using MyApp.Business.DomainObjects.Models;
using System.Collections.Generic;
using WebMatrix.WebData;
using System.Web.Security;
using System.Web;
using System.Web.Helpers;
using System.Collections;
using OP.General.Model;
using Aspose.Cells;

namespace MyApp.Business.Services
{
    public class AdminService : IAdminService
    {
        private IRepository _Repository;
        private ILog _Log;
        private IConfiguration _Configuration;
        private IAccountService _AccountService;


        public AdminService(IRepository repository, ILog log, IConfiguration configuration, IAccountService accountService)
        {
            _Repository = repository;
            _Log = log;
            _Configuration = configuration;
            _AccountService = accountService;
        }




    }
}
