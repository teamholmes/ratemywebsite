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
using System.Text.RegularExpressions;

namespace MyApp.Business.Services
{
    public class SwearWordService : ISwearWordService
    {
        private IRepository _Repository;
        private ILog _Log;
        private IConfiguration _Configuration;


        public SwearWordService(IRepository repository, ILog log, IConfiguration configuration)
        {
            _Repository = repository;
            _Log = log;
            _Configuration = configuration;
        }


        public Boolean IsSwearWord(string word)
        {
            string uniquekey = "IsSwearWord_" + word;
            if (word.IsNullOrEmpty()) return false;

            if (GetAllMatchingSwearWords(word).Any()) return true;

            return false;

        }


        public List<SwearWord> GetAllMatchingSwearWords(string word)
        {
            string uniquekey = "GetAllMatchingSwearWords_" + word;

            if (Utilities.GetFromCache(uniquekey) != null)
            {
                return (List<SwearWord>)Utilities.GetFromCache(uniquekey);
            }
            else
            {

                List<SwearWord> listofoptions = _Repository.GetFiltered<SwearWord>((x => x.Word.Equals(word, StringComparison.InvariantCultureIgnoreCase))).ToList();

                Utilities.SetCache(uniquekey, listofoptions, 5);

                return (List<SwearWord>)Utilities.GetFromCache(uniquekey);
            }
        }



        #region Helper methods

        #endregion

    }
}
