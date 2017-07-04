using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MI.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class OADbContext : DbContext, IDbContext
    {
        public OADbContext(string nameOrString) :
            base(nameOrString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbEntityEntry<T> GetEntry<T>(T t) where T : class
        {
            return base.Entry<T>(t);
        }

        public IDbSet<T> SetEntity<T>() where T : class
        {
            return base.Set<T>();
        }
        /// <summary>
        /// 执行sql查询语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<T>(sql, parameters);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="isTransaction">是否使用事务</param>
        /// <param name="parameters"参数></param>
        /// <returns>受影响行数</returns>
        public int ExecuteSqlCommand(string sql, bool isTransaction = false, params object[] parameters)
        {

            var transactionalBehavior = isTransaction
             ? TransactionalBehavior.DoNotEnsureTransaction
             : TransactionalBehavior.EnsureTransaction;

            return this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //动态添加映射实体
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(m => m.BaseType != null && m.BaseType.IsGenericType && m.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in types)
            {
                dynamic map = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(map);
            }
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// 一键解锁
        /// </summary>
        /// <param name="sBuilding">楼栋</param>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        public virtual ObjectResult<string> aKeyUnlockDormitory(string sBuilding, string sCommunity)
        {
            var sBuildingParameter = sBuilding != null ?
                new SqlParameter("sBuilding", sBuilding) :
                new SqlParameter("sBuilding", typeof(string));

            var sCommunityParameter = sCommunity != null ?
                new SqlParameter("sCommunity", sCommunity) :
                new SqlParameter("sCommunity", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<string>("aKeyUnlockDormitory @sBuilding,@sCommunity", sBuildingParameter, sCommunityParameter);
        }
        /// <summary>
        /// 生成格子数据
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="tid2"></param>
        /// <param name="width"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public virtual ObjectResult<string> GenerateGridData(Nullable<int> tid, Nullable<int> tid2, Nullable<int> width, Nullable<int> high)
        {
            var tidParameter = tid.HasValue ?
                new SqlParameter("tid", tid) :
                new SqlParameter("tid", typeof(int));

            var tid2Parameter = tid2.HasValue ?
                new SqlParameter("tid2", tid2) :
                new SqlParameter("tid2", typeof(int));

            var widthParameter = width.HasValue ?
                new SqlParameter("width", width) :
                new SqlParameter("width", typeof(int));

            var highParameter = high.HasValue ?
                new SqlParameter("high", high) :
                new SqlParameter("high", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<string>("GeneratingGridData @tid,@tid2,@width,@high", tidParameter, tid2Parameter, widthParameter, highParameter);
        }
        public virtual ObjectResult<GetExtendProperByTabName_Result> GetExtendProperByTabNames(string tableName)
        {
            var tableNameParameter = tableName != null ?
                new SqlParameter("TableName", tableName) :
                new SqlParameter("TableName", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<GetExtendProperByTabName_Result>("GetExtendProperByTabName @TableName", tableNameParameter);
        }
    }
}

