using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.IBLL
{
    public interface IBaseService<T> where T : class ,new()
    {
        /// <summary>
        /// 实现对数据库的添加功能,添加实现EF框架的引用
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回实体类型</returns>
        T AddEntity(T entity);

        /// <summary>
        /// 实现对数据库的修改功能
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回是否成功，如果成功，返回true，负责返回false</returns>
        bool UpdateEntity(T entity);

        /// <summary>
        /// 实现对数据的修改功能
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        int UpdateEntity();

        /// <summary>
        /// 实现对数据库的删除功能
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回是否成功，如果成功，返回true，负责返回false</returns>
        bool DeleteEntity(T entity);

        /// <summary>
        /// 实现对数据库的查询  --简单查询
        /// </summary>
        /// <param name="whereLambda">执行查询的条件</param>
        /// <returns>返回实体类的IQueryable集合</returns>
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);


        /// <summary>
        /// 实现对数据的分页查询
        /// </summary>
        /// <typeparam name="S">按照某个类进行排序</typeparam>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageSize">一页显示多少条数据</param>
        /// <param name="total">总条数</param>
        /// <param name="whereLambda">取得排序的条件</param>
        /// <param name="isAsc">如何排序，根据倒叙还是升序</param>
        /// <param name="orderByLambda">根据那个字段进行排序</param>
        /// <returns>返回实体类的IQueryable集合</returns>
        IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda,
                                          bool isAsc, Expression<Func<T, S>> orderByLambda);


        /* 执行SQL语句参考文档
         * https://msdn.microsoft.com/en-us/library/system.data.entity.core.objects.objectcontext(v=vs.113).aspx
         * 执行SQL语句参考文档
         */

        /// <summary>
        /// 执行Sql语句的方法  EF5.0的写法
        /// EF4.0的写法   int ExcuteSql(string strSql, ObjectParameter[] parameters);
        ///  <para>----------------------------------------------------------------------------------------------------------------------------------------------------------</para>
        ///  <para>   对数据库执行给定的 DDL/DML 命令。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。</para>
        ///  <para>   您可以在SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。</para>
        ///  <para>   context.Database.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); </para>
        ///  <para>  或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。</para>
        ///  <para>   context.Database.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor))</para>
        ///   <para>---------------------------------------------------------------------------------------------------------------------------------------------------------</para>
        /// </summary>
        /// <param name="strSQL">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回int类型</returns>
        int ExcuteSql(string strSQL, System.Data.Common.DbParameter[] parameters);


        /// <summary>
        /// 执行SQL语句或存储过程 返回IEnumerable&lt;T&gt;
        ///    <para>------------------------------------------------------------------------------------------------------------------------------------</para>
        ///    <para>创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。</para>
        ///    <para>该类型不必是实体类型。即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。</para>
        ///    <para>使用 System.Data.Entity.DbSet&lt;TEntity&gt;.SqlQuery(System.String,System.Object[])</para>
        ///    <para>方法可返回上下文跟踪的实体。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。</para>
        ///    <para>您可以在 SQL查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。</para>
        ///    <para>context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); </para>
        ///    <para>或者，您还可以构造一个 DbParameter并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。</para>
        ///    <para>context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));</para>
        ///    
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        IEnumerable<T> ExcuteSqlGetIEnumerable<T>(string strSQL, System.Data.Common.DbParameter[] parameters);





        /// <summary>
        ///     执行SQL语句或存储过程返回 IQueryable&lt;T&gt;
        ///     
        ///     <para>-------------------------------------------------------------------------------------------------------------------------------------</para>
        ///     <para>创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。</para>
        ///     <para>该类型不必是实体类型。即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。</para>
        ///     <para>使用 System.Data.Entity.DbSet&lt;TEntity&gt;.SqlQuery(System.String,System.Object[])</para>
        ///     <para>方法可返回上下文跟踪的实体。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。</para>
        ///     <para>您可以在 SQL查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。</para>
        ///     <para>context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); </para>
        ///     <para>或者，您还可以构造一个 DbParameter并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。</para>
        ///     <para>context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <returns>IQueryable&lt;T&gt;</returns>
        IQueryable<T> ExcuteSqlGetIQueryable<T>(string strSQL, System.Data.Common.DbParameter[] parameters);
 

    }
}
