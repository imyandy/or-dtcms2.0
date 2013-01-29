using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.OleDb;
using DTcms.Common;
using DTcms.DBUtility;

namespace DTcms.DAL
{
    /// <summary>
    /// 文章模型
    /// </summary>
    public partial class article
    {
        #region  Method
        /// <summary>
        /// 修改文章副表一列数据
        /// </summary>
        public void UpdateNewsField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_article_news set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperOleDb.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 增加一条数据,及其子表数据
        /// </summary>
        public int Add(Model.article_news model)
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
                        //取得新插入的ID
                        newId = GetMaxId(conn, trans);

                        //副表信息
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("insert into dt_article_news(");
                        strSql2.Append("id,author,[from],zhaiyao,is_msg,is_top,is_red,is_hot,is_slide)");
                        strSql2.Append(" values (");
                        strSql2.Append("@id,@author,@from,@zhaiyao,@is_msg,@is_top,@is_red,@is_hot,@is_slide)");
                        OleDbParameter[] parameters2 = {
					    new OleDbParameter("@id", OleDbType.Integer,4),
					    new OleDbParameter("@author", OleDbType.VarChar,100),
					    new OleDbParameter("@from", OleDbType.VarChar,50),
					    new OleDbParameter("@zhaiyao", OleDbType.VarChar,255),
					    new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_top", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_red", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_hot", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_slide", OleDbType.TinyInt,1)};
                        parameters2[0].Value = newId;
                        parameters2[1].Value = model.author;
                        parameters2[2].Value = model.from;
                        parameters2[3].Value = model.zhaiyao;
                        parameters2[4].Value = model.is_msg;
                        parameters2[5].Value = model.is_top;
                        parameters2[6].Value = model.is_red;
                        parameters2[7].Value = model.is_hot;
                        parameters2[8].Value = model.is_slide;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);

                        //顶和踩
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

                        //图片相册
                        if (model.albums != null)
                        {
                            StringBuilder strSql4;
                            foreach (Model.article_albums models in model.albums)
                            {
                                strSql4 = new StringBuilder();
                                strSql4.Append("insert into dt_article_albums(");
                                strSql4.Append("article_id,big_img,small_img,remark)");
                                strSql4.Append(" values (");
                                strSql4.Append("@article_id,@big_img,@small_img,@remark)");
                                OleDbParameter[] parameters4 = {
					            new OleDbParameter("@article_id", OleDbType.Integer,4),
					            new OleDbParameter("@big_img", OleDbType.VarChar,255),
					            new OleDbParameter("@small_img", OleDbType.VarChar,255),
					            new OleDbParameter("@remark", OleDbType.VarChar,500)};
                                parameters4[0].Value = newId;
                                parameters4[1].Value = models.big_img;
                                parameters4[2].Value = models.small_img;
                                parameters4[3].Value = models.remark;

                                DbHelperOleDb.ExecuteSql(conn, trans, strSql4.ToString(), parameters4);
                            }
                        }
                        //扩展属性
                        if (model.attribute_values != null)
                        {
                            StringBuilder strSql5;
                            foreach (Model.attribute_value models in model.attribute_values)
                            {
                                strSql5 = new StringBuilder();
                                strSql5.Append("insert into dt_attribute_value(");
                                strSql5.Append("article_id,attribute_id,title,content)");
                                strSql5.Append(" values (");
                                strSql5.Append("@article_id,@attribute_id,@title,@content)");
                                OleDbParameter[] parameters5 = {
					            new OleDbParameter("@article_id", OleDbType.Integer,4),
					            new OleDbParameter("@attribute_id", OleDbType.Integer,4),
					            new OleDbParameter("@title", OleDbType.VarChar,100),
					            new OleDbParameter("@content", OleDbType.VarChar)};
                                parameters5[0].Value = newId;
                                parameters5[1].Value = models.attribute_id;
                                parameters5[2].Value = models.title;
                                parameters5[3].Value = models.content;
                                DbHelperOleDb.ExecuteSql(conn, trans, strSql5.ToString(), parameters5);
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
        /// 更新一条数据,及其子表数据 
        /// </summary>
        public bool Update(Model.article_news model)
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

                        //修改副表
                        StringBuilder strSql21 = new StringBuilder();
                        strSql21.Append("update dt_article_news set ");
                        strSql21.Append("author=@author,");
                        strSql21.Append("[from]=@from,");
                        strSql21.Append("zhaiyao=@zhaiyao,");
                        strSql21.Append("is_msg=@is_msg,");
                        strSql21.Append("is_top=@is_top,");
                        strSql21.Append("is_red=@is_red,");
                        strSql21.Append("is_hot=@is_hot,");
                        strSql21.Append("is_slide=@is_slide");
                        strSql21.Append(" where id=@id ");
                        OleDbParameter[] parameters21 = {
					            new OleDbParameter("@author", OleDbType.VarChar,100),
					            new OleDbParameter("@from", OleDbType.VarChar,50),
					            new OleDbParameter("@zhaiyao", OleDbType.VarChar,255),
					            new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_top", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_red", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_hot", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_slide", OleDbType.TinyInt,1),
                                new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters21[0].Value = model.author;
                        parameters21[1].Value = model.from;
                        parameters21[2].Value = model.zhaiyao;
                        parameters21[3].Value = model.is_msg;
                        parameters21[4].Value = model.is_top;
                        parameters21[5].Value = model.is_red;
                        parameters21[6].Value = model.is_hot;
                        parameters21[7].Value = model.is_slide;
                        parameters21[8].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql21.ToString(), parameters21);

                        //修改顶和踩
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

                        //删除已删除的图片
                        new article_albums().DeleteList(conn, trans, model.albums, model.id);
                        //添加/修改相册
                        if (model.albums != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.article_albums models in model.albums)
                            {
                                strSql2 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql2.Append("update dt_article_albums set ");
                                    strSql2.Append("article_id=@article_id,");
                                    strSql2.Append("big_img=@big_img,");
                                    strSql2.Append("small_img=@small_img,");
                                    strSql2.Append("remark=@remark");
                                    strSql2.Append(" where id=@id");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@big_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@small_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@remark", OleDbType.VarChar,500),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.article_id;
                                    parameters2[1].Value = models.big_img;
                                    parameters2[2].Value = models.small_img;
                                    parameters2[3].Value = models.remark;
                                    parameters2[4].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                                else
                                {
                                    strSql2.Append("insert into dt_article_albums(");
                                    strSql2.Append("article_id,big_img,small_img,remark)");
                                    strSql2.Append(" values (");
                                    strSql2.Append("@article_id,@big_img,@small_img,@remark)");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@big_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@small_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@remark", OleDbType.VarChar,500)};
                                    parameters2[0].Value = models.article_id;
                                    parameters2[1].Value = models.big_img;
                                    parameters2[2].Value = models.small_img;
                                    parameters2[3].Value = models.remark;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                            }
                        }

                        //添加/修改属性
                        if (model.attribute_values != null)
                        {
                            StringBuilder strSql3;
                            foreach (Model.attribute_value models in model.attribute_values)
                            {
                                strSql3 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql3.Append("update dt_attribute_value set ");
                                    strSql3.Append("article_id=@article_id,");
                                    strSql3.Append("attribute_id=@attribute_id,");
                                    strSql3.Append("title=@title,");
                                    strSql3.Append("content=@content");
                                    strSql3.Append(" where id=@id");
                                    OleDbParameter[] parameters3 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@attribute_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@content", OleDbType.VarChar),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters3[0].Value = models.article_id;
                                    parameters3[1].Value = models.attribute_id;
                                    parameters3[2].Value = models.title;
                                    parameters3[3].Value = models.content;
                                    parameters3[4].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql3.ToString(), parameters3);
                                }
                                else
                                {
                                    strSql3.Append("insert into dt_attribute_value(");
                                    strSql3.Append("article_id,attribute_id,title,content)");
                                    strSql3.Append(" values (");
                                    strSql3.Append("@article_id,@attribute_id,@title,@content)");
                                    OleDbParameter[] parameters3 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@attribute_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@content", OleDbType.VarChar)};
                                    parameters3[0].Value = models.article_id;
                                    parameters3[1].Value = models.attribute_id;
                                    parameters3[2].Value = models.title;
                                    parameters3[3].Value = models.content;
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
        /// 得到一个对象实体
        /// </summary>
        public Model.article_news GetNewsModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,author,[from],zhaiyao,is_msg,is_top,is_red,is_hot,is_slide,digg_good,digg_bad from view_article_news ");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            Model.article_news model = new Model.article_news();
            DataSet ds = DbHelperOleDb.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region  父表信息
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
                if (ds.Tables[0].Rows[0]["author"] != null && ds.Tables[0].Rows[0]["author"].ToString() != "")
                {
                    model.author = ds.Tables[0].Rows[0]["author"].ToString();
                }
                if (ds.Tables[0].Rows[0]["from"] != null && ds.Tables[0].Rows[0]["from"].ToString() != "")
                {
                    model.from = ds.Tables[0].Rows[0]["from"].ToString();
                }
                if (ds.Tables[0].Rows[0]["zhaiyao"] != null && ds.Tables[0].Rows[0]["zhaiyao"].ToString() != "")
                {
                    model.zhaiyao = ds.Tables[0].Rows[0]["zhaiyao"].ToString();
                }
                if (ds.Tables[0].Rows[0]["is_msg"] != null && ds.Tables[0].Rows[0]["is_msg"].ToString() != "")
                {
                    model.is_msg = int.Parse(ds.Tables[0].Rows[0]["is_msg"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_top"] != null && ds.Tables[0].Rows[0]["is_top"].ToString() != "")
                {
                    model.is_top = int.Parse(ds.Tables[0].Rows[0]["is_top"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_red"] != null && ds.Tables[0].Rows[0]["is_red"].ToString() != "")
                {
                    model.is_red = int.Parse(ds.Tables[0].Rows[0]["is_red"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_hot"] != null && ds.Tables[0].Rows[0]["is_hot"].ToString() != "")
                {
                    model.is_hot = int.Parse(ds.Tables[0].Rows[0]["is_hot"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_slide"] != null && ds.Tables[0].Rows[0]["is_slide"].ToString() != "")
                {
                    model.is_slide = int.Parse(ds.Tables[0].Rows[0]["is_slide"].ToString());
                }
                if (ds.Tables[0].Rows[0]["digg_good"] != null && ds.Tables[0].Rows[0]["digg_good"].ToString() != "")
                {
                    model.digg_good = int.Parse(ds.Tables[0].Rows[0]["digg_good"].ToString());
                }
                if (ds.Tables[0].Rows[0]["digg_bad"] != null && ds.Tables[0].Rows[0]["digg_bad"].ToString() != "")
                {
                    model.digg_bad = int.Parse(ds.Tables[0].Rows[0]["digg_bad"].ToString());
                }
                
                #endregion  父表信息end

                model.albums = new article_albums().GetList(id); //相册信息
                model.attribute_values = new attribute_value().GetList(id); //扩展属性

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetNewsList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,author,[from],zhaiyao,is_msg,is_top,is_red,is_hot,is_slide,digg_good,digg_bad ");
            strSql.Append(" FROM view_article_news ");
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
        public DataSet GetNewsList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM view_article_news");
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