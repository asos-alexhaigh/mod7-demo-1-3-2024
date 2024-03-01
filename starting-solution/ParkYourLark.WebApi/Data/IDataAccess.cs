using System.Collections.Generic;

namespace ParkYourLark.WebApi.Data
{
    public interface IDataAccess
    {
        void Add<T>(T entity);
        IEnumerable<T> Get<T>();
    }
}