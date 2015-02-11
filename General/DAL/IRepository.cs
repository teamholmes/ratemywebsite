using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
namespace OP.General.Dal
{
    public interface IRepository
    {
        string Add<T>(T item);
        string Add<T>(T item, Boolean AllowAutoincrementOfId);

        void DeleteById<T>(string id);
        global::System.Collections.Generic.IEnumerable<T> Find<T>(Func<T, bool> predictate);
        global::System.Collections.Generic.IList<T> GetAll<T>();
        IList<T> GetAll<T>(string eagerlyloadedobject1, string eagerlyloadedobject2);
        IList<T> GetAll<T>(string eagerlyloadedobject1);
        T GetById<T>(int id);
        //global::System.Collections.Generic.IEnumerable<T> GetMany<T>(Func<T, bool> action);
        //IEnumerable<T> GetMany<T>(Func<T, bool> action) where T : class;
        IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> action) where T : class;

        bool Update<T>(string id, T item);
        bool UpdateAll<T>(IEnumerable<T> itemstoupdate);
        int AddAll<T>(IEnumerable<T> itemstoadd);
        void CreateNewContext();
        int DeleteAll<T>(Expression<Func<T, bool>> criteria);

        IList<T> GetFiltered<T>(Expression<Func<T, bool>> action = null) where T : class;
        IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1) where T : class;
        IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1, string eagerlyloadedobject2) where T : class;


    }
}
