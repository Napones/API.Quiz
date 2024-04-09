using Microsoft.Extensions.Configuration;
using Quiz.Entidade;
using Quiz.Persistencia;
using System.Data;
using System.Data.SqlClient;

namespace Quiz.Negocio
{
    public abstract class NegBaseComum<T> : BancoDados, IDisposable
               where T : class
    {
        protected IConfiguration _configuration;

        /// <summary>
        /// Objeto que acesso o banco
        /// </summary>
        protected Quiz.Entidade.Models.QuizDataBaseContext Db { get; set; }

        /// <summary>
        /// Inicializador da classe
        /// cria o objeto Db para ser usado nas classes de negocio
        /// </summary>
        public NegBaseComum(Quiz.Entidade.Models.QuizDataBaseContext dbSCIContext, IConfiguration configuration) : base(configuration)
        {
            var option = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder();

            //  option.UseSqlServer(ConfigurationManager.ConnectionStrings["SCIDatabase"].ConnectionString);
            //new SCI.Entidade.Context.SCIContext(  option);
            Db = dbSCIContext;// new SCI.Entidade.Context.SCIContext();
            this._configuration = configuration;
            // Db.Configuration.LazyLoadingEnabled = false;
            //Db.Configuration.ProxyCreationEnabled = false;
            //Db.Database.Log = (query) => System.Diagnostics.Debug.Write(query);
        }
        public NegBaseComum(Quiz.Entidade.Models.QuizDataBaseContext dbSCIContext) : base(null)
        {
            var option = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder();

            //  option.UseSqlServer(ConfigurationManager.ConnectionStrings["SCIDatabase"].ConnectionString);
            //new SCI.Entidade.Context.SCIContext(  option);
            Db = dbSCIContext;// new SCI.Entidade.Context.SCIContext();
                              // Db.Configuration.LazyLoadingEnabled = false;
                              //Db.Configuration.ProxyCreationEnabled = false;
                              //Db.Database.Log = (query) => System.Diagnostics.Debug.Write(query);
        }


        /// <summary>
        /// Insere um item no banco de dados
        /// </summary>
        /// <param name="item">Objeto a ser inserido</param>
        /// <example>
        /// <![CDATA[
        ///     public void Inserir(MODALIDADE modalidade)
        ///     {
        ///         this.Inserir(modalidade) 
        ///     }
        ///     new NegModalidade().Inserir(modalidade)
        /// ]]>
        /// </example>
        public virtual RetornoAcao<T> Inserir(T item)
        {
            Db.Set<T>().Add(item);
            Db.SaveChanges();
            RetornoAcao<T> retorno = new RetornoAcao<T>();
            retorno.Objeto = item;
            return retorno;
        }

        /// <summary>
        /// Atualiza um item no banco de dados
        /// </summary>
        /// <param name="entity">Objeto a ser Atualizado</param>
        public virtual RetornoAcao<T> Atualizar(T entity)
        {
            Db.Set<T>().Attach(entity);
            Db.Entry<T>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Db.SaveChanges();
            RetornoAcao<T> retorno = new RetornoAcao<T>();
            retorno.Objeto = entity;
            return retorno;
        }

        /// <summary>
        /// Destroi o objeto
        /// </summary>
        public void Dispose()
        {
            Db.Dispose();
        }

        //public override string connectionString
        //{ get { return ConfigurationManager.ConnectionStrings["ADO.NET.EXPERT"].ConnectionString; } }


        public void BulkCopyMapeado(DataTable dtDados, string tabela, int numeroRegistroInsert)
        {
            try
            {

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_configuration.GetConnectionString("QuizDataBase"), SqlBulkCopyOptions.KeepIdentity))
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
    }
}
