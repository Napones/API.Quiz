using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Quiz.Persistencia
{
    public static class Db
    {
        // Note: Static initializers are thread safe.
        // If this class had a static constructor then these static variables 
        // would need to be initialized there. 
        private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

        private static readonly string connectionString = "";// ConfigurationManager.ConnectionStrings["SCIDatabase"].ConnectionString;
        private static readonly int timeOut = 120;

        #region Fast data readers
        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T Read<T>(string sql, Func<IDataReader, T> make, CommandType commandType, List<DbParameter> parms, bool autoRead = true)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = sql;
                    command.SetParameters(parms);  // Extension method
                    command.CommandTimeout = timeOut;
                    connection.Open();

                    T t = default(T);
                    var reader = command.ExecuteReader();
                    if (autoRead && reader.Read())
                        t = make(reader);
                    //else
                    //t = make(reader);

                    return t;
                }
            }

        }

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="make"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T Read<T>(string procName, Func<IDataReader, T> make, List<DbParameter> parms, bool autoRead = true)
        {
            return Read<T>(procName, make, CommandType.StoredProcedure, parms, autoRead);
        }

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataTable ReadTable(string sql, CommandType commandType, List<DbParameter> parms, bool autoRead = true)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = sql;
                    command.SetParameters(parms);  // Extension method
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    DbDataAdapter da = factory.CreateDataAdapter();

                    DataTable tbl = new DataTable();

                    da.SelectCommand = command;
                    da.Fill(tbl);

                    return tbl;
                }
            }
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static List<T> ReadList<T>(string sql, Func<IDataReader, T> make, CommandType commandType, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<T>();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        list.Add(make(reader));

                    return list;
                }
            }
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="make"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static List<T> ReadList<T>(string procName, Func<IDataReader, T> make, List<DbParameter> parms)
        {
            return ReadList<T>(procName, make, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int GetCount(string sql, CommandType commandType, List<DbParameter> parms)
        {
            return GetScalar(sql, commandType, parms).AsInt();
        }

        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int GetCount(string procName, List<DbParameter> parms)
        {
            return GetScalar(procName, parms).AsInt();
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static object GetScalar(string sql, CommandType commandType, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        public static List<string> ReadList(string sql, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = CommandType.StoredProcedure;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<String>();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        for (int i = 0; i < reader.FieldCount; i++)
                            list.Add(reader.GetValue(i).ToString());

                    return list;
                }
            }
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static object GetScalar(string procName, List<DbParameter> parms)
        {
            return GetScalar(procName, CommandType.StoredProcedure, parms);
        }

        public static Tuple<List<T>, List<T2>> ReadListTwoResults<T, T2>(string sql, Func<IDataReader, T> make, Func<IDataReader, T2> make2, CommandType commandType, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<T>();
                    var list2 = new List<T2>();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(make(reader));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                                list2.Add(make2(reader));
                        }
                    }

                    return Tuple.Create(list, list2);
                }
            }
        }
        #endregion

        #region Data update section

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int Insert(string sql, CommandType commandType, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandText = sql;

                    connection.Open();

                    return command.ExecuteScalar().AsInt();
                }
            }
        }

        public static void BulkCopy(string tabela, DataTable dtDados)
        {
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
            {
                destinationConnection.Open();
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = tabela;
                bulkCopy.WriteToServer(dtDados);
            }
        }

        /// <summary>
        /// BukCopy TABELA E COLUNAS MAPEADAS (DATA TABLE COM OS MESMOS NOMES DAS COLUNAS CORRESPONDENTES NO BANCO DE DADOS)
        /// </summary>
        /// <param name="dtDados">DATA TABLE COM OS MESMOS NOMES DAS COLUNAS CORRESPONDENTES NO BANCO DE DADOS</param>
        /// <param name="tabela">NOME DA TABELA</param>
        /// <param name="numeroRegistroInsert">INFORME A QUANTIDADE DE REGISTROS (dtDados.Rows.Count)</param>
        public static void BulkCopyMapeado(DataTable dtDados, string tabela, int numeroRegistroInsert)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = tabela;
                    bulkCopy.BatchSize = numeroRegistroInsert;

                    int index = 0;
                    int countColunas = dtDados.Columns.Count - 1;

                    foreach (DataColumn dtColumn in dtDados.Columns)
                    {
                        if (index <= countColunas)
                        {
                            bulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                            index++;
                        }
                    }
                    bulkCopy.WriteToServer(dtDados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int Insert(string procName, List<DbParameter> parms)
        {
            return Insert(procName, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Valida o CPF
        /// </summary>
        /// <param name="_valueCpf"></param>
        /// <returns></returns>
        private static Boolean ValidaCPF(String _valueCpf)
        {
            // Caso coloque todos os numeros iguais
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            _valueCpf = _valueCpf.Trim();
            _valueCpf = _valueCpf.Replace(".", "").Replace("-", "");

            if (_valueCpf.Length != 11)
                return false;

            tempCpf = _valueCpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return _valueCpf.EndsWith(digito);
        }

        /// <summary>
        /// Valida CNPJ
        /// </summary>
        /// <param name="_valueCNPJ"></param>
        /// <returns></returns>
        private static Boolean ValidaCNPJ(String _valueCNPJ)
        {
            // Se vazio

            if (_valueCNPJ.Length == 0)
                return false;

            // Limpa caracteres especiais
            _valueCNPJ = _valueCNPJ.Trim();
            _valueCNPJ = _valueCNPJ.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            _valueCNPJ = _valueCNPJ.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            _valueCNPJ = _valueCNPJ.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            _valueCNPJ = _valueCNPJ.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            _valueCNPJ = _valueCNPJ.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            _valueCNPJ = _valueCNPJ.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            _valueCNPJ = _valueCNPJ.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");




            // Se o tamanho for < 11 entao retorna como inválido

            if (_valueCNPJ.Length != 14)
                return false;


            // Caso coloque todos os numeros iguais

            switch (_valueCNPJ)
            {       //00000000000000

                case "11111111111111":
                    return false;

                case "00000000000000":
                    return false;

                case "22222222222222":
                    return false;

                case "33333333333333":
                    return false;

                case "44444444444444":
                    return false;

                case "55555555555555":
                    return false;

                case "66666666666666":
                    return false;

                case "77777777777777":
                    return false;

                case "88888888888888":
                    return false;

                case "99999999999999":
                    return false;

            }


            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            _valueCNPJ = _valueCNPJ.Trim();
            _valueCNPJ = _valueCNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

            if (_valueCNPJ.Length != 14)
                return false;

            tempCnpj = _valueCNPJ.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return _valueCNPJ.EndsWith(digito);

        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        public static void Update(string sql, CommandType commandType, List<DbParameter> parms)
        {
            //GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = 0;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        public static void Update(string procName, List<DbParameter> parms)
        {
            Update(procName, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        public static void Delete(string sql, List<DbParameter> parms)
        {
            Update(sql, parms);
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Extension method: Appends the db specific syntax to sql string 
        /// for retrieving newly generated identity (autonumber) value.
        /// </summary>
        /// <param name="sql">The sql string to which to append syntax.</param>
        /// <returns>Sql string with identity select.</returns>
        private static string AppendIdentitySelect(this string sql)
        {
            //switch (dataProvider)
            //{
            //    // Microsoft Access does not support multistatement batch commands
            //    case "System.Data.OleDb": return sql;
            //    case "System.Data.SqlClient": return sql + ";SELECT SCOPE_IDENTITY()";
            //    case "System.Data.OracleClient": return sql + ";SELECT MySequence.NEXTVAL FROM DUAL";
            //    default: return sql + ";SELECT @@IDENTITY";
            //}
            return sql + ";SELECT SCOPE_IDENTITY()";
        }

        public static string[] GetParameterProcedure(string procedureName)
        {
            List<string> ret = new List<string>();
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();
                var result = connection.GetSchema("ProcedureParameters");
                var view = result.DefaultView;
                view.RowFilter = string.Format("SPECIFIC_NAME = '{0}'", procedureName);
                foreach (DataRowView item in view)
                {
                    ret.Add(item["PARAMETER_NAME"].AsString());
                }

                return ret.ToArray();
            }

        }

        public static void SetParameters(this DbCommand command, List<DbParameter> parameters)
        {
            if (parameters != null && command != null)
            {
                foreach (DbParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
        }

        private static void SetParameters(this DbCommand command, object[] parms)
        {
            if (parms != null && parms.Length > 0)
            {
                // NOTE: Processes a name/value pair at each iteration
                for (int i = 0; i < parms.Length; i += 2)
                {
                    string name = parms[i].ToString();

                    // No empty strings to the database
                    if (parms[i + 1] is string && (string)parms[i + 1] == "")
                        parms[i + 1] = null;

                    // If null, set to DbNull
                    object value = parms[i + 1] ?? DBNull.Value;

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = name;
                    dbParameter.Value = value;

                    command.Parameters.Add(dbParameter);
                }
            }
        }

        public static DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = factory.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;

            return param;
        }

        #endregion

        public static string GetScalar(string v1, int v2, List<DbParameter> parms)
        {
            throw new NotImplementedException();
        }
    }

    public class BancoDados
    {
        private string dataProvider;
        private DbProviderFactory factory;
        //public string connectionStringName = ConfigurationManager.AppSettings.Get("SCIDatabase"); 

        public string connectionString { get; set; }

        private int timeOut = 120;// ConfigurationManager.AppSettings.Get("CommandTimeout").AsInt();

        public BancoDados(IConfiguration configuration)
        {
            if (configuration != null)
            {
                var dataBaseName = configuration.GetSection("appSettings:ConnectionStringName").Value;
                connectionString = configuration.GetConnectionString(dataBaseName);
                DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
                dataProvider = "System.Data.SqlClient";// ConfigurationManager.AppSettings.Get("DataProvider");

                DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);

                factory = DbProviderFactories.GetFactory(dataProvider);
            }
        }

        #region Fast data readers
        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public T Read<T>(string sql, Func<IDataReader, T> make, CommandType commandType, List<DbParameter> parms, bool autoRead = true)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = sql;
                    command.SetParameters(parms);  // Extension method
                    command.CommandTimeout = timeOut;
                    connection.Open();

                    T t = default(T);
                    var reader = command.ExecuteReader();
                    if (autoRead && reader.Read())
                        t = make(reader);
                    //else
                    //t = make(reader);

                    return t;
                }
            }

        }

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="make"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public T Read<T>(string procName, Func<IDataReader, T> make, List<DbParameter> parms, bool autoRead = true)
        {
            return Read<T>(procName, make, CommandType.StoredProcedure, parms, autoRead);
        }

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public DataTable ReadTable(string sql, CommandType commandType, List<DbParameter> parms, bool autoRead = true)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = sql;
                    command.SetParameters(parms);  // Extension method
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    DbDataAdapter da = factory.CreateDataAdapter();

                    DataTable tbl = new DataTable();

                    da.SelectCommand = command;
                    da.Fill(tbl);

                    return tbl;
                }
            }
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="make"></param>
        /// <param name="commandType"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<T> ReadList<T>(string sql, Func<IDataReader, T> make, CommandType commandType, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<T>();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        list.Add(make(reader));

                    return list;
                }
            }
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="make"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<T> ReadList<T>(string procName, Func<IDataReader, T> make, List<DbParameter> parms)
        {
            return ReadList<T>(procName, make, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int GetCount(string sql, CommandType commandType, List<DbParameter> parms)
        {
            return GetScalar(sql, commandType, parms).AsInt();
        }

        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int GetCount(string procName, List<DbParameter> parms)
        {
            return GetScalar(procName, parms).AsInt();
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public object GetScalar(string sql, CommandType commandType, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        public List<string> ReadList(string sql, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = CommandType.StoredProcedure;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<String>();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        for (int i = 0; i < reader.FieldCount; i++)
                            list.Add(reader.GetValue(i).ToString());

                    return list;
                }
            }
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public object GetScalar(string procName, List<DbParameter> parms)
        {
            return GetScalar(procName, CommandType.StoredProcedure, parms);
        }

        public Tuple<List<T>, List<T2>> ReadListTwoResults<T, T2>(string sql, Func<IDataReader, T> make, Func<IDataReader, T2> make2, CommandType commandType, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = timeOut;

                    connection.Open();

                    var list = new List<T>();
                    var list2 = new List<T2>();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(make(reader));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                                list2.Add(make2(reader));
                        }
                    }

                    return Tuple.Create(list, list2);
                }
            }
        }
        #endregion

        #region Data update section

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int Insert(string sql, CommandType commandType, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandText = sql;

                    connection.Open();

                    return command.ExecuteScalar().AsInt();
                }
            }
        }

        public void BulkCopy(string tabela, DataTable dtDados)
        {
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
            {
                destinationConnection.Open();
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = tabela;
                bulkCopy.WriteToServer(dtDados);
            }
        }

        /// <summary>
        /// BukCopy TABELA E COLUNAS MAPEADAS (DATA TABLE COM OS MESMOS NOMES DAS COLUNAS CORRESPONDENTES NO BANCO DE DADOS)
        /// </summary>
        /// <param name="dtDados">DATA TABLE COM OS MESMOS NOMES DAS COLUNAS CORRESPONDENTES NO BANCO DE DADOS</param>
        /// <param name="tabela">NOME DA TABELA</param>
        /// <param name="numeroRegistroInsert">INFORME A QUANTIDADE DE REGISTROS (dtDados.Rows.Count)</param>
        public void BulkCopyMapeado(DataTable dtDados, string tabela, int numeroRegistroInsert)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = tabela;
                    bulkCopy.BatchSize = numeroRegistroInsert;

                    int index = 0;
                    int countColunas = dtDados.Columns.Count - 1;

                    foreach (DataColumn dtColumn in dtDados.Columns)
                    {
                        if (index <= countColunas)
                        {
                            bulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                            index++;
                        }
                    }
                    bulkCopy.WriteToServer(dtDados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int Insert(string procName, List<DbParameter> parms)
        {
            return Insert(procName, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        public void Update(string sql, CommandType commandType, List<DbParameter> parms)
        {
            GravarLogExecucao(sql, parms);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.SetParameters(parms);
                    command.CommandTimeout = 0;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        public void Update(string procName, List<DbParameter> parms)
        {
            Update(procName, CommandType.StoredProcedure, parms);
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        public void Delete(string sql, List<DbParameter> parms)
        {
            Update(sql, parms);
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Extension method: Appends the db specific syntax to sql string 
        /// for retrieving newly generated identity (autonumber) value.
        /// </summary>
        /// <param name="sql">The sql string to which to append syntax.</param>
        /// <returns>Sql string with identity select.</returns>
        private string AppendIdentitySelect(string sql)
        {
            switch (dataProvider)
            {
                // Microsoft Access does not support multistatement batch commands
                case "System.Data.OleDb": return sql;
                case "System.Data.SqlClient": return sql + ";SELECT SCOPE_IDENTITY()";
                case "System.Data.OracleClient": return sql + ";SELECT MySequence.NEXTVAL FROM DUAL";
                default: return sql + ";SELECT @@IDENTITY";
            }
        }

        public string[] GetParameterProcedure(string procedureName)
        {
            List<string> ret = new List<string>();
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();
                var result = connection.GetSchema("ProcedureParameters");
                var view = result.DefaultView;
                view.RowFilter = string.Format("SPECIFIC_NAME = '{0}'", procedureName);
                foreach (DataRowView item in view)
                {
                    ret.Add(item["PARAMETER_NAME"].AsString());
                }

                return ret.ToArray();
            }

        }

        public void SetParameters(DbCommand command, List<DbParameter> parameters)
        {
            if (parameters != null && command != null)
            {
                foreach (DbParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
        }

        private void SetParameters(DbCommand command, object[] parms)
        {
            if (parms != null && parms.Length > 0)
            {
                // NOTE: Processes a name/value pair at each iteration
                for (int i = 0; i < parms.Length; i += 2)
                {
                    string name = parms[i].ToString();

                    // No empty strings to the database
                    if (parms[i + 1] is string && (string)parms[i + 1] == "")
                        parms[i + 1] = null;

                    // If null, set to DbNull
                    object value = parms[i + 1] ?? DBNull.Value;

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = name;
                    dbParameter.Value = value;

                    command.Parameters.Add(dbParameter);
                }
            }
        }

        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = factory.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;

            return param;
        }

        #endregion

        private void GravarLogExecucao(string nomeProcedure, List<DbParameter> parms)
        {
        }

        public string GetScalar(string v1, int v2, List<DbParameter> parms)
        {
            throw new NotImplementedException();
        }
    }
}

