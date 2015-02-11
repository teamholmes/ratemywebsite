using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Dal
{
    public class TestRepository : IRepository
    {
        int autoIncrementingId = 0;

        private List<object> records;

        public TestRepository()
        {
            records = new List<object>();
        }

        public string Add<T>(T item)
        {
            autoIncrementingId++;

            string guid = Guid.NewGuid().ToString();

            SetIdProperty(item, guid);

            records.Add(item);

            return guid;
        }

        public string Add<T>(T item, Boolean AllowAutoincrementOfId)
        {

            string guid = Guid.NewGuid().ToString();
            if (AllowAutoincrementOfId)
            {
                autoIncrementingId++;

                SetIdProperty(item, guid);
            }

            records.Add(item);

            return guid;
        }


        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action = null) where T : class
        {

            var typedExpression = (System.Linq.Expressions.Expression<System.Func<T, bool>>)(object)action;

            var lists = new List<T>();
            foreach (object o in records)
            {
                try
                {
                    lists.Add((T)o);
                }
                catch (Exception err)
                { }

            }

            return lists.AsQueryable().Where(action).ToList();
        }


        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1) where T : class
        {

            var typedExpression = (System.Linq.Expressions.Expression<System.Func<T, bool>>)(object)action;

            var lists = new List<T>();
            foreach (object o in records)
            {
                try
                {
                    lists.Add((T)o);
                }
                catch (Exception err)
                { }
            }

            return lists.AsQueryable().Where(action).ToList();
        }

        public IList<T> GetFiltered<T>(Expression<Func<T, bool>> action, string eagerlyloadedobject1, string eagerlyloadedobject2) where T : class
        {

            var typedExpression = (System.Linq.Expressions.Expression<System.Func<T, bool>>)(object)action;

            var lists = new List<T>();
            foreach (object o in records)
            {
                try
                {
                    lists.Add((T)o);
                }
                catch (Exception err)
                {

                }
            }

            return lists.AsQueryable().Where(action).ToList();
        }


        public IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> action) where T : class
        {
            //    return records.Where(x => AreTypesEqual(x.GetType(), typeof(T)) && action((T)x)).Select(x => (T)x).ToList();
            return null;
        }




        public T GetById<T>(int id)
        {
            return (T)records.Find(x => (GetIdForObject(x) == id) && (AreTypesEqual(x.GetType(), typeof(T))));
        }



        private T GetByGuid<T>(string id)
        {
            return (T)records.Find(x => (GetGuidtIdForObject(x) == id) && (AreTypesEqual(x.GetType(), typeof(T))));
        }

        public void DeleteById<T>(string id)
        {
            var item = GetByGuid<T>(id);

            records.Remove(item);
        }





        public bool Update<T>(string id, T item)
        {
            var existingItem = GetByGuid<T>(id);

            existingItem = item;
            return true;
        }


        public void CreateNewContext()
        {
            // do nothing
        }

        public bool UpdateAll<T>(IEnumerable<T> itemstoupdate)
        {
            // TODO : Finish This this
            return true;
        }

        public int AddAll<T>(IEnumerable<T> itemstoadd)
        {
            // todo

            return 1;
        }

        private long GetIdForObject(object o)
        {
            Type t = o.GetType();

            var propertyInfo = t.GetProperty("Id");
            if (propertyInfo == null) return long.MinValue;

            return Convert.ToInt64(propertyInfo.GetValue(o, null));
        }


        private string GetGuidtIdForObject(object o)
        {
            Type t = o.GetType();

            var propertyInfo = t.GetProperty("Id");
            if (propertyInfo == null) return null;

            return Convert.ToString(propertyInfo.GetValue(o, null));
        }

        public IList<T> GetAll<T>()
        {
            return records.Where(x => AreTypesEqual(x.GetType(), typeof(T))).Select(x => (T)x).ToList();
        }

        public IList<T> GetAll<T>(string eagerlyloadedobject1)
        {
            return GetAll<T>(eagerlyloadedobject1, string.Empty);
        }

        public IList<T> GetAll<T>(string eagerlyloadedobject1, string eagerlyloadedobject2)
        {
            return records.Where(x => AreTypesEqual(x.GetType(), typeof(T))).Select(x => (T)x).ToList();
        }

        private bool AreTypesEqual(Type a, Type b)
        {
            return a == b;
        }

        private void SetIdProperty<T>(T item, string id)
        {
            Type type = item.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name == "Id")
                {
                    if (property.GetType() == typeof(string))
                    {
                        property.SetValue(item, id, null);
                    }
                    else if (property.GetType() == typeof(int))
                    {
                        property.SetValue(item, autoIncrementingId, null);
                    }
                }
            }
        }

        public IEnumerable<T> Find<T>(Func<T, bool> predictate)
        {
            return records
                .Where(x => AreTypesEqual(x.GetType(), typeof(T)))
                .Cast<T>()
                .Where(predictate);
        }

        public int DeleteAll<T>(Expression<Func<T, bool>> criteria)
        {  // TODO
            return -1;
            //return records.RemoveAll(criteria);
        }


    }
}
