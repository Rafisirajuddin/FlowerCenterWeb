using FlowerCenter.Models;
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
    public class notificationBLL
    {
        public int StatusID { get; set; }
        public int NotificationID { get; set; }

        public string Title { get; set; }

        public string ButtonURL { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
    public class settingBLL
    {
        public int SettingID { get; set; }
        public double DeliveryCharges { get; set; }
        public double ServiceCharges { get; set; }
        public double OtherCharges { get; set; }
        public double TaxPercentage { get; set; }
        public double MinimumOrderValue { get; set; }
        public double COD { get; set; }
        public double Credimax { get; set; }
        public double PayPal { get; set; }
        public double BenefitPay { get; set; }
        public string TopHeaderText { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string ShopUrl { get; set; }
        public int? Facebook { get; set; }
        public int? Instagram { get; set; }
        public int? Twitter { get; set; }
        public List<notificationBLL> NotificationsList { get; set; }
        public static DataTable _dt;
        public static DataSet _ds;
        public settingBLL GetSettings()
        {
            try
            {
                var obj = new settingBLL();
                SqlParameter[] p = new SqlParameter[0];
                _ds = (new DBHelper().GetDatasetFromSP)("sp_GetSettings", p);
                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        if (_ds.Tables[0] != null)
                        {
                            obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<settingBLL>>().FirstOrDefault();
                        }
                        if (_ds.Tables[1] != null)
                        {
                            obj.NotificationsList = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[1])).ToObject<List<notificationBLL>>();
                        }
                    }
                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}