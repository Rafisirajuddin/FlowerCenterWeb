using FlowerCenter.Models.BLL;
using FlowerCenter.Models.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace FlowerCenter.Controllers
{
    public class ShopController : Controller
    {
        shopService _service;
        filterService filterService;
        public ShopController()
        {
            _service = new shopService();
            filterService = new filterService();

        }
        // GET: Shop
        public ActionResult Shop(string Category="",string CategoryIDs = "", string SubCategoryIDs = "", string ColorIDs = "", string MinPrice = "", string MaxPrice = "", string Searchtext = "",int SortID = 0)
        {
            ViewBag.BestProduct = new shopService().BestProducts();
            ViewBag.Category = new categoryBLL().GetAll();
            ViewBag.SubCategory = new subcategoryBLL().GetAll();
            ViewBag.Color = new colorBLL().GetAll();
            ViewBag.Banner = new bannerBLL().GetBanner("Shop");
            TempData["Category"] = Category;
            TempData["CategoryIDs"] = CategoryIDs;
            TempData["SubCategoryIDs"] = SubCategoryIDs;
            TempData["ColorIDs"] = ColorIDs;
            TempData["MinPrice"] = MinPrice;
            TempData["MaxPrice"] = MaxPrice;
            TempData["Searchtext"] = Searchtext;
            TempData["SortID"] = SortID.ToString();
            return View();
        }
        
        public ActionResult Products(List<filterBLL> Products)
        {
            ViewBag.Message = "";
            if (Products != null)
            {
                ViewBag.shopList = Products;
                if (ViewBag.shopList.Count < 1)
                {
                    ViewBag.Message = "No Product Found";
                }
                return PartialView("AllProducts");
            }
            else
            {
                if (TempData.Count > 1)
                {
                    if (TempData["CategoryIDs"].ToString() != "" ||
                    TempData["SubCategoryIDs"].ToString() != "" ||
                    TempData["ColorIDs"].ToString() != "" ||
                    TempData["MinPrice"].ToString() != "" ||
                    TempData["MaxPrice"].ToString() != "" ||
                    TempData["Searchtext"].ToString() != "" ||
                    TempData["SortID"].ToString() != "5")
                    {
                        filterBLL data = new filterBLL();
                        data.Category = TempData["CategoryIDs"].ToString();
                        data.SubCategory = TempData["SubCategoryIDs"].ToString();
                        data.Color = TempData["ColorIDs"].ToString();
                        data.MinPrice = TempData["MinPrice"].ToString();
                        data.MaxPrice = TempData["MaxPrice"].ToString();
                        data.Searchtxt = TempData["Searchtext"].ToString();
                        data.SortID = Convert.ToInt32(TempData["SortID"].ToString());
                        if (data.MinPrice == "" || data.MaxPrice == "")
                        {
                            data.MinPrice = "RS0";
                            data.MaxPrice = "RS50000";
                        }
                        ViewBag.shopList = filterService.GetAll(data);
                        if (ViewBag.shopList.Count < 1)
                        {
                            ViewBag.Message = "No Product Found";
                        }
                    }
                }
                else
                {
                    /*string category = "";
                    if (TempData["Category"] != null)
                    {
                        category = TempData["Category"].ToString();
                    }
                    ViewBag.shopList = _service.GetAll(category);*/
                    ViewBag.shopList = "";
                    ViewBag.Message = "No Product Found";

                }
                
                return PartialView("AllProducts");
            }
        }

        public JsonResult Filter(filterBLL data)
        {
            ViewBag.shopList = filterService.GetAll(data);
            return Json(new { data = ViewBag.shopList }, JsonRequestBehavior.AllowGet);
        }
    }
}