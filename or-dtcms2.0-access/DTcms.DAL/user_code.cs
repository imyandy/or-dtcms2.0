using System;
using System.Data;
using System.Text;
using System.Data.OleDb;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 用户生成码
    /// </summary>
    public partial class user_code
    {
        public user_code()
        { }
        #region  Method
        /// <summary>
        /// 得到最大ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_user_code order by id desc";
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
            strSql.Append("select count(1) from dt_user_code");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string type, string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_user_code");
            strSql.Append(" where status=0 and datediff(\"d\",eff_time,now())<=0 and [type]=@type and user_name=@user_name");
            OleDbParameter[] parameters = {
					new OleDbParameter("@type", OleDbType.VarChar,20),
                    new OleDbParameter("@user_name", OleDbType.VarChar,100)};
            parameters[0].Value = type;
            parameters[1].Value = user_name;

            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 返回数据总数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from dt_user_code ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperOleDb.GetSingle(strSql.ToString()));
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.user_code model)
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
                        strSql.Append("insert into dt_user_code(");
                        strSql.Append("user_id,user_name,[type],str_code,[count],status,eff_time,add_time)");
                        strSql.Append(" values (");
                        strSql.Append("@user_id,@user_name,@type,@str_code,@count,@status,@eff_time,@add_time)");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@user_id", OleDbType.Integer,4),
					            new OleDbParameter("@user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@type", OleDbType.VarChar,20),
					            new OleDbParameter("@str_code", OleDbType.VarChar,255),
                                new OleDbParameter("@count", OleDbType.Integer,4),
					            new OleDbParameter("@status", OleDbType.TinyInt,1),
					            new OleDbParameter("@eff_time", OleDbType.Date),
					            new OleDbParameter("@add_time", OleDbType.Date)};
                        parameters[0].Value = model.user_id;
                        parameters[1].Value = model.user_name;
                        parameters[2].Value = model.type;
                        parameters[3].Value = model.str_code;
                        parameters[4].Value = model.count;
                        parameters[5].Value = model.status;
                        parameters[6].Value = model.eff_time;
                        parameters[7].Value = model.add_time;

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
        public bool Update(Model.user_code model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_user_code set ");
            strSql.Append("user_id=@user_id,");
            strSql.Append("user_name=@user_name,");
            strSql.Append("[type]=@type,");
            strSql.Append("str_code=@str_code,");
            strSql.Append("[count]=@count,");
            strSql.Append("status=@status,");
            strSql.Append("eff_time=@eff_time,");
            strSql.Append("add_time=@add_time");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@user_id", OleDbType.Integer,4),
					new OleDbParameter("@user_name", OleDbType.VarChar,100),
					new OleDbParameter("@type", OleDbType.VarChar,20),
					new OleDbParameter("@str_code", OleDbType.VarChar,255),
                    new OleDbParameter("@count", OleDbType.Integer,4),
					new OleDbParameter("@status", OleDbType.TinyInt,1),
					new OleDbParameter("@eff_time", OleDbType.Date),
					new OleDbParameter("@add_time", OleDbType.Date),
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.user_name;
            parameters[2].Value = model.type;
            parameters[3].Value = model.str_code;
            parameters[4].Value = model.count;
            parameters[5].Value = model.status;
            parameters[6].Value = model.eff_time;
            parameters[7].Value = model.add_time;
            parameters[8].Value = model.id;

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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_user_code ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

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
        /// 根据条件批量删除
        /// </summary>
        public bool Delete(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_user_code ");
            strSql.Append(" where " + strWhere);
            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString());
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
        /// 得到一个对象实体
        /// </summary>
        public Model.user_code GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,user_id,user_name,[type],str_code,[count],status,eff_time,add_time from dt_user_code ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            Model.user_code model = new Model.user_code();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_id"] != null && ds.Tables[0].Rows[0]["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_name"] != null && ds.Tables[0].Rows[0]["user_name"].ToString() != "")
                {
                    model.user_name = ds.Tables[0].Rows[0]["user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["type"] != null && ds.Tables[0].Rows[0]["type"].ToString() != "")
                {
                    model.type = ds.Tables[0].Rows[0]["type"].ToString();
                }
                if (ds.Tables[0].Rows[0]["str_code"] != null && ds.Tables[0].Rows[0]["str_code"].ToString() != "")
                {
                    model.str_code = ds.Tables[0].Rows[0]["str_code"].ToString();
                }
                if (ds.Tables[0].Rows[0]["count"] != null && ds.Tables[0].Rows[0]["count"].ToString() != "")
                {
                    model.count = int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
                }
                if (ds.Tables[0].Rows[0]["status"] != null && ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    model.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["eff_time"] != null && ds.Tables[0].Rows[0]["eff_time"].ToString() != "")
                {
                    model.eff_time = DateTime.Parse(ds.Tables[0].Rows[0]["eff_time"].ToString());
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
        /// 根据生成码得到一个对象实体
        /// </summary>
        public Model.user_code GetModel(string str_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id from dt_user_code");
            strSql.Append(" where status=0 and datediff(\"d\",eff_time,now())<=0 and str_code=@str_code");
            OleDbParameter[] parameters = {
					new OleDbParameter("@str_code", OleDbType.VarChar,255)};
            parameters[0].Value = str_code;

            object obj = DbHelperOleDb.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return GetModel(Convert.ToInt32(obj));
            }
            return null;
        }

        /// <summary>
        /// 根据用户名得到一个对象实体
        /// </summary>
        public Model.user_code GetModel(string user_name, string code_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id from dt_user_code");
            strSql.Append(" where status=0 and datediff(\"d\",eff_time,now())<=0 and user_name=@user_name and [type]=@type");
            OleDbParameter[] parameters = {
					new OleDbParameter("@user_name", OleDbType.VarChar,100),
                    new OleDbParameter("@type", OleDbType.VarChar,20)};
            parameters[0].Value = user_name;
            parameters[1].Value = code_type;

            object obj = DbHelperOleDb.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return GetModel(Convert.ToInt32(obj));
            }
            return null;
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
            strSql.Append(" id,user_id,user_name,[type],str_code,[count],status,eff_time,add_time ");
            strSql.Append(" FROM dt_user_code ");
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
            strSql.Append("select * FROM dt_user_code");
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