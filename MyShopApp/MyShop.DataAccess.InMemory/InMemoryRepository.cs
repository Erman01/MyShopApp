using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;
        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }
        public void Commit()
        {
            cache[className] = items;
        }
        public void Insert(T Tentity)
        {
            items.Add(Tentity);
        }
        public void Update(T Tentity)
        {
            T tToupdate = items.Find(t => t.Id == Tentity.Id);
            if (tToupdate != null)
            {
                tToupdate = Tentity;
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }
        public T Find(string id)
        {
            T tToFind = items.Find(t => t.Id == id);
            if (tToFind != null)
            {
                return tToFind;
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string id)
        {
            T tToDelete = items.Find(t => t.Id == id);
            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

    }
}
