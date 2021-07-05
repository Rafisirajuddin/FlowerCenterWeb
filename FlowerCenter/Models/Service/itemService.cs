using FlowerCenter.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerCenter.Models.Service
{
    public class itemService : baseService
    {
        itemBLL _service;
        public itemService()
        {
            _service = new itemBLL();
        }

        public List<itemBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<itemBLL>();
            }
        }
        public List<itemBLL> GetSelecteditems()
        {
            try
            {
                return _service.GetSelecteditems();
            }
            catch (Exception ex)
            {
                return new List<itemBLL>();
            }
        }
        public List<itemBLL> GetAllFeatured()
        {
            try
            {
                return _service.GetAllFeatured();
            }
            catch (Exception ex)
            {
                return new List<itemBLL>();
            }
        }

    }
}