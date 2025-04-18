﻿using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DataLibrary.Models;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataLibrary.Repositories;

public abstract class BaseRepository<TEntity, TModel>(DataContext context) : IBaseRepository<TEntity, TModel> where TEntity : class
{
    protected readonly DataContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<RepositoryResult<bool>> CreateAsync(TEntity entity)
    {
        if (entity == null) return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null"};

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in creating {nameof(TEntity)} in database: {ex.Message}");
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public virtual async Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync (
            bool orderByDescendning = false,
            Expression<Func<TEntity, object>>? sortBy = null,
            Expression<Func<TEntity, bool>>? where = null,
            params Expression<Func<TEntity, object>>[] includes) 
    {
        
        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescendning ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return new RepositoryResult<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = result };
       
    }

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllEntitiesAsync(
          bool orderByDescendning = false,
          Expression<Func<TEntity, object>>? sortBy = null,
          Expression<Func<TEntity, bool>>? where = null,
          params Expression<Func<TEntity, object>>[] includes)
    {

        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescendning ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        return new RepositoryResult<IEnumerable<TEntity>> { Succeeded = true, StatusCode = 200, Result = entities };

    }


    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescendning = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] includes)
    {

        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescendning ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

        var entities = await query.Select(selector).ToListAsync();
        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepositoryResult<IEnumerable<TSelect>> { Succeeded = true, StatusCode = 200, Result = result };

    }

    public virtual async Task<RepositoryResult<TModel>> GetOneAsync(
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = where != null
          ? await query.FirstOrDefaultAsync(where)
          : await query.FirstOrDefaultAsync();

        if (entity == null)
            return new RepositoryResult<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

        var result = entity.MapTo<TModel>();

        // Specialmappning för MemberEntity -> Member (inklusive Address). Genererad av chatGPT4o
        if (typeof(TModel) == typeof(Member) && entity is MemberEntity memberEntity && result is Member member)
        {
            if (memberEntity.Address != null)
            {
                member.Address = new MemberAddress
                {
                    StreetName = memberEntity.Address.StreetName,
                    PostalCode = memberEntity.Address.PostalCode,
                    City = memberEntity.Address.City
                };
            }
        }

        return new RepositoryResult<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }


    /// <summary>
    /// Get one entity from the database so that EF can track it.Generated by chatGPT4o
    /// </summary>
    /// <param name="where"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public virtual async Task<RepositoryResult<TEntity>> GetOneEntityAsync(
    Expression<Func<TEntity, bool>>? where = null,
    params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length > 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = where != null
            ? await query.FirstOrDefaultAsync(where)
            : await query.FirstOrDefaultAsync();

        if (entity == null)
            return new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

        return new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }





    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity updatedEntity)
    {
        if (updatedEntity == null) 
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

        try
        {
            _dbSet.Update(updatedEntity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in updating {nameof(TEntity)} in database: {ex.Message}");
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null) 
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in deleting {nameof(TEntity)} from database: {ex.Message}");
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var exists = await _dbSet.AnyAsync(predicate);
        return !exists
            ? new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found" }
            : new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
    }
}
