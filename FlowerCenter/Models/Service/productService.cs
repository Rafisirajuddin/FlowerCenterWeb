﻿using FlowerCenter.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerCenter.Models.Service
{
    public class productService : baseService
    {
        productBLL _service;
        public productService()
        {
            _service = new productBLL();
        }

        public productBLL GetAll(int ItemID)
        {
            try
            {
                return _service.GetAll(ItemID);
            }
            catch (Exception ex)
            {
                return new productBLL();
            }
        }

    }
}