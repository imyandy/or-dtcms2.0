using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.OleDb;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类:管理员
    /// </summary>
    public partial class manager
    {
        public manager()
        { }
        #region  Method
        /// <summary>
        /// 得到最大ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_manager order by id desc";
            object obj = DbHelperOleDb.GetSingle(conn, trans, strSql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_manager");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;
            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public bool Exists(string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_manager");
            strSql.Append(" where user_name=@user_name ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@user_name", OleDbType.VarChar,100)};
            parameters[0].Value = user_name;
            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.manager model)
        {
            int newId;
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into dt_manager(");
                        strSql.Append("role_id,role_type,user_name,user_pwd,real_name,telephone,email,is_lock,add_time)");
                        strSql.Append(" values (");
                        strSql.Append("@role_id,@role_type,@user_name,@user_pwd,@real_name,@telephone,@email,@is_lock,@add_time)");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@role_id", OleDbType.Integer,4),
					            new OleDbParameter("@role_type", OleDbType.Integer,4),
					            new OleDbParameter("@user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@user_pwd", OleDbType.VarChar,100),
					            new OleDbParameter("@real_name", OleDbType.VarChar,50),
					            new OleDbParameter("@telephone", OleDbType.VarChar,30),
					            new OleDbParameter("@email", OleDbType.VarChar,30),
					            new OleDbParameter("@is_lock", OleDbType.Integer,4),
					            new OleDbParameter("@add_time", OleDbType.Date)};
                        parameters[0].Value = model.role_id;
                        parameters[1].Value = model.role_type;
                        parameters[2].Value = model.user_name;
                        parameters[3].Value = model.user_pwd;
                        parameters[4].Value = model.real_name;
                        parameters[5].Value = model.telephone;
                        parameters[6].Value = model.email;
                        parameters[7].Value = model.is_lock;
                        parameters[8].Value = model.add_time;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //取得新插入的ID
                        newId = GetMaxId(conn, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        return -1;
                    }
                }
            }
            return newId;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.manager model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_manager set ");
            strSql.Append("role_id=@role_id,");
            strSql.Append("role_type=@role_type,");
            strSql.Append("user_name=@user_name,");
            strSql.Append("user_pwd=@user_pwd,");
            strSql.Append("real_name=@real_name,");
            strSql.Append("telephone=@telephone,");
            strSql.Append("email=@email,");
            strSql.Append("is_lock=@is_lock,");
            strSql.Append("add_time=@add_time");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@role_id", OleDbType.Integer,4),
					new OleDbParameter("@role_type", OleDbType.Integer,4),
					new OleDbParameter("@user_name", OleDbType.VarChar,100),
					new OleDbParameter("@user_pwd", OleDbType.VarChar,100),
					new OleDbParameter("@real_name", OleDbType.VarChar,50),
					new OleDbParameter("@telephone", OleDbType.VarChar,30),
					new OleDbParameter("@email", OleDbType.VarChar,30),
					new OleDbParameter("@is_lock", OleDbType.Integer,4),
					new OleDbParameter("@add_time", OleDbType.Date),
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = model.role_id;
            parameters[1].Value = model.role_type;
            parameters[2].Value = model.user_name;
            parameters[3].Value = model.user_pwd;
            parameters[4].Value = model.real_name;
            parameters[5].Value = model.telephone;
            parameters[6].Value = model.email;
            parameters[7].Value = model.is_lock;
            parameters[8].Value = model.add_time;
            parameters[9].Value = model.id;

            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            Hashtable sqllist = new Hashtable();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_manager_log ");
            strSql.Append(" where user_id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;
            sqllist.Add(strSql.ToString(), parameters);

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from dt_manager ");
            strSql1.Append(" where id=@id");
            OleDbParameter[] parameters1 = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters1[0].Value = id;
            sqllist.Add(strSql1.ToString(), parameters1);

            return DbHelperOleDb.ExecuteSqlTran(sqllist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.manager GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,role_id,role_type,user_name,user_pwd,real_name,telephone,email,is_lock,add_time from dt_manager ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            DTcms.Model.manager model = new DTcms.Model.manager();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["role_id"] != null && ds.Tables[0].Rows[0]["role_id"].ToString() != "")
                {
                    model.role_id = int.Parse(ds.Tables[0].Rows[0]["role_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["role_type"] != null && ds.Tables[0].Rows[0]["role_type"].ToString() != "")
                {
                    model.role_type = int.Parse(ds.Tables[0].Rows[0]["role_type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_name"] != null && ds.Tables[0].Rows[0]["user_name"].ToString() != "")
                {
                    model.user_name = ds.Tables[0].Rows[0]["user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["user_pwd"] != null && ds.Tables[0].Rows[0]["user_pwd"].ToString() != "")
                {
                    model.user_pwd = ds.Tables[0].Rows[0]["user_pwd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["real_name"] != null && ds.Tables[0].Rows[0]["real_name"].ToString() != "")
                {
                    model.real_name = ds.Tables[0].Rows[0]["real_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["telephone"] != null && ds.Tables[0].Rows[0]["telephone"].ToString() != "")
                {
                    model.telephone = ds.Tables[0].Rows[0]["telephone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["email"] != null && ds.Tables[0].Rows[0]["email"].ToString() != "")
                {
                    model.email = ds.Tables[0].Rows[0]["email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["is_lock"] != null && ds.Tables[0].Rows[0]["is_lock"].ToString() != "")
                {
                    model.is_lock = int.Parse(ds.Tables[0].Rows[0]["is_lock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["add_time"] != null && ds.Tables[0].Rows[0]["add_time"].ToString() != "")
                {
                    model.add_time = DateTime.Parse(ds.Tables[0].Rows[0]["add_time"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        public Model.manager GetModel(string user_name, string user_pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from dt_manager");
            strSql.Append(" where user_name=@user_name and user_pwd=@user_pwd and is_lock=0");
            OleDbParameter[] parameters = {
					new OleDbParameter("@user_name", OleDbType.VarChar,100),
                    new OleDbParameter("@user_pwd", OleDbType.VarChar,100)};
            parameters[0].Value = user_name;
            parameters[1].Value = user_pwd;

            object obj = DbHelperOleDb.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return GetModel(Convert.ToInt32(obj));
            }
            return null;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_id,role_type,user_name,user_pwd,real_name,telephone,email,is_lock,add_time ");
            strSql.Append(" FROM dt_manager ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by add_time desc");
            return DbHelperOleDb.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,role_id,role_type,user_name,user_pwd,real_name,telephone,email,is_lock,add_time ");
            strSql.Append(" FROM dt_manager ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperOleDb.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM dt_manager");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperOleDb.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperOleDb.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        #endregion  Method
    }
}