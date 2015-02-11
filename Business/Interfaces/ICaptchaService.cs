using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyApp.Business.Services
{
    public interface ICaptchaService
    {
        byte[] GenerateCaptchaImageAsJpeg(string code, int imagewidth, int imageheight);
        Boolean IsCaptchaEntryValid(string userinput);

        string GenerateRandomCode(string overrideURL = "");
    }
}
