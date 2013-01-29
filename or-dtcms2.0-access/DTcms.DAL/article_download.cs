using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.OleDb;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    public partial class article
    {
        #region Method
        /// <summary>
        /// �޸����ظ���һ������
        /// </summary>
        public void UpdateDownloadField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_article_download set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperOleDb.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// ����һ������,�����ӱ�����
        /// </summary>
        public int Add(Model.article_download model)
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
                        strSql.Append("insert into dt_article(");
                        strSql.Append("channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time)");
                        strSql.Append(" values (");
                        strSql.Append("@channel_id,@category_id,@title,@link_url,@img_url,@seo_title,@seo_keywords,@seo_description,@content,@sort_id,@click,@is_lock,@user_id,@add_time)");
                        OleDbParameter[] parameters = {
					    new OleDbParameter("@channel_id", OleDbType.Integer,4),
					    new OleDbParameter("@category_id", OleDbType.Integer,4),
					    new OleDbParameter("@title", OleDbType.VarChar,100),
					    new OleDbParameter("@link_url", OleDbType.VarChar,255),
					    new OleDbParameter("@img_url", OleDbType.VarChar,255),
					    new OleDbParameter("@seo_title", OleDbType.VarChar,255),
					    new OleDbParameter("@seo_keywords", OleDbType.VarChar,255),
					    new OleDbParameter("@seo_description", OleDbType.VarChar,255),
					    new OleDbParameter("@content", OleDbType.VarChar),
					    new OleDbParameter("@sort_id", OleDbType.Integer,4),
					    new OleDbParameter("@click", OleDbType.Integer,4),
					    new OleDbParameter("@is_lock", OleDbType.TinyInt,1),
					    new OleDbParameter("@user_id", OleDbType.Integer,4),
					    new OleDbParameter("@add_time", OleDbType.Date)};
                        parameters[0].Value = model.channel_id;
                        parameters[1].Value = model.category_id;
                        parameters[2].Value = model.title;
                        parameters[3].Value = model.link_url;
                        parameters[4].Value = model.img_url;
                        parameters[5].Value = model.seo_title;
                        parameters[6].Value = model.seo_keywords;
                        parameters[7].Value = model.seo_description;
                        parameters[8].Value = model.content;
                        parameters[9].Value = model.sort_id;
                        parameters[10].Value = model.click;
                        parameters[11].Value = model.is_lock;
                        parameters[12].Value = model.user_id;
                        parameters[13].Value = model.add_time;

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //ȡ���²����ID
                        newId = GetMaxId(conn, trans);
                        //������Ϣ
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("insert into dt_article_download(");
                        strSql2.Append("id,is_msg,is_red)");
                        strSql2.Append(" values (");
                        strSql2.Append("@id,@is_msg,@is_red)");
                        OleDbParameter[] parameters2 = {
					    new OleDbParameter("@id", OleDbType.Integer,4),
					    new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_red", OleDbType.TinyInt,1)};
                        parameters2[0].Value = newId;
                        parameters2[1].Value = model.is_msg;
                        parameters2[2].Value = model.is_red;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);

                        //���Ͳ�
                        StringBuilder strSql3 = new StringBuilder();
                        strSql3.Append("insert into dt_article_diggs(");
                        strSql3.Append("id,digg_good,digg_bad)");
                        strSql3.Append(" values (");
                        strSql3.Append("@id,@digg_good,@digg_bad)");
                        OleDbParameter[] parameters3 = {
					    new OleDbParameter("@id", OleDbType.Integer,4),
					    new OleDbParameter("@digg_good", OleDbType.Integer,4),
					    new OleDbParameter("@digg_bad", OleDbType.Integer,4)};
                        parameters3[0].Value = newId;
                        parameters3[1].Value = model.digg_good;
                        parameters3[2].Value = model.digg_bad;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql3.ToString(), parameters3);

                        //���ظ���
                        if (model.download_attachs != null)
                        {
                            StringBuilder strSql4;
                            foreach (Model.download_attach models in model.download_attachs)
                            {
                                strSql4 = new StringBuilder();
                                strSql4.Append("insert into dt_download_attach(");
                                strSql4.Append("article_id,title,file_path,file_ext,file_size,down_num,point)");
                                strSql4.Append(" values (");
                                strSql4.Append("@article_id,@title,@file_path,@file_ext,@file_size,@down_num,@point)");
                                OleDbParameter[] parameters4 = {
					            new OleDbParameter("@article_id", OleDbType.Integer,4),
					            new OleDbParameter("@title", OleDbType.VarChar,255),
					            new OleDbParameter("@file_path", OleDbType.VarChar,255),
					            new OleDbParameter("@file_ext", OleDbType.VarChar,100),
					            new OleDbParameter("@file_size", OleDbType.Integer,4),
					            new OleDbParameter("@down_num", OleDbType.Integer,4),
					            new OleDbParameter("@point", OleDbType.Integer,4)};
                                parameters4[0].Value = newId;
                                parameters4[1].Value = models.title;
                                parameters4[2].Value = models.file_path;
                                parameters4[3].Value = models.file_ext;
                                parameters4[4].Value = models.file_size;
                                parameters4[5].Value = models.down_num;
                                parameters4[6].Value = models.point;
                                DbHelperOleDb.ExecuteSql(conn, trans, strSql4.ToString(), parameters4);
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
        public bool Update(Model.article_download model)
        {
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update dt_article set ");
                        strSql.Append("channel_id=@channel_id,");
                        strSql.Append("category_id=@category_id,");
                        strSql.Append("title=@title,");
                        strSql.Append("link_url=@link_url,");
                        strSql.Append("img_url=@img_url,");
                        strSql.Append("seo_title=@seo_title,");
                        strSql.Append("seo_keywords=@seo_keywords,");
                        strSql.Append("seo_description=@seo_description,");
                        strSql.Append("content=@content,");
                        strSql.Append("sort_id=@sort_id,");
                        strSql.Append("click=@click,");
                        strSql.Append("is_lock=@is_lock,");
                        strSql.Append("user_id=@user_id,");
                        strSql.Append("add_time=@add_time");
                        strSql.Append(" where id=@id");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@channel_id", OleDbType.Integer,4),
					            new OleDbParameter("@category_id", OleDbType.Integer,4),
					            new OleDbParameter("@title", OleDbType.VarChar,100),
					            new OleDbParameter("@link_url", OleDbType.VarChar,255),
					            new OleDbParameter("@img_url", OleDbType.VarChar,255),
					            new OleDbParameter("@seo_title", OleDbType.VarChar,255),
					            new OleDbParameter("@seo_keywords", OleDbType.VarChar,255),
					            new OleDbParameter("@seo_description", OleDbType.VarChar,255),
					            new OleDbParameter("@content", OleDbType.VarChar),
					            new OleDbParameter("@sort_id", OleDbType.Integer,4),
					            new OleDbParameter("@click", OleDbType.Integer,4),
					            new OleDbParameter("@is_lock", OleDbType.TinyInt,1),
					            new OleDbParameter("@user_id", OleDbType.Integer,4),
					            new OleDbParameter("@add_time", OleDbType.Date),
					            new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters[0].Value = model.channel_id;
                        parameters[1].Value = model.category_id;
                        parameters[2].Value = model.title;
                        parameters[3].Value = model.link_url;
                        parameters[4].Value = model.img_url;
                        parameters[5].Value = model.seo_title;
                        parameters[6].Value = model.seo_keywords;
                        parameters[7].Value = model.seo_description;
                        parameters[8].Value = model.content;
                        parameters[9].Value = model.sort_id;
                        parameters[10].Value = model.click;
                        parameters[11].Value = model.is_lock;
                        parameters[12].Value = model.user_id;
                        parameters[13].Value = model.add_time;
                        parameters[14].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //�޸ĸ���
                        StringBuilder strSql21 = new StringBuilder();
                        strSql21.Append("update dt_article_download set ");
                        strSql21.Append("id=@id,");
                        strSql21.Append("is_msg=@is_msg,");
                        strSql21.Append("is_red=@is_red");
                        strSql21.Append(" where id=@id ");
                        OleDbParameter[] parameters21 = {
					            new OleDbParameter("@id", OleDbType.Integer,4),
					            new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_red", OleDbType.TinyInt,1)};
                        parameters21[0].Value = model.id;
                        parameters21[1].Value = model.is_msg;
                        parameters21[2].Value = model.is_red;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql21.ToString(), parameters21);

                        //�޸Ķ��Ͳ�
                        StringBuilder strSql22 = new StringBuilder();
                        strSql22.Append("update dt_article_diggs set ");
                        strSql22.Append("digg_good=@digg_good,");
                        strSql22.Append("digg_bad=@digg_bad");
                        strSql22.Append(" where id=@id ");
                        OleDbParameter[] parameters22 = {
					            new OleDbParameter("@digg_good", OleDbType.Integer,4),
					            new OleDbParameter("@digg_bad", OleDbType.Integer,4),
                                new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters22[0].Value = model.digg_good;
                        parameters22[1].Value = model.digg_bad;
                        parameters22[2].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql22.ToString(), parameters22);

                        //ɾ����ɾ���ĸ���
                        new download_attach().DeleteList(conn, trans, model.download_attachs, model.id);
                        // ���/�޸ĸ���
                        if (model.download_attachs != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.download_attach models in model.download_attachs)
                            {
                                strSql2 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql2.Append("update dt_download_attach set ");
                                    strSql2.Append("article_id=@article_id,");
                                    strSql2.Append("title=@title,");
                                    strSql2.Append("file_path=@file_path,");
                                    strSql2.Append("file_ext=@file_ext,");
                                    strSql2.Append("file_size=@file_size,");
                                    strSql2.Append("down_num=@down_num,");
                                    strSql2.Append("point=@point");
                                    strSql2.Append(" where id=@id");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,255),
					                        new OleDbParameter("@file_path", OleDbType.VarChar,255),
					                        new OleDbParameter("@file_ext", OleDbType.VarChar,100),
					                        new OleDbParameter("@file_size", OleDbType.Integer,4),
					                        new OleDbParameter("@down_num", OleDbType.Integer,4),
					                        new OleDbParameter("@point", OleDbType.Integer,4),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.article_id;
                                    parameters2[1].Value = models.title;
                                    parameters2[2].Value = models.file_path;
                                    parameters2[3].Value = models.file_ext;
                                    parameters2[4].Value = models.file_size;
                                    parameters2[5].Value = models.down_num;
                                    parameters2[6].Value = models.point;
                                    parameters2[7].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                                else
                                {
                                    strSql2.Append("insert into dt_download_attach(");
                                    strSql2.Append("article_id,title,file_path,file_ext,file_size,down_num,point)");
                                    strSql2.Append(" values (");
                                    strSql2.Append("@article_id,@title,@file_path,@file_ext,@file_size,@down_num,@point)");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,255),
					                        new OleDbParameter("@file_path", OleDbType.VarChar,255),
					                        new OleDbParameter("@file_ext", OleDbType.VarChar,100),
					                        new OleDbParameter("@file_size", OleDbType.Integer,4),
					                        new OleDbParameter("@down_num", OleDbType.Integer,4),
					                        new OleDbParameter("@point", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.article_id;
                                    parameters2[1].Value = models.title;
                                    parameters2[2].Value = models.file_path;
                                    parameters2[3].Value = models.file_ext;
                                    parameters2[4].Value = models.file_size;
                                    parameters2[5].Value = models.down_num;
                                    parameters2[6].Value = models.point;
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
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.article_download GetDownloadModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,is_msg,is_red,digg_good,digg_bad from view_article_download ");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            Model.article_download model = new Model.article_download();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region  ������Ϣ
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["channel_id"] != null && ds.Tables[0].Rows[0]["channel_id"].ToString() != "")
                {
                    model.channel_id = int.Parse(ds.Tables[0].Rows[0]["channel_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["category_id"] != null && ds.Tables[0].Rows[0]["category_id"].ToString() != "")
                {
                    model.category_id = int.Parse(ds.Tables[0].Rows[0]["category_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["title"] != null && ds.Tables[0].Rows[0]["title"].ToString() != "")
                {
                    model.title = ds.Tables[0].Rows[0]["title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["link_url"] != null && ds.Tables[0].Rows[0]["link_url"].ToString() != "")
                {
                    model.link_url = ds.Tables[0].Rows[0]["link_url"].ToString();
                }
                if (ds.Tables[0].Rows[0]["img_url"] != null && ds.Tables[0].Rows[0]["img_url"].ToString() != "")
                {
                    model.img_url = ds.Tables[0].Rows[0]["img_url"].ToString();
                }
                if (ds.Tables[0].Rows[0]["seo_title"] != null && ds.Tables[0].Rows[0]["seo_title"].ToString() != "")
                {
                    model.seo_title = ds.Tables[0].Rows[0]["seo_title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["seo_keywords"] != null && ds.Tables[0].Rows[0]["seo_keywords"].ToString() != "")
                {
                    model.seo_keywords = ds.Tables[0].Rows[0]["seo_keywords"].ToString();
                }
                if (ds.Tables[0].Rows[0]["seo_description"] != null && ds.Tables[0].Rows[0]["seo_description"].ToString() != "")
                {
                    model.seo_description = ds.Tables[0].Rows[0]["seo_description"].ToString();
                }
                if (ds.Tables[0].Rows[0]["content"] != null && ds.Tables[0].Rows[0]["content"].ToString() != "")
                {
                    model.content = ds.Tables[0].Rows[0]["content"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sort_id"] != null && ds.Tables[0].Rows[0]["sort_id"].ToString() != "")
                {
                    model.sort_id = int.Parse(ds.Tables[0].Rows[0]["sort_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["click"] != null && ds.Tables[0].Rows[0]["click"].ToString() != "")
                {
                    model.click = int.Parse(ds.Tables[0].Rows[0]["click"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_lock"] != null && ds.Tables[0].Rows[0]["is_lock"].ToString() != "")
                {
                    model.is_lock = int.Parse(ds.Tables[0].Rows[0]["is_lock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_id"] != null && ds.Tables[0].Rows[0]["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["add_time"] != null && ds.Tables[0].Rows[0]["add_time"].ToString() != "")
                {
                    model.add_time = DateTime.Parse(ds.Tables[0].Rows[0]["add_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_msg"] != null && ds.Tables[0].Rows[0]["is_msg"].ToString() != "")
                {
                    model.is_msg = int.Parse(ds.Tables[0].Rows[0]["is_msg"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_red"] != null && ds.Tables[0].Rows[0]["is_red"].ToString() != "")
                {
                    model.is_red = int.Parse(ds.Tables[0].Rows[0]["is_red"].ToString());
                }
                if (ds.Tables[0].Rows[0]["digg_good"] != null && ds.Tables[0].Rows[0]["digg_good"].ToString() != "")
                {
                    model.digg_good = int.Parse(ds.Tables[0].Rows[0]["digg_good"].ToString());
                }
                if (ds.Tables[0].Rows[0]["digg_bad"] != null && ds.Tables[0].Rows[0]["digg_bad"].ToString() != "")
                {
                    model.digg_bad = int.Parse(ds.Tables[0].Rows[0]["digg_bad"].ToString());
                }
                #endregion  ������Ϣend

                model.download_attachs = new download_attach().GetList(id); //�����б�

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
        public DataSet GetDownloadList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,is_msg,is_red,digg_good,digg_bad ");
            strSql.Append(" FROM view_article_download ");
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
        public DataSet GetDownloadList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM view_article_download");
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