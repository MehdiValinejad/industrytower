using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;



namespace IndustryTower.DAL
{
    public class GenericReader
    {
        internal SqlConnection connection;

        public GenericReader()
        {
            this.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ITTContext"].ConnectionString);
        }

        public SqlDataReader GetDataReader(string statement, List<SqlParameter> parameters)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command.CommandText = statement;
                command.Connection = connection;
                parameters.ForEach(x => command.Parameters.Add(x));

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }


        public SqlDataReader GetSPDataReader(string SPName, params SqlParameter [] parameters)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command.CommandText = SPName;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                foreach (var pr in parameters)
                {
                    command.Parameters.Add(pr);
                }

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public SqlDataReader GetSPDataReader(string SPName, List<SqlParameter> parameters)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command.CommandText = SPName;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                parameters.ForEach(x => command.Parameters.Add(x));

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public void SPExecuteNonQuery(string SPName, params SqlParameter [] parameters)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command.CommandText = SPName;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                foreach (var pr in parameters)
                {
                    command.Parameters.Add(pr);
                }

                command.ExecuteNonQuery();
            }
        }


        public SqlDataReader GetSPDataReader(string SPName, List<SqlParameter> parameters, out SqlCommand cmd)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command.CommandText = SPName;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                parameters.ForEach(x => command.Parameters.Add(x));

                cmd = command;
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
   
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal ITTContext context;
        internal DbSet<TEntity> dbSet;
        
       

        public GenericRepository(ITTContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            var cdd = context.Database.Connection;

            //context.Configuration.AutoDetectChangesEnabled = false;
        }
        public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }


        public virtual void Update( TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual void PartialUpdate(FormCollection form, TEntity entityToUpdate, int Id)
        {
            var poco = GetByID(Id);
            foreach (string key in form)
            {

                System.Reflection.PropertyInfo propertyInfo = poco.GetType().GetProperty(key);
                if (propertyInfo != null)
                {

                    propertyInfo.SetValue(poco, Convert.ChangeType(form[key], propertyInfo.PropertyType), null);

                }

            }
        }
        
    }
}