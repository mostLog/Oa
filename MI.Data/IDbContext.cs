using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace MI.Data
{
    public interface IDbContext
    {

        IDbSet<T> SetEntity<T>() where T:class;

        int SaveChanges();

        DbEntityEntry<T> GetEntry<T>(T t) where T : class;
        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string sql,params object[] parameters);
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, bool isTransaction = false, params object[] parameters);
        /// <summary>
        /// 一键解锁
        /// </summary>
        /// <param name="sBuilding">楼栋</param>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        ObjectResult<string> aKeyUnlockDormitory(string sBuilding, string sCommunity);
        /// <summary>
        /// 生成格子数据
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="tid2"></param>
        /// <param name="width"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        ObjectResult<string> GenerateGridData(Nullable<int> tid, Nullable<int> tid2, Nullable<int> width, Nullable<int> high);
        ObjectResult<GetExtendProperByTabName_Result> GetExtendProperByTabNames(string tableName);
    }
}
