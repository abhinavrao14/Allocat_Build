using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;

namespace Allocat.DataService
{
    public class EntityFrameworkDataService : IDataService, IDisposable
    {

        AllocatDbEntities _connection;

        public AllocatDbEntities dbConnection
        {
            get { return _connection; }
        }


        public void CommitTransaction(Boolean closeSession)
        {
            dbConnection.SaveChanges();
        }

        public void RollbackTransaction(Boolean closeSession)
        {

        }

        public void Save(object entity) { }

        public void CreateSession()
        {
            _connection = new AllocatDbEntities();
        }
        public void BeginTransaction() { }

        public void CloseSession() { }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}
