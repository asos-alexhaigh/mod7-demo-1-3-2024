using System.Collections.Generic;

namespace ParkYourLark.WebApi.Data
{
    public class SqlDataAccess : IDataAccess
    {
        public IEnumerable<T> Get<T>()
        {
            throw new System.NotImplementedException();
        }

        public void Add<T>(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}