using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Metering_System_BackEnd.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Smart_Metering_System_BackEnd.Implementations.Repositories;

public class GenericRepo<T> : IGenericRepo<T> where T : class, new()
{
    protected SmartElectricMeteringContext context;
    public async Task<T> Create(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> Update(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> Get(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().FirstOrDefaultAsync(expression);
    }
    public async Task<IList<T>> GetAll()
    {
        return await context.Set<T>().ToListAsync();
    }
    public async Task<bool> Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<IList<T>> GetByExpression(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().Where(expression).ToListAsync();
    }
}

