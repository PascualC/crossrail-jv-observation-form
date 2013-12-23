using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Crossrail.ObservationForm.DataLayer.Models;

namespace Crossrail.ObservationForm.DataLayer
{
    /// <summary>
    /// A thin repository for wrapping the EF context for use in creating
    /// mock repositories for use in unit tests.
    /// </summary>
    /// <typeparam name="T">Type of repository, e.g. <see cref="Observation"/></typeparam>

    public class Repository<T> where T : class
    {
        private readonly ObservationDbContext _context;

        public Repository(ObservationDbContext context)
        {
            _context = context;
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Use this to create a new object instance rather than an objects
        /// constructor, e.g. new(). Without this, if you try to use any 
        /// navigation properties after adding an object to the EF context
        /// they will be <c>NULL</c>.
        /// </summary>
        /// <returns></returns>

        public virtual T Create()
        {
            return _context.Set<T>().Create();
        }

        public virtual T Add(T item)
        {
            _context.Set<T>().Add(item);
            return item;
        }

        public virtual void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public void Remove(int id)
        {
            _context.Set<T>().Remove(GetById(id));
        }

        public void Remove(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public void Attach(T item)
        {
            _context.Set<T>().Attach(item);
        }

        public virtual void Update(T item)
        {
            //Slightly annoying I guess, slight leak of the ORM?
            _context.Entry(item).State = EntityState.Modified;
        }

        public DbEntityEntry Entry(T item)
        {
            return _context.Entry(item);
        }
    }
}
