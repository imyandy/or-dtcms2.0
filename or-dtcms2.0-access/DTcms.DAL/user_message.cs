using System;
using System.Data;
using System.Text;
using System.Data.OleDb;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// �û�����Ϣ
    /// </summary>
    public partial class user_message
    {
        public user_message()
        { }
        #region  Method
        /// <summary>
        /// �õ����ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_user_message order by id desc";
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_user_message");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���ؼ�¼����
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from dt_user_message ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperOleDb.GetSingle(strSql.ToString()));
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.user_message model)
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
                        strSql.Append("insert into dt_user_message(");
                        strSql.Append("[type],post_user_name,accept_user_name,is_read,title,content,post_time,read_time)");
                        strSql.Append(" values (");
                        strSql.Append("@type,@post_user_name,@accept_user_name,@is_read,@title,@content,@post_time,@read_time)");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@type", OleDbType.TinyInt,1),
					            new OleDbParameter("@post_user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@accept_user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@is_read", OleDbType.TinyInt,1),
					            new OleDbParameter("@title", OleDbType.VarChar,100),
					            new OleDbParameter("@content", OleDbType.VarChar),
					            new OleDbParameter("@post_time", OleDbType.Date),
					            new OleDbParameter("@read_time", OleDbType.Date)};
                        parameters[0].Value = model.type;
                        parameters[1].Value = model.post_user_name;
                        parameters[2].Value = model.accept_user_name;
                        parameters[3].Value = model.is_read;
                        parameters[4].Value = model.title;
                        parameters[5].Value = model.content;
                        parameters[6].Value = model.post_time;
                        if (model.read_time != null)
                        {
                            parameters[7].Value = model.read_time;
                        }
                        else
                        {
                            parameters[7].Value = DBNull.Value;
                        }
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //ȡ���²����ID
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
        /// �޸�һ������
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_user_message set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperOleDb.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.user_message model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_user_message set ");
            strSql.Append("[type]=@type,");
            strSql.Append("post_user_name=@post_user_name,");
            strSql.Append("accept_user_name=@accept_user_name,");
            strSql.Append("is_read=@is_read,");
            strSql.Append("title=@title,");
            strSql.Append("content=@content,");
            strSql.Append("post_time=@post_time,");
            strSql.Append("read_time=@read_time");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@type", OleDbType.TinyInt,1),
					new OleDbParameter("@post_user_name", OleDbType.VarChar,100),
					new OleDbParameter("@accept_user_name", OleDbType.VarChar,100),
					new OleDbParameter("@is_read", OleDbType.TinyInt,1),
					new OleDbParameter("@title", OleDbType.VarChar,100),
					new OleDbParameter("@content", OleDbType.VarChar),
					new OleDbParameter("@post_time", OleDbType.Date),
					new OleDbParameter("@read_time", OleDbType.Date),
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = model.type;
            parameters[1].Value = model.post_user_name;
            parameters[2].Value = model.accept_user_name;
            parameters[3].Value = model.is_read;
            parameters[4].Value = model.title;
            parameters[5].Value = model.content;
            parameters[6].Value = model.post_time;
            if (model.read_time != null)
            {
                parameters[7].Value = model.read_time;
            }
            else
            {
                parameters[7].Value = DBNull.Value;
            }
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
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_user_message ");
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
        public bool Delete(int id, string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_user_message ");
            strSql.Append(" where id=@id and (post_user_name=@post_user_name or accept_user_name=@accept_user_name)");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4),
                    new OleDbParameter("@post_user_name", OleDbType.VarChar,100),
                    new OleDbParameter("@accept_user_name", OleDbType.VarChar,100)};
            parameters[0].Value = id;
            parameters[1].Value = user_name;
            parameters[2].Value = user_name;

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
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.user_message GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,[type],post_user_name,accept_user_name,is_read,title,content,post_time,read_time from dt_user_message ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            DTcms.Model.user_message model = new DTcms.Model.user_message();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["type"] != null && ds.Tables[0].Rows[0]["type"].ToString() != "")
                {
                    model.type = int.Parse(ds.Tables[0].Rows[0]["type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["post_user_name"] != null && ds.Tables[0].Rows[0]["post_user_name"].ToString() != "")
                {
                    model.post_user_name = ds.Tables[0].Rows[0]["post_user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["accept_user_name"] != null && ds.Tables[0].Rows[0]["accept_user_name"].ToString() != "")
                {
                    model.accept_user_name = ds.Tables[0].Rows[0]["accept_user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["is_read"] != null && ds.Tables[0].Rows[0]["is_read"].ToString() != "")
                {
                    model.is_read = int.Parse(ds.Tables[0].Rows[0]["is_read"].ToString());
                }
                if (ds.Tables[0].Rows[0]["title"] != null && ds.Tables[0].Rows[0]["title"].ToString() != "")
                {
                    model.title = ds.Tables[0].Rows[0]["title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["content"] != null && ds.Tables[0].Rows[0]["content"].ToString() != "")
                {
                    model.content = ds.Tables[0].Rows[0]["content"].ToString();
                }
                if (ds.Tables[0].Rows[0]["post_time"] != null && ds.Tables[0].Rows[0]["post_time"].ToString() != "")
                {
                    model.post_time = DateTime.Parse(ds.Tables[0].Rows[0]["post_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["read_time"] != null && ds.Tables[0].Rows[0]["read_time"].ToString() != "")
                {
                    model.read_time = DateTime.Parse(ds.Tables[0].Rows[0]["read_time"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,[type],post_user_name,accept_user_name,is_read,title,content,post_time,read_time ");
            strSql.Append(" FROM dt_user_message ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperOleDb.Query(strSql.ToString());
        }

        /// <summary>
        /// ��ò�ѯ��ҳ����
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM dt_user_message");
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