﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Paginate;
using SWD392.Manim.Repositories.Repository.Interface;
using System.Linq.Expressions;

namespace SWD392.Manim.Repositories.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly Swd392Context _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Swd392Context dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(object id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task DeleteAsync(object id)
        {
            T entity = await _dbSet.FindAsync(id) ?? throw new Exception();
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_dbSet.Where(predicate));
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IEnumerable<T> Get(int index, int pageSize)
        {
            return _dbSet.Skip(index * pageSize).Take(pageSize).ToList();
        }



        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IQueryable<T>> GetAllQueryableAsync()
        {
            return await Task.FromResult(_dbSet.AsQueryable());
        }

        public async Task<IEnumerable<T>> GetAsync(int index, int pageSize)
        {
            return await _dbSet.Skip(index * pageSize).Take(pageSize).ToListAsync();
        }

        public T GetById(object id)
        {

            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<PaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize)
        {
            return await query.GetPaginatedList(index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void InsertRange(List<T> obj)
        {
            _dbSet.AddRange(obj);
        }
        public async Task InsertCollection(ICollection<T> collection)
        {
            await _dbSet.AddRangeAsync(collection);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }

}