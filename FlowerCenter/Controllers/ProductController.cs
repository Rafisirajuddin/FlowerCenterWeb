using FlowerCenter.Models.BLL;
using FlowerCenter.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowerCenter.Controllers
{
    public class ProductController : Controller
    {
        productService _service;
        public ProductController()
        {
            _service = new productService();

        }
        // GET: Product
        public ActionResult ProductDetails(int ItemID)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            ViewBag.ProductDetails = _service.GetAll(ItemID);
            ViewBag.Gift = new giftService().GetAll();
            return View(_service.GetAll(ItemID));
        }
        public ActionResult Wishlist()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            return View();
        }

        public JsonResult PostProductReview(productBLL.ReviewsBLL data)
        {
            return Json(new
            {
                data = new productBLL().InsertProductReview(data)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}