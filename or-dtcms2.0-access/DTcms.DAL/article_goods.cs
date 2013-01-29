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
	/// 商品模型
	/// </summary>
	public partial class article
	{
		#region  Method
        /// <summary>
        /// 修改商品副表一列数据
        /// </summary>
        public void UpdateGoodsField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_article_goods set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperOleDb.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// 增加一条数据,及其子表数据
		/// </summary>
		public int Add(Model.article_goods model)
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
                        strSql2.Append("insert into dt_article_goods(");
                        strSql2.Append("id,goods_no,stock_quantity,market_price,sell_price,point,is_msg,is_top,is_red,is_hot,is_slide)");
                        strSql2.Append(" values (");
                        strSql2.Append("@id,@goods_no,@stock_quantity,@market_price,@sell_price,@point,@is_msg,@is_top,@is_red,@is_hot,@is_slide)");
                        OleDbParameter[] parameters2 = {
					    new OleDbParameter("@id", OleDbType.Integer,4),
					    new OleDbParameter("@goods_no", OleDbType.VarChar,100),
					    new OleDbParameter("@stock_quantity", OleDbType.Integer,4),
					    new OleDbParameter("@market_price", OleDbType.Decimal,5),
					    new OleDbParameter("@sell_price", OleDbType.Decimal,5),
					    new OleDbParameter("@point", OleDbType.Integer,4),
					    new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_top", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_red", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_hot", OleDbType.TinyInt,1),
					    new OleDbParameter("@is_slide", OleDbType.TinyInt,1)};
                        parameters2[0].Value = newId;
                        parameters2[1].Value = model.goods_no;
                        parameters2[2].Value = model.stock_quantity;
                        parameters2[3].Value = model.market_price;
                        parameters2[4].Value = model.sell_price;
                        parameters2[5].Value = model.point;
                        parameters2[6].Value = model.is_msg;
                        parameters2[7].Value = model.is_top;
                        parameters2[8].Value = model.is_red;
                        parameters2[9].Value = model.is_hot;
                        parameters2[10].Value = model.is_slide;
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

                        //用户组价格
                        if (model.goods_group_prices != null)
                        {
                            StringBuilder strSql6;
                            foreach (Model.goods_group_price models in model.goods_group_prices)
                            {
                                strSql6 = new StringBuilder();
                                strSql6.Append("insert into dt_goods_group_price(");
                                strSql6.Append("article_id,group_id,price)");
                                strSql6.Append(" values (");
                                strSql6.Append("@article_id,@group_id,@price)");
                                OleDbParameter[] parameters6 = {
						        new OleDbParameter("@article_id", OleDbType.Integer,4),
					            new OleDbParameter("@group_id", OleDbType.Integer,4),
					            new OleDbParameter("@price", OleDbType.Decimal,5)};
                                parameters6[0].Value = newId;
                                parameters6[1].Value = models.group_id;
                                parameters6[2].Value = models.price;
                                DbHelperOleDb.ExecuteSql(conn, trans, strSql6.ToString(), parameters6);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.article_goods model)
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
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("update dt_article_goods set ");
                        strSql2.Append("id=@id,");
                        strSql2.Append("goods_no=@goods_no,");
                        strSql2.Append("stock_quantity=@stock_quantity,");
                        strSql2.Append("market_price=@market_price,");
                        strSql2.Append("sell_price=@sell_price,");
                        strSql2.Append("point=@point,");
                        strSql2.Append("is_msg=@is_msg,");
                        strSql2.Append("is_top=@is_top,");
                        strSql2.Append("is_red=@is_red,");
                        strSql2.Append("is_hot=@is_hot,");
                        strSql2.Append("is_slide=@is_slide");
                        strSql2.Append(" where id=@id ");
                        OleDbParameter[] parameters2 = {
					            new OleDbParameter("@id", OleDbType.Integer,4),
					            new OleDbParameter("@goods_no", OleDbType.VarChar,100),
					            new OleDbParameter("@stock_quantity", OleDbType.Integer,4),
					            new OleDbParameter("@market_price", OleDbType.Decimal,5),
					            new OleDbParameter("@sell_price", OleDbType.Decimal,5),
					            new OleDbParameter("@point", OleDbType.Integer,4),
					            new OleDbParameter("@is_msg", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_top", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_red", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_hot", OleDbType.TinyInt,1),
					            new OleDbParameter("@is_slide", OleDbType.TinyInt,1)};
                        parameters2[0].Value = model.id;
                        parameters2[1].Value = model.goods_no;
                        parameters2[2].Value = model.stock_quantity;
                        parameters2[3].Value = model.market_price;
                        parameters2[4].Value = model.sell_price;
                        parameters2[5].Value = model.point;
                        parameters2[6].Value = model.is_msg;
                        parameters2[7].Value = model.is_top;
                        parameters2[8].Value = model.is_red;
                        parameters2[9].Value = model.is_hot;
                        parameters2[10].Value = model.is_slide;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);

                        //修改顶和踩
                        StringBuilder strSql3 = new StringBuilder();
                        strSql3.Append("update dt_article_diggs set ");
                        strSql3.Append("digg_good=@digg_good,");
                        strSql3.Append("digg_bad=@digg_bad");
                        strSql3.Append(" where id=@id ");
                        OleDbParameter[] parameters3 = {
					            new OleDbParameter("@digg_good", OleDbType.Integer,4),
					            new OleDbParameter("@digg_bad", OleDbType.Integer,4),
                                new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters3[0].Value = model.digg_good;
                        parameters3[1].Value = model.digg_bad;
                        parameters3[2].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql3.ToString(), parameters3);

                        //删除已删除的图片
                        new article_albums().DeleteList(conn, trans, model.albums, model.id);
                        //添加/修改相册
                        if (model.albums != null)
                        {
                            StringBuilder strSql4;
                            foreach (Model.article_albums models in model.albums)
                            {
                                strSql4 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql4.Append("update dt_article_albums set ");
                                    strSql4.Append("article_id=@article_id,");
                                    strSql4.Append("big_img=@big_img,");
                                    strSql4.Append("small_img=@small_img,");
                                    strSql4.Append("remark=@remark");
                                    strSql4.Append(" where id=@id");
                                    OleDbParameter[] parameters4 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@big_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@small_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@remark", OleDbType.VarChar,500),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters4[0].Value = models.article_id;
                                    parameters4[1].Value = models.big_img;
                                    parameters4[2].Value = models.small_img;
                                    parameters4[3].Value = models.remark;
                                    parameters4[4].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql4.ToString(), parameters4);
                                }
                                else
                                {
                                    strSql4.Append("insert into dt_article_albums(");
                                    strSql4.Append("article_id,big_img,small_img,remark)");
                                    strSql4.Append(" values (");
                                    strSql4.Append("@article_id,@big_img,@small_img,@remark)");
                                    OleDbParameter[] parameters4 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@big_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@small_img", OleDbType.VarChar,255),
					                        new OleDbParameter("@remark", OleDbType.VarChar,500)};
                                    parameters4[0].Value = models.article_id;
                                    parameters4[1].Value = models.big_img;
                                    parameters4[2].Value = models.small_img;
                                    parameters4[3].Value = models.remark;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql4.ToString(), parameters4);
                                }
                            }
                        }

                        //添加/修改属性
                        if (model.attribute_values != null)
                        {
                            StringBuilder strSql5;
                            foreach (Model.attribute_value models in model.attribute_values)
                            {
                                strSql5 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql5.Append("update dt_attribute_value set ");
                                    strSql5.Append("article_id=@article_id,");
                                    strSql5.Append("attribute_id=@attribute_id,");
                                    strSql5.Append("title=@title,");
                                    strSql5.Append("content=@content");
                                    strSql5.Append(" where id=@id");
                                    OleDbParameter[] parameters5 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@attribute_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@content", OleDbType.VarChar),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters5[0].Value = models.article_id;
                                    parameters5[1].Value = models.attribute_id;
                                    parameters5[2].Value = models.title;
                                    parameters5[3].Value = models.content;
                                    parameters5[4].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql5.ToString(), parameters5);
                                }
                                else
                                {
                                    strSql5.Append("insert into dt_attribute_value(");
                                    strSql5.Append("article_id,attribute_id,title,content)");
                                    strSql5.Append(" values (");
                                    strSql5.Append("@article_id,@attribute_id,@title,@content)");
                                    OleDbParameter[] parameters5 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@attribute_id", OleDbType.Integer,4),
					                        new OleDbParameter("@title", OleDbType.VarChar,100),
					                        new OleDbParameter("@content", OleDbType.VarChar)};
                                    parameters5[0].Value = models.article_id;
                                    parameters5[1].Value = models.attribute_id;
                                    parameters5[2].Value = models.title;
                                    parameters5[3].Value = models.content;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql5.ToString(), parameters5);
                                }
                            }
                        }

                        //添加/修改用户组价格
                        if (model.goods_group_prices != null)
                        {
                            StringBuilder strSql6;
                            foreach (Model.goods_group_price models in model.goods_group_prices)
                            {
                                strSql6 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql6.Append("update dt_goods_group_price set ");
                                    strSql6.Append("article_id=@article_id,");
                                    strSql6.Append("group_id=@group_id,");
                                    strSql6.Append("price=@price");
                                    strSql6.Append(" where id=@id");
                                    OleDbParameter[] parameters6 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@group_id", OleDbType.Integer,4),
					                        new OleDbParameter("@price", OleDbType.Decimal,5),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters6[0].Value = models.article_id;
                                    parameters6[1].Value = models.group_id;
                                    parameters6[2].Value = models.price;
                                    parameters6[3].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql6.ToString(), parameters6);
                                }
                                else
                                {
                                    strSql6.Append("insert into dt_goods_group_price(");
                                    strSql6.Append("article_id,group_id,price)");
                                    strSql6.Append(" values (");
                                    strSql6.Append("@article_id,@group_id,@price)");
                                    OleDbParameter[] parameters6 = {
					                        new OleDbParameter("@article_id", OleDbType.Integer,4),
					                        new OleDbParameter("@group_id", OleDbType.Integer,4),
					                        new OleDbParameter("@price", OleDbType.Decimal,5)};
                                    parameters6[0].Value = models.article_id;
                                    parameters6[1].Value = models.group_id;
                                    parameters6[2].Value = models.price;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql6.ToString(), parameters6);
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
        public Model.article_goods GetGoodsModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,goods_no,stock_quantity,market_price,sell_price,point,is_msg,is_top,is_red,is_hot,is_slide,digg_good,digg_bad from view_article_goods ");
            strSql.Append(" where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
            parameters[0].Value = id;

            Model.article_goods model = new Model.article_goods();
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
                if (ds.Tables[0].Rows[0]["goods_no"] != null && ds.Tables[0].Rows[0]["goods_no"].ToString() != "")
                {
                    model.goods_no = ds.Tables[0].Rows[0]["goods_no"].ToString();
                }
                if (ds.Tables[0].Rows[0]["stock_quantity"] != null && ds.Tables[0].Rows[0]["stock_quantity"].ToString() != "")
                {
                    model.stock_quantity = int.Parse(ds.Tables[0].Rows[0]["stock_quantity"].ToString());
                }
                if (ds.Tables[0].Rows[0]["market_price"] != null && ds.Tables[0].Rows[0]["market_price"].ToString() != "")
                {
                    model.market_price = decimal.Parse(ds.Tables[0].Rows[0]["market_price"].ToString());
                }
                if (ds.Tables[0].Rows[0]["sell_price"] != null && ds.Tables[0].Rows[0]["sell_price"].ToString() != "")
                {
                    model.sell_price = decimal.Parse(ds.Tables[0].Rows[0]["sell_price"].ToString());
                }
                if (ds.Tables[0].Rows[0]["point"] != null && ds.Tables[0].Rows[0]["point"].ToString() != "")
                {
                    model.point = int.Parse(ds.Tables[0].Rows[0]["point"].ToString());
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

                model.goods_group_prices = GetGoodsGroupPriceList(id); //获得用户组商品价格
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
        public DataSet GetGoodsList(int Top, string strWhere, string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append(" id,channel_id,category_id,title,link_url,img_url,seo_title,seo_keywords,seo_description,content,sort_id,click,is_lock,user_id,add_time,goods_no,stock_quantity,market_price,sell_price,point,is_msg,is_top,is_red,is_hot,is_slide,digg_good,digg_bad ");
            strSql.Append(" FROM view_article_goods ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperOleDb.Query(strSql.ToString());
		}

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetGoodsList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM view_article_goods");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperOleDb.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperOleDb.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        #region 获得商品价格数据列表===================================
        /// <summary>
        /// 获得商品价格数据列表
        /// </summary>
        private List<Model.goods_group_price> GetGoodsGroupPriceList(int article_id)
        {
            List<Model.goods_group_price> modelList = new List<Model.goods_group_price>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,article_id,group_id,price from dt_goods_group_price ");
            strSql.Append(" where article_id=" + article_id);
            DataTable dt = DbHelperOleDb.Query(strSql.ToString()).Tables[0];

            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.goods_group_price model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.goods_group_price();
                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["article_id"] != null && dt.Rows[n]["article_id"].ToString() != "")
                    {
                        model.article_id = int.Parse(dt.Rows[n]["article_id"].ToString());
                    }
                    if (dt.Rows[n]["group_id"] != null && dt.Rows[n]["group_id"].ToString() != "")
                    {
                        model.group_id = int.Parse(dt.Rows[n]["group_id"].ToString());
                    }
                    if (dt.Rows[n]["price"] != null && dt.Rows[n]["price"].ToString() != "")
                    {
                        model.price = decimal.Parse(dt.Rows[n]["price"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion 获得商品价格数据列表

        #endregion  Method
    }
}

