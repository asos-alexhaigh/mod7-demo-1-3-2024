using System.Collections.Generic;

namespace ParkYourLark.WebApi.Data
{
    public interface IDataAccess
    {
        IEnumerable<T> Get<T>();
        void Add<T>(T entity);
    }
}