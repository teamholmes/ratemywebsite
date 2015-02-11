using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Business.DomainObjects.Models
{
    public class AppConfiguration
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
