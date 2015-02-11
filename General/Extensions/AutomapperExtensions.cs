using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class AutomapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object src)
        {
            if (src == null)
                return default(TDestination);

            return (TDestination)Mapper.Map(src, src.GetType(), typeof(TDestination));
        }

        public static void MapToExisting<TDestination>(this object src, TDestination existingInstance)
        {
            if (src == null)
                return;

            existingInstance = (TDestination)Mapper.Map(src, existingInstance, src.GetType(), typeof(TDestination));
        }
    }
}
