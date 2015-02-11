using OP.General.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DAL.Repository
{
    public class ApplicationRepository : BaseRepository
    {
        public ApplicationRepository() : base(new ApplicationDbContext())
        {

        }
    }
}
