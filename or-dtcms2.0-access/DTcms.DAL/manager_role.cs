using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 管理角色
    /// </summary>
    public partial class manager_role
    {
        public manager_role()
        { }
        #region  Method
        /// <summary>
        /// 得到最大ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_manager_role order by id desc";
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
            strSql.Append("select count(1) from dt_manager_role");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;
            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 返回角色名称
        /// </summary>
        public string GetTitle(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 role_name from dt_manager_role");
            strSql.Append(" where id=" + id);
            string title = Convert.ToString(DbHelperOleDb.GetSingle(strSql.ToString()));
            if (string.IsNullOrEmpty(title))
            {
                return "";
            }
            return title;
        }

        /// <summary>
        /// 增加一条数据,及其子表数据
        /// </summary>
        public int Add(Model.manager_role model)
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
                        strSql.Append("insert into dt_manager_role(");
                        strSql.Append("role_name,role_type)");
                        strSql.Append(" values (");
                        strSql.Append("@role_name,@role_type)");

                        OleDbParameter[] parameters = {
					            new OleDbParameter("@role_name", OleDbType.VarChar,100),
					            new OleDbParameter("@role_type", OleDbType.TinyInt,1)};
                        parameters[0].Value = model.role_name;
                        parameters[1].Value = model.role_type;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //取得新插入的ID
                        newId = GetMaxId(conn, trans);

                        StringBuilder strSql2;
                        foreach (Model.manager_role_value models in model.manager_role_values)
                        {
                            strSql2 = new StringBuilder();
                            strSql2.Append("insert into dt_manager_role_value(");
                            strSql2.Append("role_id,channel_name,channel_id,action_type)");
                            strSql2.Append(" values (");
                            strSql2.Append("@role_id,@channel_name,@channel_id,@action_type)");
                            OleDbParameter[] parameters2 = {
						            new OleDbParameter("@role_id", OleDbType.Integer,4),
						            new OleDbParameter("@channel_name", OleDbType.VarChar,255),
						            new OleDbParameter("@channel_id", OleDbType.Integer,4),
						            new OleDbParameter("@action_type", OleDbType.VarChar,100)};
                            parameters2[0].Value = newId;
                            parameters2[1].Value = models.channel_name;
                            parameters2[2].Value = models.channel_id;
                            parameters2[3].Value = models.action_type;

                            DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                        }
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
        /// 更新一条数据及其子表
        /// </summary>
        public bool Update(Model.manager_role model)
        {
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update dt_manager_role set ");
                        strSql.Append("role_name=@role_name,");
                        strSql.Append("role_type=@role_type");
                        strSql.Append(" where id=@id ");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@role_name", OleDbType.VarChar,100),
					            new OleDbParameter("@role_type", OleDbType.TinyInt,1),
                                new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters[0].Value = model.role_name;
                        parameters[1].Value = model.role_type;
                        parameters[2].Value = model.id;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //删除权限
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("delete from dt_manager_role_value where role_id=@role_id ");
                        StringBuilder idList = new StringBuilder();
                        if (model.manager_role_values != null)
                        {
                            foreach (Model.manager_role_value models in model.manager_role_values)
                            {
                                if (models.id > 0)
                                {
                                    idList.Append(models.id + ",");
                                }
                            }
                            string id_list = Utils.DelLastChar(idList.ToString(), ",");
                            if (!string.IsNullOrEmpty(id_list))
                            {
                                strSql2.Append(" and id not in(" + id_list + ")");
                            }
                        }
                        OleDbParameter[] parameters2 = {new OleDbParameter("@role_id", OleDbType.Integer,4)};
                        parameters2[0].Value = model.id;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);

                        //添加权限
                        if (model.manager_role_values != null)
                        {
                            StringBuilder strSql3;
                            foreach (Model.manager_role_value models in model.manager_role_values)
                            {
                                strSql3 = new StringBuilder();
                                if (models.id == 0)
                                {
                                    strSql3.Append("insert into dt_manager_role_value(");
                                    strSql3.Append("role_id,channel_name,channel_id,action_type)");
                                    strSql3.Append(" values (");
                                    strSql3.Append("@role_id,@channel_name,@channel_id,@action_type)");
                                    OleDbParameter[] parameters3 = {
						                    new OleDbParameter("@role_id", OleDbType.Integer,4),
						                    new OleDbParameter("@channel_name", OleDbType.VarChar,255),
						                    new OleDbParameter("@channel_id", OleDbType.Integer,4),
						                    new OleDbParameter("@action_type", OleDbType.VarChar,100)};
                                    parameters3[0].Value = model.id;
                                    parameters3[1].Value = models.channel_name;
                                    parameters3[2].Value = models.channel_id;
                                    parameters3[3].Value = models.action_type;

                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql3.ToString(), parameters3);
                                }
                            }
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 删除一条数据，及子表所有相关数据
        /// </summary>
        public bool Delete(int id)
        {
            Hashtable sqllist = new Hashtable();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_manager_role ");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;
            sqllist.Add(strSql.ToString(), parameters);

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from dt_manager_role_value ");
            strSql2.Append(" where role_id=@role_id ");
            OleDbParameter[] parameters2 = {
					new OleDbParameter("@role_id", OleDbType.Integer,4)};
            parameters2[0].Value = id;
            sqllist.Add(strSql2.ToString(), parameters2);

            return DbHelperOleDb.ExecuteSqlTran(sqllist);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DTcms.Model.manager_role GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_name,role_type from dt_manager_role ");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            DTcms.Model.manager_role model = new DTcms.Model.manager_role();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region  父表信息
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                model.role_name = ds.Tables[0].Rows[0]["role_name"].ToString();
                if (ds.Tables[0].Rows[0]["role_type"].ToString() != "")
                {
                    model.role_type = int.Parse(ds.Tables[0].Rows[0]["role_type"].ToString());
                }
                #endregion  父表信息end

                #region  子表信息
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("select id,role_id,channel_name,channel_id,action_type from dt_manager_role_value ");
                strSql2.Append(" where role_id=@role_id ");
                OleDbParameter[] parameters2 = {
					new OleDbParameter("@role_id", OleDbType.Integer,4)};
                parameters2[0].Value = id;

                DataSet ds2 = DbHelperOleDb.Query(strSql2.ToString(), parameters2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    #region  子表字段信息
                    int i = ds2.Tables[0].Rows.Count;
                    List<DTcms.Model.manager_role_value> models = new List<DTcms.Model.manager_role_value>();
                    DTcms.Model.manager_role_value modelt;
                    for (int n = 0; n < i; n++)
                    {
                        modelt = new DTcms.Model.manager_role_value();
                        if (ds2.Tables[0].Rows[n]["id"].ToString() != "")
                        {
                            modelt.id = int.Parse(ds2.Tables[0].Rows[n]["id"].ToString());
                        }
                        if (ds2.Tables[0].Rows[n]["role_id"].ToString() != "")
                        {
                            modelt.role_id = int.Parse(ds2.Tables[0].Rows[n]["role_id"].ToString());
                        }
                        modelt.channel_name = ds2.Tables[0].Rows[n]["channel_name"].ToString();
                        if (ds2.Tables[0].Rows[n]["channel_id"].ToString() != "")
                        {
                            modelt.channel_id = int.Parse(ds2.Tables[0].Rows[n]["channel_id"].ToString());
                        }
                        modelt.action_type = ds2.Tables[0].Rows[n]["action_type"].ToString();
                        models.Add(modelt);
                    }
                    model.manager_role_values = models;
                    #endregion  子表字段信息end
                }
                #endregion  子表信息end

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_name,role_type ");
            strSql.Append(" FROM dt_manager_role ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperOleDb.Query(strSql.ToString());
        }

        #endregion  Method
    }
}