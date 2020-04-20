using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Tests.Mocks
{
    public class MockContext<T>:IRepository<T> where T:BaseEntity
    {
        List<T> items;
        string className;
        public MockContext()
        {
            items = new List<T>();
        }
        public void Commit()
        {
            return;
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
