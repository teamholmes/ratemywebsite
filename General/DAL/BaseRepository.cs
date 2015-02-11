using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
//using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OP.General.Dal
{
    // information regarding relationships can be found here
    // http://msdn.microsoft.com/en-us/data/jj591620.aspx#RequiredToOptional
    public abstract class BaseRepository : IRepository
    {


        public DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;

        }

        public T GetById<T>(int id)
        {
            Context = CreateNewDbContext();

            T result;

            var dbSet = Context.Set(typeof(T));

            result = (T)dbSet.Find(id);


            return result;
        }

        //public IEnumerable<T> GetMany<T>(Func<T, bool> action) where T : class
        public IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> action) where T : class
        {

            var dbSet = Context.Set<T>().Where(action);

            return dbSet;
        }


        public string Add<T>(T item)
        {
            var entry = Context.Set(typeof(T));

            var entity = entry.Add(item);

            Context.SaveChanges();

            return GetNewIDOfAddedEntry(entity);

        }


      

        public string Add<T>(T item, Boolean AllowAutoincrementOfId)
        {
            var entry = Context.Set(typeof(T));

            var entity = entry.Add(item);

            Context.SaveChanges();

            return GetNewIDOfAddedEntry(entity);

        }



      


        private string GetNewIDOfAddedEntry(object entity)
        {
            Type type = entity.GetType();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                {
                    return (string)property.GetValue(entity, null);
                }
            }

            throw new Exception("couldn't get id of added entity");
        }



        public bool Update<T>(string id, T item)
        {

            var dbSet = Context.Set(typeof(T));


            var entity = dbSet.Attach(item);


            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;

            return Context.SaveChanges() > 0;

        }


       


        public void CreateNewContext()
        {
            Context = CreateNewDbContext();
        }


        public bool UpdateAll<T>(IEnumerable<T> itemstoupdate)
        {
            var dbSet = Context.Set(typeof(T));

            foreach (T entity in itemstoupdate)
            {
                var currentitem = dbSet.Attach(entity);
                Context.Entry(currentitem).State = System.Data.Entity.EntityState.Modified;
            }

            return Context.SaveChanges() > 0;

        }


        public int AddAll<T>(IEnumerable<T> itemstoadd)
        {
            var entry = Context.Set(typeof(T));

            foreach (T entity in itemstoadd)
            {
                entry.Add(entity);
            }

            return Context.SaveChanges();
        }

        public void DeleteById<T>(string id)
        {
            var dbSet = Context.Set(typeof(T));

            T entity = (T)dbSet.Find(id);

            try
            {
                dbSet.Remove(entity);
                Context.SaveChanges();
            }
            catch (Exception exc)
            {
                //discovered through a unit test
                Debug.WriteLine(string.Format("Could not delete record of type {0} for id {1} - {2}", typeof(T).ToString(), id, exc.Message));
            }
        }


        public IList<T> GetAll<T>()
        {
            var dbSet = Context.Set(typeof(T));

            return Queryable.OfType<T>(dbSet).ToList();
        }


        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action = null) where T : class
        {
            var dbSet = Context.Set<T>().Where(action);
            return Queryable.OfType<T>(dbSet).ToList();
        }


        public IList<T> GetAll<T>(string eagerlyloadedobject1)
        {
            return GetAll<T>(eagerlyloadedobject1, string.Empty);
        }

        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1) where T : class
        {
            return GetFiltered<T>(action, eagerlyloadedobject1, string.Empty);
        }

        public IList<T> GetAll<T>(string eagerlyloadedobject1, string eagerlyloadedobject2)
        {
            if (!String.IsNullOrEmpty(eagerlyloadedobject1) && !String.IsNullOrEmpty(eagerlyloadedobject2))
            {
                var dbSet = Context.Set(typeof(T)).Include(eagerlyloadedobject1).Include(eagerlyloadedobject2);
                return Queryable.OfType<T>(dbSet).ToList();
            }
            else
            {
                var dbSet1 = Context.Set(typeof(T)).Include(eagerlyloadedobject1);
                return Queryable.OfType<T>(dbSet1).ToList();
            }
        }

        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1, string eagerlyloadedobject2) where T : class
        {
            if (!String.IsNullOrEmpty(eagerlyloadedobject1) && !String.IsNullOrEmpty(eagerlyloadedobject2))
            {

                var dbSet = Context.Set<T>().Where(action).Include(eagerlyloadedobject1).Include(eagerlyloadedobject2);
                return Queryable.OfType<T>(dbSet).ToList();
            }
            else
            {

                var dbSet1 = Context.Set<T>().Where(action).Include(eagerlyloadedobject1);
                return Queryable.OfType<T>(dbSet1).ToList();
            }
        }

        public IEnumerable<T> Find<T>(Func<T, bool> predictate)
        {
            var dbSet = Context.Set(typeof(T));

            return Queryable.OfType<T>(dbSet).Where(predictate).AsEnumerable<T>();
        }


        public int DeleteAll<T>(Expression<Func<T, bool>> criteria)
        {

            int results = 0;
            var dbSet = Context.Set(typeof(T));

            var selection = Queryable.OfType<T>(dbSet).Where(criteria);

            try
            {
                if (selection.Any())
                {
                    foreach (T entity in selection)
                    {
                        dbSet.Remove(entity);
                    }
                    results = Context.SaveChanges();
                }
            }
            catch (Exception err)
            {

            }

            return results;
        }

        private bool IsGenericICollection(object o)
        {
            Type type = o.GetType();

            bool result = type.GetInterfaces()
                .Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(ICollection<>));

            return result;
        }

        private DbContext CreateNewDbContext()
        {
            return (System.Data.Entity.DbContext)Activator.CreateInstance(Context.GetType());
        }

    }
}
