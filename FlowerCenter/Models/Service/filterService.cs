﻿using FlowerCenter.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FlowerCenter.Models.Service
{
    public class filterService : baseService
    {
        filterBLL _service;
        public filterService()
        {
            _service = new filterBLL();
        }

        public List<filterBLL> GetAll(filterBLL filter)
        {
            try
            {
                return _service.GetAll(filter);
            }
            catch (Exception ex)
            {
                return new List<filterBLL>();
            }
        }

    }
}