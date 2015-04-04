using System.Data;
using System.Data.Odbc;
using Furesoft.Creek.Office.Access;

namespace Access_Example
{
    public class NorthWindContext : ACCDBContext
    {
        public NorthWindContext(string connectionString) : base(connectionString)
        {
        }

        public override IDbConnection CreateConnection()
        {
            IDbConnection connection = new OdbcConnection();
            connection.ConnectionString = this.connectionString;
            return connection;
        }

        public ACCDBSet<Employees> Employees
        {
            get
            {
                return new ACCDBSet<Employees>(this);
            }
        }
    }
}