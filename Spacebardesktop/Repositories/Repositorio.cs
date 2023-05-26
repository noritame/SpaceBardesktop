using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Spacebardesktop.Repositories
{


   public class Repositorio
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
        public DataSet SqlProcedure(string procedurename, List<SqlParameter> parameters = null)
        {
            var dataSet = new DataSet();

            using (var command = new SqlCommand() { Connection = GetConnection(), CommandType = CommandType.StoredProcedure, CommandText = procedurename })
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = GetConnection();

                if (parameters != null && parameters.Count > 0)
                {
                    // Limpa a lista de parametros antes de adicionar novos
                    command.Parameters.Clear();
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataSet);
                }
            }
            return dataSet;
        }
    }

}
