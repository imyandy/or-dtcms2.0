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
	/// 商品订单
	/// </summary>
	public partial class orders
	{
		public orders()
		{}
		#region  Method
        /// <summary>
        /// 得到最大ID
        /// </summary>
        private int GetMaxId(OleDbConnection conn, OleDbTransaction trans)
        {
            string strSql = "select top 1 id from dt_orders order by id desc";
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from dt_orders");
			strSql.Append(" where id=@id");
			OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)
			};
			parameters[0].Value = id;

			return DbHelperOleDb.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from dt_orders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperOleDb.GetSingle(strSql.ToString()));
        }

		/// <summary>
		/// 增加一条数据,及其子表数据
		/// </summary>
		public int Add(Model.orders model)
		{
            int newId;
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
			            StringBuilder strSql=new StringBuilder();
                        strSql.Append("insert into dt_orders(");
                        strSql.Append("order_no,user_id,user_name,payment_id,distribution_id,status,payment_status,distribution_status,delivery_name,delivery_no,accept_name,post_code,telphone,mobile,address,message,payable_amount,real_amount,payable_freight,real_freight,payment_fee,order_amount,point,add_time,payment_time,confirm_time,distribution_time,complete_time)");
                        strSql.Append(" values (");
                        strSql.Append("@order_no,@user_id,@user_name,@payment_id,@distribution_id,@status,@payment_status,@distribution_status,@delivery_name,@delivery_no,@accept_name,@post_code,@telphone,@mobile,@address,@message,@payable_amount,@real_amount,@payable_freight,@real_freight,@payment_fee,@order_amount,@point,@add_time,@payment_time,@confirm_time,@distribution_time,@complete_time)");
			            OleDbParameter[] parameters = {
					            new OleDbParameter("@order_no", OleDbType.VarChar,100),
					            new OleDbParameter("@user_id", OleDbType.Integer,4),
                                new OleDbParameter("@user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@payment_id", OleDbType.Integer,4),
					            new OleDbParameter("@distribution_id", OleDbType.Integer,4),
					            new OleDbParameter("@status", OleDbType.TinyInt,1),
					            new OleDbParameter("@payment_status", OleDbType.TinyInt,1),
					            new OleDbParameter("@distribution_status", OleDbType.TinyInt,1),
                                new OleDbParameter("@delivery_name", OleDbType.VarChar,100),
					            new OleDbParameter("@delivery_no", OleDbType.VarChar,100),
					            new OleDbParameter("@accept_name", OleDbType.VarChar,50),
					            new OleDbParameter("@post_code", OleDbType.VarChar,20),
					            new OleDbParameter("@telphone", OleDbType.VarChar,30),
					            new OleDbParameter("@mobile", OleDbType.VarChar,30),
					            new OleDbParameter("@address", OleDbType.VarChar,255),
					            new OleDbParameter("@message", OleDbType.VarChar,500),
					            new OleDbParameter("@payable_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@real_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@payable_freight", OleDbType.Decimal,5),
					            new OleDbParameter("@real_freight", OleDbType.Decimal,5),
					            new OleDbParameter("@payment_fee", OleDbType.Decimal,5),
					            new OleDbParameter("@order_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@point", OleDbType.Integer,4),
					            new OleDbParameter("@add_time", OleDbType.Date),
					            new OleDbParameter("@payment_time", OleDbType.Date),
                                new OleDbParameter("@confirm_time", OleDbType.Date),
					            new OleDbParameter("@distribution_time", OleDbType.Date),
					            new OleDbParameter("@complete_time", OleDbType.Date)};
                        parameters[0].Value = model.order_no;
                        parameters[1].Value = model.user_id;
                        parameters[2].Value = model.user_name;
                        parameters[3].Value = model.payment_id;
                        parameters[4].Value = model.distribution_id;
                        parameters[5].Value = model.status;
                        parameters[6].Value = model.payment_status;
                        parameters[7].Value = model.distribution_status;
                        parameters[8].Value = model.delivery_name;
                        parameters[9].Value = model.delivery_no;
                        parameters[10].Value = model.accept_name;
                        parameters[11].Value = model.post_code;
                        parameters[12].Value = model.telphone;
                        parameters[13].Value = model.mobile;
                        parameters[14].Value = model.address;
                        parameters[15].Value = model.message;
                        parameters[16].Value = model.payable_amount;
                        parameters[17].Value = model.real_amount;
                        parameters[18].Value = model.payable_freight;
                        parameters[19].Value = model.real_freight;
                        parameters[20].Value = model.payment_fee;
                        parameters[21].Value = model.order_amount;
                        parameters[22].Value = model.point;
                        parameters[23].Value = model.add_time;
                        if (model.payment_time != null)
                        {
                            parameters[24].Value = model.payment_time;
                        }
                        else
                        {
                            parameters[24].Value = DBNull.Value;
                        }
                        if (model.confirm_time != null)
                        {
                            parameters[25].Value = model.confirm_time;
                        }
                        else
                        {
                            parameters[25].Value = DBNull.Value;
                        }
                        if (model.distribution_time != null)
                        {
                            parameters[26].Value = model.distribution_time;
                        }
                        else
                        {
                            parameters[26].Value = DBNull.Value;
                        }
                        if (model.complete_time != null)
                        {
                            parameters[27].Value = model.complete_time;
                        }
                        else
                        {
                            parameters[27].Value = DBNull.Value;
                        }

                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        //取得新插入的ID
                        newId = GetMaxId(conn, trans);

                        //订单商品列表
                        if (model.order_goods != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.order_goods models in model.order_goods)
                            {
                                strSql2 = new StringBuilder();
                                strSql2.Append("insert into dt_order_goods(");
                                strSql2.Append("order_id,goods_id,goods_name,goods_price,real_price,quantity,point)");
                                strSql2.Append(" values (");
                                strSql2.Append("@order_id,@goods_id,@goods_name,@goods_price,@real_price,@quantity,@point)");
                                OleDbParameter[] parameters2 = {
						                new OleDbParameter("@order_id", OleDbType.Integer,4),
						                new OleDbParameter("@goods_id", OleDbType.Integer,4),
						                new OleDbParameter("@goods_name", OleDbType.VarChar,100),
						                new OleDbParameter("@goods_price", OleDbType.Decimal,5),
						                new OleDbParameter("@real_price", OleDbType.Decimal,5),
						                new OleDbParameter("@quantity", OleDbType.Integer,4),
						                new OleDbParameter("@point", OleDbType.Integer,4)};
                                parameters2[0].Value = newId;
                                parameters2[1].Value = models.goods_id;
                                parameters2[2].Value = models.goods_name;
                                parameters2[3].Value = models.goods_price;
                                parameters2[4].Value = models.real_price;
                                parameters2[5].Value = models.quantity;
                                parameters2[6].Value = models.point;

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
        /// 修改一列数据
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_orders set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperOleDb.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public bool UpdateField(string order_no, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_orders set " + strValue);
            strSql.Append(" where order_no='" + order_no + "'");
            int rowsAffected = DbHelperOleDb.ExecuteSql(strSql.ToString());
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.orders model)
		{
            using (OleDbConnection conn = new OleDbConnection(DbHelperOleDb.connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
			            StringBuilder strSql=new StringBuilder();
                        strSql.Append("update dt_orders set ");
                        strSql.Append("order_no=@order_no,");
                        strSql.Append("user_id=@user_id,");
                        strSql.Append("user_name=@user_name,");
                        strSql.Append("payment_id=@payment_id,");
                        strSql.Append("distribution_id=@distribution_id,");
                        strSql.Append("status=@status,");
                        strSql.Append("payment_status=@payment_status,");
                        strSql.Append("distribution_status=@distribution_status,");
                        strSql.Append("delivery_name=@delivery_name,");
                        strSql.Append("delivery_no=@delivery_no,");
                        strSql.Append("accept_name=@accept_name,");
                        strSql.Append("post_code=@post_code,");
                        strSql.Append("telphone=@telphone,");
                        strSql.Append("mobile=@mobile,");
                        strSql.Append("address=@address,");
                        strSql.Append("message=@message,");
                        strSql.Append("payable_amount=@payable_amount,");
                        strSql.Append("real_amount=@real_amount,");
                        strSql.Append("payable_freight=@payable_freight,");
                        strSql.Append("real_freight=@real_freight,");
                        strSql.Append("payment_fee=@payment_fee,");
                        strSql.Append("order_amount=@order_amount,");
                        strSql.Append("point=@point,");
                        strSql.Append("add_time=@add_time,");
                        strSql.Append("payment_time=@payment_time,");
                        strSql.Append("confirm_time=@confirm_time,");
                        strSql.Append("distribution_time=@distribution_time,");
                        strSql.Append("complete_time=@complete_time");
                        strSql.Append(" where id=@id");
                        OleDbParameter[] parameters = {
					            new OleDbParameter("@order_no", OleDbType.VarChar,100),
					            new OleDbParameter("@user_id", OleDbType.Integer,4),
                                new OleDbParameter("@user_name", OleDbType.VarChar,100),
					            new OleDbParameter("@payment_id", OleDbType.Integer,4),
					            new OleDbParameter("@distribution_id", OleDbType.Integer,4),
					            new OleDbParameter("@status", OleDbType.TinyInt,1),
					            new OleDbParameter("@payment_status", OleDbType.TinyInt,1),
					            new OleDbParameter("@distribution_status", OleDbType.TinyInt,1),
                                new OleDbParameter("@delivery_name", OleDbType.VarChar,100),
					            new OleDbParameter("@delivery_no", OleDbType.VarChar,100),
					            new OleDbParameter("@accept_name", OleDbType.VarChar,50),
					            new OleDbParameter("@post_code", OleDbType.VarChar,20),
					            new OleDbParameter("@telphone", OleDbType.VarChar,30),
					            new OleDbParameter("@mobile", OleDbType.VarChar,30),
					            new OleDbParameter("@address", OleDbType.VarChar,255),
					            new OleDbParameter("@message", OleDbType.VarChar,500),
					            new OleDbParameter("@payable_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@real_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@payable_freight", OleDbType.Decimal,5),
					            new OleDbParameter("@real_freight", OleDbType.Decimal,5),
					            new OleDbParameter("@payment_fee", OleDbType.Decimal,5),
					            new OleDbParameter("@order_amount", OleDbType.Decimal,5),
					            new OleDbParameter("@point", OleDbType.Integer,4),
					            new OleDbParameter("@add_time", OleDbType.Date),
					            new OleDbParameter("@payment_time", OleDbType.Date),
					            new OleDbParameter("@distribution_time", OleDbType.Date),
					            new OleDbParameter("@complete_time", OleDbType.Date),
					            new OleDbParameter("@id", OleDbType.Integer,4)};
                        parameters[0].Value = model.order_no;
                        parameters[1].Value = model.user_id;
                        parameters[2].Value = model.user_name;
                        parameters[3].Value = model.payment_id;
                        parameters[4].Value = model.distribution_id;
                        parameters[5].Value = model.status;
                        parameters[6].Value = model.payment_status;
                        parameters[7].Value = model.distribution_status;
                        parameters[8].Value = model.delivery_name;
                        parameters[9].Value = model.delivery_no;
                        parameters[10].Value = model.accept_name;
                        parameters[11].Value = model.post_code;
                        parameters[12].Value = model.telphone;
                        parameters[13].Value = model.mobile;
                        parameters[14].Value = model.address;
                        parameters[15].Value = model.message;
                        parameters[16].Value = model.payable_amount;
                        parameters[17].Value = model.real_amount;
                        parameters[18].Value = model.payable_freight;
                        parameters[19].Value = model.real_freight;
                        parameters[20].Value = model.payment_fee;
                        parameters[21].Value = model.order_amount;
                        parameters[22].Value = model.point;
                        parameters[23].Value = model.add_time;
                        if (model.payment_time != null)
                        {
                            parameters[24].Value = model.payment_time;
                        }
                        else
                        {
                            parameters[24].Value = DBNull.Value;
                        }
                        if (model.confirm_time != null)
                        {
                            parameters[25].Value = model.confirm_time;
                        }
                        else
                        {
                            parameters[25].Value = DBNull.Value;
                        }
                        if (model.distribution_time != null)
                        {
                            parameters[26].Value = model.distribution_time;
                        }
                        else
                        {
                            parameters[26].Value = DBNull.Value;
                        }
                        if (model.complete_time != null)
                        {
                            parameters[27].Value = model.complete_time;
                        }
                        else
                        {
                            parameters[27].Value = DBNull.Value;
                        }
                        parameters[28].Value = model.id;
                        DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //删除已删除的商品
                        DeleteGoodsList(conn, trans, model.order_goods, model.id);

                        //添加/修改商品
                        if (model.order_goods != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.order_goods models in model.order_goods)
                            {
                                strSql2 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql2.Append("update dt_order_goods set ");
                                    strSql2.Append("order_id=@order_id,");
                                    strSql2.Append("goods_id=@goods_id,");
                                    strSql2.Append("goods_name=@goods_name,");
                                    strSql2.Append("goods_price=@goods_price,");
                                    strSql2.Append("real_price=@real_price,");
                                    strSql2.Append("quantity=@quantity,");
                                    strSql2.Append("point=@point");
                                    strSql2.Append(" where id=@id");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@order_id", OleDbType.Integer,4),
					                        new OleDbParameter("@goods_id", OleDbType.Integer,4),
					                        new OleDbParameter("@goods_name", OleDbType.VarChar,100),
					                        new OleDbParameter("@goods_price", OleDbType.Decimal,5),
					                        new OleDbParameter("@real_price", OleDbType.Decimal,5),
					                        new OleDbParameter("@quantity", OleDbType.Integer,4),
					                        new OleDbParameter("@point", OleDbType.Integer,4),
					                        new OleDbParameter("@id", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.order_id;
                                    parameters2[1].Value = models.goods_id;
                                    parameters2[2].Value = models.goods_name;
                                    parameters2[3].Value = models.goods_price;
                                    parameters2[4].Value = models.real_price;
                                    parameters2[5].Value = models.quantity;
                                    parameters2[6].Value = models.point;
                                    parameters2[7].Value = models.id;
                                    DbHelperOleDb.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                                else
                                {
                                    strSql2.Append("insert into dt_order_goods(");
                                    strSql2.Append("order_id,goods_id,goods_name,goods_price,real_price,quantity,point)");
                                    strSql2.Append(" values (");
                                    strSql2.Append("@order_id,@goods_id,@goods_name,@goods_price,@real_price,@quantity,@point)");
                                    OleDbParameter[] parameters2 = {
					                        new OleDbParameter("@order_id", OleDbType.Integer,4),
					                        new OleDbParameter("@goods_id", OleDbType.Integer,4),
					                        new OleDbParameter("@goods_name", OleDbType.VarChar,100),
					                        new OleDbParameter("@goods_price", OleDbType.Decimal,5),
					                        new OleDbParameter("@real_price", OleDbType.Decimal,5),
					                        new OleDbParameter("@quantity", OleDbType.Integer,4),
					                        new OleDbParameter("@point", OleDbType.Integer,4)};
                                    parameters2[0].Value = models.order_id;
                                    parameters2[1].Value = models.goods_id;
                                    parameters2[2].Value = models.goods_name;
                                    parameters2[3].Value = models.goods_price;
                                    parameters2[4].Value = models.real_price;
                                    parameters2[5].Value = models.quantity;
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
		/// 删除一条数据，及子表所有相关数据
		/// </summary>
		public bool Delete(int id)
		{
            Hashtable sqllist = new Hashtable();
			StringBuilder strSql2=new StringBuilder();
			strSql2.Append("delete from dt_order_goods ");
			strSql2.Append(" where order_id=@order_id ");
			OleDbParameter[] parameters2 = {
					new OleDbParameter("@order_id", OleDbType.Integer,4)};
			parameters2[0].Value = id;
            sqllist.Add(strSql2.ToString(), parameters2);

			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from dt_orders ");
			strSql.Append(" where id=@id");
			OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
			parameters[0].Value = id;
            sqllist.Add(strSql.ToString(), parameters);

            return DbHelperOleDb.ExecuteSqlTran(sqllist);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.orders GetModel(int id)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 id,order_no,user_id,user_name,payment_id,distribution_id,status,payment_status,distribution_status,delivery_name,delivery_no,accept_name,post_code,telphone,mobile,address,message,payable_amount,real_amount,payable_freight,real_freight,payment_fee,order_amount,point,add_time,payment_time,confirm_time,distribution_time,complete_time from dt_orders ");
            strSql.Append(" where id=@id");
			OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.Integer,4)};
			parameters[0].Value = id;

			Model.orders model=new Model.orders();
			DataSet ds=DbHelperOleDb.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				#region  父表信息
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["order_no"] != null && ds.Tables[0].Rows[0]["order_no"].ToString() != "")
                {
                    model.order_no = ds.Tables[0].Rows[0]["order_no"].ToString();
                }
                if (ds.Tables[0].Rows[0]["user_id"] != null && ds.Tables[0].Rows[0]["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_name"] != null && ds.Tables[0].Rows[0]["user_name"].ToString() != "")
                {
                    model.user_name = ds.Tables[0].Rows[0]["user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["payment_id"] != null && ds.Tables[0].Rows[0]["payment_id"].ToString() != "")
                {
                    model.payment_id = int.Parse(ds.Tables[0].Rows[0]["payment_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_id"] != null && ds.Tables[0].Rows[0]["distribution_id"].ToString() != "")
                {
                    model.distribution_id = int.Parse(ds.Tables[0].Rows[0]["distribution_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["status"] != null && ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    model.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_status"] != null && ds.Tables[0].Rows[0]["payment_status"].ToString() != "")
                {
                    model.payment_status = int.Parse(ds.Tables[0].Rows[0]["payment_status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_status"] != null && ds.Tables[0].Rows[0]["distribution_status"].ToString() != "")
                {
                    model.distribution_status = int.Parse(ds.Tables[0].Rows[0]["distribution_status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["delivery_name"] != null && ds.Tables[0].Rows[0]["delivery_name"].ToString() != "")
                {
                    model.delivery_name = ds.Tables[0].Rows[0]["delivery_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["delivery_no"] != null && ds.Tables[0].Rows[0]["delivery_no"].ToString() != "")
                {
                    model.delivery_no = ds.Tables[0].Rows[0]["delivery_no"].ToString();
                }
                if (ds.Tables[0].Rows[0]["accept_name"] != null && ds.Tables[0].Rows[0]["accept_name"].ToString() != "")
                {
                    model.accept_name = ds.Tables[0].Rows[0]["accept_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["post_code"] != null && ds.Tables[0].Rows[0]["post_code"].ToString() != "")
                {
                    model.post_code = ds.Tables[0].Rows[0]["post_code"].ToString();
                }
                if (ds.Tables[0].Rows[0]["telphone"] != null && ds.Tables[0].Rows[0]["telphone"].ToString() != "")
                {
                    model.telphone = ds.Tables[0].Rows[0]["telphone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["mobile"] != null && ds.Tables[0].Rows[0]["mobile"].ToString() != "")
                {
                    model.mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["address"] != null && ds.Tables[0].Rows[0]["address"].ToString() != "")
                {
                    model.address = ds.Tables[0].Rows[0]["address"].ToString();
                }
                if (ds.Tables[0].Rows[0]["message"] != null && ds.Tables[0].Rows[0]["message"].ToString() != "")
                {
                    model.message = ds.Tables[0].Rows[0]["message"].ToString();
                }
                if (ds.Tables[0].Rows[0]["payable_amount"] != null && ds.Tables[0].Rows[0]["payable_amount"].ToString() != "")
                {
                    model.payable_amount = decimal.Parse(ds.Tables[0].Rows[0]["payable_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["real_amount"] != null && ds.Tables[0].Rows[0]["real_amount"].ToString() != "")
                {
                    model.real_amount = decimal.Parse(ds.Tables[0].Rows[0]["real_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payable_freight"] != null && ds.Tables[0].Rows[0]["payable_freight"].ToString() != "")
                {
                    model.payable_freight = decimal.Parse(ds.Tables[0].Rows[0]["payable_freight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["real_freight"] != null && ds.Tables[0].Rows[0]["real_freight"].ToString() != "")
                {
                    model.real_freight = decimal.Parse(ds.Tables[0].Rows[0]["real_freight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_fee"] != null && ds.Tables[0].Rows[0]["payment_fee"].ToString() != "")
                {
                    model.payment_fee = decimal.Parse(ds.Tables[0].Rows[0]["payment_fee"].ToString());
                }
                if (ds.Tables[0].Rows[0]["order_amount"] != null && ds.Tables[0].Rows[0]["order_amount"].ToString() != "")
                {
                    model.order_amount = decimal.Parse(ds.Tables[0].Rows[0]["order_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["point"] != null && ds.Tables[0].Rows[0]["point"].ToString() != "")
                {
                    model.point = int.Parse(ds.Tables[0].Rows[0]["point"].ToString());
                }
                if (ds.Tables[0].Rows[0]["add_time"] != null && ds.Tables[0].Rows[0]["add_time"].ToString() != "")
                {
                    model.add_time = DateTime.Parse(ds.Tables[0].Rows[0]["add_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_time"] != null && ds.Tables[0].Rows[0]["payment_time"].ToString() != "")
                {
                    model.payment_time = DateTime.Parse(ds.Tables[0].Rows[0]["payment_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["confirm_time"] != null && ds.Tables[0].Rows[0]["confirm_time"].ToString() != "")
                {
                    model.confirm_time = DateTime.Parse(ds.Tables[0].Rows[0]["confirm_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_time"] != null && ds.Tables[0].Rows[0]["distribution_time"].ToString() != "")
                {
                    model.distribution_time = DateTime.Parse(ds.Tables[0].Rows[0]["distribution_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["complete_time"] != null && ds.Tables[0].Rows[0]["complete_time"].ToString() != "")
                {
                    model.complete_time = DateTime.Parse(ds.Tables[0].Rows[0]["complete_time"].ToString());
                }
				#endregion  父表信息end

				#region  子表信息
				StringBuilder strSql2=new StringBuilder();
				strSql2.Append("select id,order_id,goods_id,goods_name,goods_price,real_price,quantity,point from dt_order_goods ");
				strSql2.Append(" where order_id=@id ");
			    OleDbParameter[] parameters2 = {
					    new OleDbParameter("@id", OleDbType.Integer,4)};
			    parameters2[0].Value = id;

				DataSet ds2=DbHelperOleDb.Query(strSql2.ToString(),parameters2);
				if(ds2.Tables[0].Rows.Count>0)
				{
					#region  子表字段信息
					int i = ds2.Tables[0].Rows.Count;
					List<Model.order_goods> models = new List<Model.order_goods>();
					Model.order_goods modelt;
					for (int n = 0; n < i; n++)
					{
						modelt = new Model.order_goods();
						if(ds2.Tables[0].Rows[n]["id"]!=null && ds2.Tables[0].Rows[n]["id"].ToString()!="")
						{
							modelt.id=int.Parse(ds2.Tables[0].Rows[n]["id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["order_id"]!=null && ds2.Tables[0].Rows[n]["order_id"].ToString()!="")
						{
							modelt.order_id=int.Parse(ds2.Tables[0].Rows[n]["order_id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["goods_id"]!=null && ds2.Tables[0].Rows[n]["goods_id"].ToString()!="")
						{
							modelt.goods_id=int.Parse(ds2.Tables[0].Rows[n]["goods_id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["goods_name"]!=null && ds2.Tables[0].Rows[n]["goods_name"].ToString()!="")
						{
							modelt.goods_name=ds2.Tables[0].Rows[n]["goods_name"].ToString();
						}
						if(ds2.Tables[0].Rows[n]["goods_price"]!=null && ds2.Tables[0].Rows[n]["goods_price"].ToString()!="")
						{
							modelt.goods_price=decimal.Parse(ds2.Tables[0].Rows[n]["goods_price"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["real_price"]!=null && ds2.Tables[0].Rows[n]["real_price"].ToString()!="")
						{
							modelt.real_price=decimal.Parse(ds2.Tables[0].Rows[n]["real_price"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["quantity"]!=null && ds2.Tables[0].Rows[n]["quantity"].ToString()!="")
						{
							modelt.quantity=int.Parse(ds2.Tables[0].Rows[n]["quantity"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["point"]!=null && ds2.Tables[0].Rows[n]["point"].ToString()!="")
						{
							modelt.point=int.Parse(ds2.Tables[0].Rows[n]["point"].ToString());
						}
						models.Add(modelt);
					}
					model.order_goods = models;
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
        /// 根据订单号返回一个实体
        /// </summary>
        public Model.orders GetModel(string order_no)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from dt_orders");
            strSql.Append(" where order_no=@order_no");
            OleDbParameter[] parameters = {
					new OleDbParameter("@order_no", OleDbType.VarChar,100)};
            parameters[0].Value = order_no;

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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append(" id,order_no,user_id,user_name,payment_id,distribution_id,status,payment_status,distribution_status,delivery_name,delivery_no,accept_name,post_code,telphone,mobile,address,message,payable_amount,real_amount,payable_freight,real_freight,payment_fee,order_amount,point,add_time,payment_time,confirm_time,distribution_time,complete_time ");
            strSql.Append(" FROM dt_orders ");
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
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM dt_orders");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperOleDb.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperOleDb.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

		#endregion  Method

        #region 扩展方法============================================
        /// <summary>
        /// 查找不存在的商品并删除
        /// </summary>
        private void DeleteGoodsList(OleDbConnection conn, OleDbTransaction trans, List<Model.order_goods> models, int order_id)
        {
            StringBuilder idList = new StringBuilder();
            if (models != null)
            {
                foreach (Model.order_goods modelt in models)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
            }
            string id_list = Utils.DelLastChar(idList.ToString(), ",");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_order_goods where order_id=" +order_id);
            if (!string.IsNullOrEmpty(id_list))
            {
                strSql.Append(" and id not in(" + id_list + ")");
            }
            DbHelperOleDb.ExecuteSql(conn, trans, strSql.ToString());
        }
        #endregion 扩展方法end

    }
}

