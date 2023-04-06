using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Spacebardesktop.Repositories
{
   public abstract class Repositorio
    {
        private readonly string connectionString;
        public Repositorio()//conecxao com o sql.
        {
            connectionString = "Server=(local); Database=SpaceBar; Integrated Security=true";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
