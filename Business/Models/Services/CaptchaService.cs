using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OP.General.Dal;
using MyApp.Business.DomainObjects.Models;
using System.Collections.Generic;
using MyApp.Business.Services;
using System;
using System.Web;
using OP.General.Captcha;
using System.Drawing;
using System.IO;
using OP.General.Extensions;


namespace MyApp.Business.Services
{
    public class CaptchaService : ICaptchaService
    {


        private IRepository _Repository;
        private ILog _Log;
        private IConfiguration _Configuration;




        public CaptchaService(IRepository repository, ILog log, IConfiguration configuration)
        {
            _Repository = repository;
            _Log = log;
            _Configuration = configuration;
        }

        public string GenerateRandomCode(string overrideURL = "")
        {



            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(6).ToString());

            _Log.Debug(string.Format("GenerateRandomCode {0} {1}", s, ""));


            if (_Configuration.ForceCAPTCHAtoShow)
            {
                return s;
            }

            if (!String.IsNullOrEmpty(overrideURL))
            {

                if (overrideURL.Contains("localhost") || overrideURL.Contains("xxxx.co.uk"))
                {
                    return "testing";
                }

            }



            return s;
        }


        public byte[] GenerateCaptchaImageAsJpeg(string code, int imagewidth, int imageheight)
        {
            _Log.Debug(string.Format("GenerateCaptchaImageAsJpeg {0} {1} {2}", code.Decrypt(), imagewidth, imageheight));

            CaptchaImage ci = new CaptchaImage(code.Decrypt(), imagewidth, imageheight, "Arial");
            MemoryStream ms = new MemoryStream();

            ci.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public Boolean IsCaptchaEntryValid(string userinput)
        {
            _Log.Debug(string.Format("GenerateCaptchaImageAsJpeg {0} {1}", userinput, ""));

            return false;

        }




    }
}
