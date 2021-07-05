﻿using FlowerCenter.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace FlowerCenter.Models.BLL
{
    public class productBLL
    {
        public int ItemID { get; set; }
        public int? Stars { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ArabicTitle { get; set; }
        public double? Cost { get; set; }
        public double? Price { get; set; }
        public double? DiscountedPrice { get; set; }
        public double? DoublePrice { get; set; }
        public bool? InStock { get; set; }
        public int? StatusID { get; set; }
        public int? StockQty { get; set; }
        public bool? IsDoubleQty { get; set; }
        public List<ItemImages> ImgList = new List<ItemImages>();
        public List<ReviewsBLL> Reviews= new List<ReviewsBLL>();

        public class ItemImages
        {
            public string Image { get; set; }
            public int Row_Counter { get; set; }
        }
        public class ReviewsBLL
        {
            public int ReviewID { get; set; }

            public int? Stars { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string Email { get; set; }

            public string Contact { get; set; }

            public int? StatusID { get; set; }

            public int? ItemID { get; set; }
        }


        public static DataTable _dt;
        public static DataSet _ds;
        public productBLL GetAll(int ItemID)
        {
            try
            {
                var obj = new productBLL();
                List<ItemImages> lstIM = new List<ItemImages>();
                List<ReviewsBLL> lstR = new List<ReviewsBLL>();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@ItemID", ItemID);
                _ds = (new DBHelper().GetDatasetFromSP)("sp_ProductDetails", p);
                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        if (_ds.Tables[0] != null)
                        {
                            obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<productBLL>>().FirstOrDefault();
                        }
                        if (_ds.Tables[1] != null)
                        {
                            lstIM = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[1])).ToObject<List<ItemImages>>();
                        }
                        if (_ds.Tables[2] != null)
                        {
                            lstR = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[2])).ToObject<List<ReviewsBLL>>();
                        }
                        obj.ImgList = lstIM;
                        obj.Reviews = lstR;
                    }
                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int InsertProductReview(ReviewsBLL data)
        {

            try
            {
                SqlParameter[] p = new SqlParameter[7];
                p[0] = new SqlParameter("@Name", data.Name);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@Contact", data.Contact);
                p[3] = new SqlParameter("@Email", data.Email);
                p[4] = new SqlParameter("@Stars", data.Stars);
                p[5] = new SqlParameter("@StatusID", data.StatusID);
                p[6] = new SqlParameter("@ItemID", data.ItemID);
                return int.Parse(new DBHelper().GetTableFromSP("sp_InsertReview", p).Rows[0]["ID"].ToString());
                
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}