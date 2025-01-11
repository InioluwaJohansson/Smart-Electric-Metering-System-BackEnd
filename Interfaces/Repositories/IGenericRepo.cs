using System.Linq.Expressions;
namespace Smart_Metering_System_BackEnd.Interfaces.Repositories;
public interface IGenericRepo<T>
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Get(Expression<Func<T, bool>> expression);
    Task<IList<T>> GetAll();
    Task<bool> Delete(T entity);
    Task<IList<T>> GetByExpression(Expression<Func<T, bool>> expression);
}
