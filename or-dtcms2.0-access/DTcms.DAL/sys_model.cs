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
	/// ϵͳģ��
	/// </summary>
    public partial class sys_model
    {
        public sys_model()
        { }
        #region  Method
        /// <summary>
        /// �õ����ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_sys_model order by id desc";
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
            strSql.Append("select count(1) from dt_sys_model");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������,�����ӱ�����
        /// </summary>
        public int Add(Model.sys_model model)
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
                        strSql.Append("insert into dt_sys_model(");
                        strSql.Append("title,sort_id,inherit_index,inherit_list,inherit_detail,is_sys)");
                        strSql.Append(" values (");
                        strSql.Append("@title,@sort_id,@inherit_index,@inherit_list,@inherit_detail,@is_sys)");
                        OleDbParameter[] parameters = {
					    new OleDbParameter("@title", OleDbType.VarChar,100),
					    new OleDbParameter("@sort_id", OleDbType.Integer,4),
					    new OleDbParameter("@inherit_index", OleDbType.VarChar,255),
					    new OleDbParameter("@inherit_list", OleDbType.VarChar,255),
					    new OleDbParameter("@inherit_detail", OleDbType.VarChar,255),
					    new OleDbParameter("@is_sys", OleDbType.TinyInt,1)};
                        parameters[0].Value = model.title;
                        parameters[1].Value = model.sort_id;
                        parameters[2].Value = model.inherit_index;
                        parameters[3].Value = model.inherit_list;
                        parameters[4].Value = model.inherit_detail;
                        parameters[5].Value = model.is_sys;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //ȡ���²����ID
                        newId = GetMaxId(conn, trans);

                        //ģ�Ͳ˵�
                        if (model.sys_model_navs != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.sys_model_nav models in model.sys_model_navs)
                            {
                                strSql2 = new StringBuilder();
                                strSql2.Append("insert into dt_sys_model_nav(");
                                strSql2.Append("model_id,title,nav_url,sort_id)");
                                strSql2.Append(" values (");
                                strSql2.Append("@model_id,@title,@nav_url,@sort_id)");
                                OleDbParameter[] parameters2 = {
						        new OleDbParameter("@model_id", OleDbType.Integer,4),
						        new OleDbParameter("@title", OleDbType.VarChar,100),
						        new OleDbParameter("@nav_url", OleDbType.VarChar,255),
						        new OleDbParameter("@sort_id", OleDbType.Integer,4)};
                                parameters2[0].Value = newId;
                                parameters2[1].Value = models.title;
                                parameters2[2].Value = models.nav_url;
                                parameters2[3].Value = models.sort_id;

                                DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                            }
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
        /// ����һ������
        /// </summary>
        public bool Update(Model.sys_model model)
        {
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update dt_sys_model set ");
                        strSql.Append("title=@title,");
                        strSql.Append("sort_id=@sort_id,");
                        strSql.Append("inherit_index=@inherit_index,");
                        strSql.Append("inherit_list=@inherit_list,");
                        strSql.Append("inherit_detail=@inherit_detail,");
                        strSql.Append("is_sys=@is_sys");
                        strSql.Append(" where id=@id");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@title", OleDbType.VarChar,100),
					            new OleDbParameter("@sort_id", OleDbType.Integer,4),
					            new OleDbParameter("@inherit_index", OleDbType.VarChar,255),
					            new OleDbParameter("@inherit_list", OleDbType.VarChar,255),
					            new OleDbParameter("@inherit_detail", OleDbType.VarChar,255),
					            new OleDbParameter("@is_sys", OleDbType.TinyInt,1),
					            new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters[0].Value = model.title;
                        parameters[1].Value = model.sort_id;
                        parameters[2].Value = model.inherit_index;
                        parameters[3].Value = model.inherit_list;
                        parameters[4].Value = model.inherit_detail;
                        parameters[5].Value = model.is_sys;
                        parameters[6].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //ɾ����ɾ���Ĳ˵�
                        new DAL.sys_model_nav().DeleteList(conn, trans, model.sys_model_navs, model.id);

                        //���/�޸Ĳ˵�
                        if (model.sys_model_navs != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.sys_model_nav models in model.sys_model_navs)
                            {
                                strSql2 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql2.Append("update dt_sys_model_nav set ");
                                    strSql2.Append("model_id=@model_id,");
                                    strSql2.Append("title=@title,");
                                    strSql2.Append("nav_url=@nav_url,");
                                    strSql2.Append("sort_id=@sort_id");
                                    strSql2.Append(" where id=@id");
                                    OleDbParameter[] parameters2 = {
                                            new OleDbParameter("@model_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@nav_url", OleDbType.VarChar,255),
					                        new OleDbParameter("@sort_id", OleDbType.Integer,4),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.model_id;
                                    parameters2[1].Value = models.title;
                                    parameters2[2].Value = models.nav_url;
                                    parameters2[3].Value = models.sort_id;
                                    parameters2[4].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                                else
                                {
                                    strSql2.Append("insert into dt_sys_model_nav(");
                                    strSql2.Append("model_id,title,nav_url,sort_id)");
                                    strSql2.Append(" values (");
                                    strSql2.Append("@model_id,@title,@nav_url,@sort_id)");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@model_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@nav_url", OleDbType.VarChar,255),
					                        new OleDbParameter("@sort_id", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.model_id;
                                    parameters2[1].Value = models.title;
                                    parameters2[2].Value = models.nav_url;
                                    parameters2[3].Value = models.sort_id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
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
        /// ɾ��һ�����ݣ����ӱ������������
        /// </summary>
        public bool Delete(int id)
        {
            Hashtable sqllist = new Hashtable();
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from dt_sys_model_nav ");
            strSql2.Append(" where model_id=@model_id ");
            OleDbParameter[] parameters2 = {
					new OleDbParameter("@model_id", OleDbType.Integer,4)};
            parameters2[0].Value = id;
            sqllist.Add(strSql2.ToString(), parameters2);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_sys_model ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;
            sqllist.Add(strSql.ToString(), parameters);

            return DbHelperOleDb.ExecuteSqlTran(sqllist);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.sys_model GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,title,sort_id,inherit_index,inherit_list,inherit_detail,is_sys from dt_sys_model ");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            Model.sys_model model = new Model.sys_model();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region  ������Ϣ
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["title"] != null && ds.Tables[0].Rows[0]["title"].ToString() != "")
                {
                    model.title = ds.Tables[0].Rows[0]["title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sort_id"] != null && ds.Tables[0].Rows[0]["sort_id"].ToString() != "")
                {
                    model.sort_id = int.Parse(ds.Tables[0].Rows[0]["sort_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["inherit_index"] != null && ds.Tables[0].Rows[0]["inherit_index"].ToString() != "")
                {
                    model.inherit_index = ds.Tables[0].Rows[0]["inherit_index"].ToString();
                }
                if (ds.Tables[0].Rows[0]["inherit_list"] != null && ds.Tables[0].Rows[0]["inherit_list"].ToString() != "")
                {
                    model.inherit_list = ds.Tables[0].Rows[0]["inherit_list"].ToString();
                }
                if (ds.Tables[0].Rows[0]["inherit_detail"] != null && ds.Tables[0].Rows[0]["inherit_detail"].ToString() != "")
                {
                    model.inherit_detail = ds.Tables[0].Rows[0]["inherit_detail"].ToString();
                }
                if (ds.Tables[0].Rows[0]["is_sys"] != null && ds.Tables[0].Rows[0]["is_sys"].ToString() != "")
                {
                    model.is_sys = int.Parse(ds.Tables[0].Rows[0]["is_sys"].ToString());
                }
                #endregion  ������Ϣend

                #region  �ӱ���Ϣ
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("select id,model_id,title,nav_url,sort_id from dt_sys_model_nav ");
                strSql2.Append(" where model_id=@model_id ");
                OleDbParameter[] parameters2 = {
					new OleDbParameter("@model_id", OleDbType.Integer,4)};
                parameters2[0].Value = id;

                DataSet ds2 = DbHelperOleDb.Query(strSql2.ToString(), parameters2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    #region  �ӱ��ֶ���Ϣ
                    int i = ds2.Tables[0].Rows.Count;
                    List<Model.sys_model_nav> models = new List<Model.sys_model_nav>();
                    Model.sys_model_nav modelt;
                    for (int n = 0; n < i; n++)
                    {
                        modelt = new Model.sys_model_nav();
                        if (ds2.Tables[0].Rows[n]["id"] != null && ds2.Tables[0].Rows[n]["id"].ToString() != "")
                        {
                            modelt.id = int.Parse(ds2.Tables[0].Rows[n]["id"].ToString());
                        }
                        if (ds2.Tables[0].Rows[n]["model_id"] != null && ds2.Tables[0].Rows[n]["model_id"].ToString() != "")
                        {
                            modelt.model_id = int.Parse(ds2.Tables[0].Rows[n]["model_id"].ToString());
                        }
                        if (ds2.Tables[0].Rows[n]["title"] != null && ds2.Tables[0].Rows[n]["title"].ToString() != "")
                        {
                            modelt.title = ds2.Tables[0].Rows[n]["title"].ToString();
                        }
                        if (ds2.Tables[0].Rows[n]["nav_url"] != null && ds2.Tables[0].Rows[n]["nav_url"].ToString() != "")
                        {
                            modelt.nav_url = ds2.Tables[0].Rows[n]["nav_url"].ToString();
                        }
                        if (ds2.Tables[0].Rows[n]["sort_id"] != null && ds2.Tables[0].Rows[n]["sort_id"].ToString() != "")
                        {
                            modelt.sort_id = int.Parse(ds2.Tables[0].Rows[n]["sort_id"].ToString());
                        }
                        models.Add(modelt);
                    }
                    model.sys_model_navs = models;
                    #endregion  �ӱ��ֶ���Ϣend
                }
                #endregion  �ӱ���Ϣend

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,title,sort_id,inherit_index,inherit_list,inherit_detail,is_sys ");
            strSql.Append(" FROM dt_sys_model ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by sort_id asc,id desc");
            return DbHelperOleDb.Query(strSql.ToString());
        }


        #endregion  Method
    }

    /// <summary>
    /// ϵͳģ�Ͳ˵�=============================================================
    /// </summary>
    public partial class sys_model_nav
    {
        public sys_model_nav()
        { }
        #region  Method
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_sys_model_nav");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            return DbHelperOleDb.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,model_id,title,nav_url,sort_id ");
            strSql.Append(" FROM dt_sys_model_nav ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperOleDb.Query(strSql.ToString());
        }

        /// <summary>
        /// ���Ҳ����ڵĲ˵���ɾ��
        /// </summary>
        public void DeleteList(OleDbConnection conn, OleDbTransaction trans, List<Model.sys_model_nav> models, int model_id)
        {
            StringBuilder idList = new StringBuilder();
            if (models != null)
            {
                foreach (Model.sys_model_nav modelt in models)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
            }
            string id_list = Utils.DelLastChar(idList.ToString(), ",");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_sys_model_nav where model_id=" + model_id);
            if (!string.IsNullOrEmpty(id_list))
            {
                strSql.Append(" and id not in(" + id_list + ")");
            }
            DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString());
        }

        #endregion  Method
    }
}

