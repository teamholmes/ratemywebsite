﻿using MyApp.Business.DomainObjects.Models;
using System;
using System.Collections.Generic;
namespace MyApp.Business.Services
{
    public interface ISwearWordService
    {
        Boolean IsSwearWord(string word);
    }
}
