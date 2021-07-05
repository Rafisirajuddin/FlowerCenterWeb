using FlowerCenter.Models.BLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static FlowerCenter.Models.BLL.checkoutBLL;
using FlowerCenter.Models;
using System.Configuration;
using com.fss.plugin;
using System.Net.Mail;

namespace FlowerCenter.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Cart()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Cart");
            return View();
        }
        public ActionResult Checkout(int id = -1)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Checkout");
            int CustomerID = id;
            if (CustomerID == 0)
            {
                Session["CustomerID"] = 0;
                return View();
            }
            else
            {
                if (Session["CustomerID"] != null && Convert.ToInt32(Session["CustomerID"]) != 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login_Register", "Account");
                }
            }

        }
        public ActionResult OrderComplete(int OrderID = 0, string OrderNo = "")
        {
            checkoutBLL check = new checkoutBLL();
            if (OrderNo == "Reject")
            {
                ViewBag.OrderNo = "Reject";
                //check.OrderUpdate(OrderID, 103);//Rejected 
            }
            else
            {
                var data = new myorderBLL().GetDetails(OrderID);
                if (data.PaymentMethodTitle != "Cash on Delivery")
                {
                    check.OrderUpdate(OrderID, 101);//In progress
                }
                string ToEmail, SubJect, cc, Bcc;
                ToEmail = data.SenderEmail;
                SubJect = "Your order on FlowerCenter - " + data.OrderNo;
                string BodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "emailpattern.txt");
                string BodyEmailadmin = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "emailpattern-admin.txt");
                string items = "";
                foreach (var item in data.OrderDetail)
                {
                    try
                    {
                        items += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                        + "<tbody>"
                        + "<tr role = 'module-content'>"
                        + "<td height = '100%' valign = 'top'>"
                        + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:0px;margin:0px;border-spacing:0;'><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                        + "<img src = '" + System.Configuration.ConfigurationManager.AppSettings["Image"].ToString() + item.ItemImage + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table></td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'> " + item.ItemTitle + "</div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'> Qty : " + item.Quantity + "</div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>RS " + item.Price + "</span></div>"
                        + "<div></div></div></td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>";

                    }
                    catch (Exception ex)
                    {
                    }
                }
                string gifts = "";
                if (data.GiftDetail.Count > 0)
                {
                    foreach (var item in data.GiftDetail)
                    {
                        try
                        {
                            gifts += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                            + "<tbody>"
                            + "<tr role = 'module-content'>"
                            + "<td height = '100%' valign = 'top'>"
                            + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                            + "<img src = '" + System.Configuration.ConfigurationManager.AppSettings["Image"].ToString() + item.GiftImage + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table></td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                            + "<div style = 'font-family: inherit; text-align: inherit'> " + item.GiftTitle + "</div>"
                            + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>RS " + item.Price + "</span></div>"
                            + "<div></div></div></td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>";

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    gifts += "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' datad-muid='f7373f10-9ba4-4ca7-9a2e-1a2ba700deb9.1'>"
                    + "<tbody>"
                    + "<tr>"
                    + "<td style='padding:20px 30px 0px 30px;' role='module-content' height='100%' valign='top' >"
                    + "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='3px' style='line-height:3px; font-size:3px;'>"
                    + "<tbody>"
                    + "<tr>"
                    + "<td style='padding:0px 0px 3px 0px;background-color: #ffcc00'></td>"
                    + "</tr>"
                    + "</tbody>"
                    + "</table>"
                    + "</td>"
                    + "</tr>"
                    + "</tbody>"
                    + "</table>";
                }
                BodyEmail = BodyEmail.Replace("#ReceiverName#", data.CustomerName.ToString());
                BodyEmail = BodyEmail.Replace("#OrderNo#", data.OrderNo.ToString());
                BodyEmail = BodyEmail.Replace("#items#", items.ToString());
                BodyEmail = BodyEmail.Replace("#gifts#", gifts.ToString());

                BodyEmailadmin = BodyEmailadmin.Replace("#ReceiverName#", data.CustomerName.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#OrderNo#", data.OrderNo.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#items#", items.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#gifts#", gifts.ToString());
                DateTime dateTime = DateTime.UtcNow.AddMinutes(180);
                BodyEmail = BodyEmail.Replace("#Customer#", data.SenderName.ToString());
                //BodyEmail = BodyEmail.Replace("#CustomerContact#", data.SenderName.ToString());
                BodyEmail = BodyEmail.Replace("#SelectedTime#", data.SelectedTime.ToString());
                BodyEmail = BodyEmail.Replace("#DeliveryDate#", data.DeliveryDate.ToString("dd/MMM/yyyy"));
                BodyEmail = BodyEmail.Replace("#OrderDate#", dateTime.ToString("dd/MMM/yyyy"));
                BodyEmail = BodyEmail.Replace("#Address#", data.Address.ToString());

                BodyEmailadmin = BodyEmailadmin.Replace("#Customer#", data.SenderName.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#CustomerContact#", data.SenderContact.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#SelectedTime#", data.SelectedTime.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryDate#", data.DeliveryDate.ToString("dd/MMM/yyyy"));
                BodyEmailadmin = BodyEmailadmin.Replace("#OrderDate#", dateTime.ToString("dd/MMM/yyyy"));
                BodyEmailadmin = BodyEmailadmin.Replace("#Address#", data.Address.ToString());
                string PaymentType = "";
                if (data.PaymentMethodID != 1)
                {
                    PaymentType = "Online Payment";
                }
                else
                {
                    PaymentType = "Cash On Delivery";
                }
                BodyEmail = BodyEmail.Replace("#PaymentType#", PaymentType.ToString());
                BodyEmail = BodyEmail.Replace("#PaymentMethod#", data.PaymentMethodTitle.ToString());
                BodyEmail = BodyEmail.Replace("#TotalItems#", data.TotalItems.ToString());
                BodyEmail = BodyEmail.Replace("#SubTotal#", data.AmountTotal.ToString());
                BodyEmail = BodyEmail.Replace("#Tax#", data.Tax.ToString());
                BodyEmail = BodyEmail.Replace("#DeliveryAmount#", data.DeliveryAmount.ToString());
                BodyEmail = BodyEmail.Replace("#GrandTotal#", data.GrandTotal.ToString());

                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", PaymentType.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentMethod#", data.PaymentMethodTitle.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#TotalItems#", data.TotalItems.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#SubTotal#", data.AmountTotal.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#Tax#", data.Tax.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryAmount#", data.DeliveryAmount.ToString());
                BodyEmailadmin = BodyEmailadmin.Replace("#GrandTotal#", data.GrandTotal.ToString());
                cc = "";
                Bcc = ConfigurationManager.AppSettings["From"].ToString();
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(ToEmail);

                    mail.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
                    mail.Subject = SubJect;
                    string Body = BodyEmail;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(); //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         (ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;

                    smtp.Send(mail);

                    //WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                    //WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    //WebMail.SmtpUseDefaultCredentials = true;
                    //WebMail.EnableSsl = false;
                    //WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();

                    //WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                    //WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                    //WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);
                    ViewBag.Status = "Order Invoice will be sent to your Email.";
                }
                catch (Exception ex)
                {
                    ViewBag.Status = "";
                }
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(ConfigurationManager.AppSettings["From"].ToString());

                    mail.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
                    mail.Subject = "NEW ORDER | " + SubJect;
                    string Body = BodyEmailadmin;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(); //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         (ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }
                catch (Exception)
                {
                }
                ViewBag.OrderNo = data.OrderNo;
            }

            return View();
        }
        //Coupon
        public JsonResult Coupon(string coupon)
        {
            couponBLL couponBLL = new couponBLL();
            ViewBag.coupon = couponBLL.Get(coupon);
            return Json(new { data = ViewBag.coupon }, JsonRequestBehavior.AllowGet);
        }
        //Order
        public JsonResult PunchOrder(checkoutBLL data)
        {
            checkoutBLL _service = new checkoutBLL();
            //orderdetails
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(JArray.Parse(data.OrderDetailString));
            JArray jsonResponse = JArray.Parse(json);
            data.OrderDetail = jsonResponse.ToObject<List<OrderDetails>>();
            //gifts
            try
            {
                if (data.OrderGiftsString != "")
                {
                    string jsonGift = Newtonsoft.Json.JsonConvert.SerializeObject(JArray.Parse(data.OrderGiftsString));
                    JArray jsonResponseGift = JArray.Parse(jsonGift);
                    data.OrderGifts = jsonResponseGift.ToObject<List<OrderGiftDetails>>();
                }
            }
            catch (Exception ex)
            { }
            int rtn = _service.InsertOrder(data);
            if (data.PaymentMethodID == 2)//Credimax 
            {
                PaymentDetails details = new PaymentDetails();
                int LastOrderID = rtn;
                try
                {
                    var client = new RestClient("https://credimax.gateway.mastercard.com/api/rest/version/54/merchant/E10561950/session");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.DefaultConnectionLimit = 9999;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", "Basic bWVyY2hhbnQuRTEwNTYxOTUwOjhhYTlhZmI5OTg0ODZhMjA0ZjI0ODY0YzIyOTY1OGNh");
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "TS01f8f5b8=014700973636cae9b19e68becca9cffe02f1b9bf08b3571d5588f1c9f20e1e143356517226b07304069c1eb77d86ef59bc3b54c7a3; TS01f8f5b8=014700973629fbc5b1c98bef8215e78947ec712f67e7bc8aad2f02e93d9992564bd3e564fc4640688959a52d9f31076f56d0f37df1");
                    request.AddParameter("text/plain", "{\n    \"apiOperation\" : \"CREATE_CHECKOUT_SESSION\",\n    \"order\": {\n            \"amount\" : \"" + data.GrandTotal + "\",\n            \"currency\" : \"RS\", \n            \"id\" : \"" + data.OrderNo + "\" \n        },\n        \"interaction\":{\n        \"operation\":\"PURCHASE\", \n        \"returnUrl\":\"https://localhost:44330/Order/OrderComplete?OrderNo=" + data.OrderNo + "&OrderID=" + LastOrderID + "\", \n        \"cancelUrl\":\"https://FlowerCenter.com/Order/OrderComplete?OrderNo=Reject&OrderID=" + LastOrderID + "\", \n            \"merchant\": {\n                 \"name\": \"Karachi Flora\",\n                 \"logo\": \"https://FlowerCenter.com/Content/assets/images/logo/logo2.png\"\n                 },\n        }\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var s = response.Content;
                    dynamic dynamicObject = JsonConvert.DeserializeObject(s);
                    string sessionID = dynamicObject["session"]["id"].ToString();
                    details.OrderNo = data.OrderNo;
                    details.GrandTotal = Convert.ToInt32(data.GrandTotal);
                    details.Description = data.OrderNo;
                    details.sessionID = sessionID;
                    return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                }
            }
            if (data.PaymentMethodID == 4)//Benefit Pay
            {
                int LastOrderID = rtn;
                return Json(new { data = "BenefitPay", OrderID = LastOrderID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = rtn }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyOrders()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            if (Session["CustomerID"] != null && Convert.ToInt32(Session["CustomerID"]) != 0)
            {
                return View(new myorderBLL().GetAll(Convert.ToInt32(Session["CustomerID"])));
            }
            else
            {
                return RedirectToAction("Login_Register", "Account");
            }
        }
        public ActionResult OrderDetails(int OrderID)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            return View(new myorderBLL().GetDetails(OrderID));
        }
        public ActionResult BenefitPay(string OrderNo, int OrderID, double GrandTotal)
        {
            iPayBenefitPipe pipe = new iPayBenefitPipe();
            Session["OrderNo"] = OrderNo;
            // Do NOT change the values of the following parameters at all.
            pipe.setAction("1");
            pipe.setCurrency("048");
            pipe.setLanguage("EN");
            pipe.setType("D");

            // modify the following to reflect your "Alias Name", "resource.cgn" file path, "keystore.bin" file path.
            pipe.setAlias("test200001056");
            string filepath = Server.MapPath("~/BenefitPay") + "\\";
            pipe.setResourcePath(filepath); //only the path that contains the file; do not write the file name
            pipe.setKeystorePath(filepath); //only the path that contains the file; do not write the file name

            // modify the following to reflect your pages URLs
            string responseurl = "https://FlowerCenter.com/Order/BenefitPayResponse?OrderID=" + OrderID;
            pipe.setResponseURL(responseurl.ToString());
            string errorurl = "https://FlowerCenter.com/Order/BenefitPayResponse";
            pipe.setErrorURL(errorurl.ToString());

            // set a unique track ID for each transaction so you can use it later to match transaction response and identify transactions in your system and “BENEFIT Payment Gateway” portal.
            pipe.setTrackId(OrderNo);

            // set transaction amount
            pipe.setAmt(GrandTotal.ToString());

            // The following user-defined fields (UDF1, UDF2, UDF3, UDF4, UDF5) are optional fields.
            // However, we recommend setting theses optional fields with invoice/product/customer identification information as they will be reflected in “BENEFIT Payment Gateway” portal where you will be able to link transactions to respective customers. This is helpful for dispute cases. 
            pipe.setUdf1("");

            pipe.setUdf2("FlowerCenter");
            pipe.setUdf3("");
            pipe.setUdf4("");
            pipe.setUdf5("");

            int val = pipe.performPaymentInitializationHTTP();
            var Address = "";
            if (val == 0)
            {
                Address = pipe.getWebAddress();
                Response.Redirect(pipe.getWebAddress());
            }
            else
            {
                Address = "error";
                Response.Write("error: " + pipe.getError());
            }
            return View();
        }
        public ActionResult BenefitPayResponse(int OrderID = 0)
        {
            iPayBenefitPipe pipe = new iPayBenefitPipe();
            pipe.setAlias("test200001056");
            string filepath = Server.MapPath("~/BenefitPay") + "\\";
            pipe.setResourcePath(filepath); //only the path that contains the file; do not write the file name
            pipe.setKeystorePath(filepath); //only the path that contains the file; do not write the file name
            string trandata = "";
            string paymentID = "";
            string result = "";
            string responseCode = "";
            string response = "";
            string transactionID = "";
            string referenceID = "";
            string trackID = "";
            string amount = "";
            string UDF1 = "";
            string UDF2 = "";
            string UDF3 = "";
            string UDF4 = "";
            string UDF5 = "";
            string authCode = "";
            string postDate = "";
            string errorCode = "";
            string errorText = "";

            if (Request.Form["trandata"] != null)
            {
                trandata = Request.Form["trandata"].ToString();
                int parse = pipe.parseEncryptedRequest(trandata);
                if (parse == 0)
                {
                    paymentID = pipe.getPaymentId();
                    result = pipe.getResult();
                    responseCode = pipe.getAuthRespCode();
                    transactionID = pipe.getTransId();
                    referenceID = pipe.getRef();
                    trackID = pipe.getTrackId();
                    amount = pipe.getAmt();
                    UDF1 = pipe.getUdf1();
                    UDF2 = pipe.getUdf2();
                    UDF3 = pipe.getUdf3();
                    UDF4 = pipe.getUdf4();
                    UDF5 = pipe.getUdf5();
                    authCode = pipe.getAuth();
                    postDate = pipe.getDate();
                    errorCode = pipe.getError();
                    errorText = pipe.getError_text();
                }
                else
                {
                    errorText = pipe.getError_text();
                }
            }
            else if (Request.Form["ErrorText"] != null)
            {
                paymentID = Request.Form["paymentid"];
                trackID = Request.Form["Trackid"];
                amount = Request.Form["amt"];
                UDF1 = Request.Form["UDF1"];
                UDF2 = Request.Form["UDF2"];
                UDF3 = Request.Form["UDF3"];
                UDF4 = Request.Form["UDF4"];
                UDF5 = Request.Form["UDF5"];
                errorText = Request.Form["ErrorText"];
            }
            else
            {
                errorText = "Unknown Exception";
            }
            // Remove any HTML/CSS/JavaScript from the page. Also, you MUST NOT write anything on the page EXCEPT the word "REDIRECT=" (in upper-case only) followed by a URL.
            // If anything else is written on the page then you will not be able to complete the process.
            if (result == "CAPTURED")
            {
                Response.Write("REDIRECT=https://FlowerCenter.com/Order/BenefitPayApproved?OrderID=" + OrderID);
            }
            else if (result == "NOT CAPTURED" || result == "CANCELED" || result == "DENIED BY RISK" || result == "HOST TIMEOUT")
            {
                if (result == "NOT CAPTURED")
                {
                    switch (responseCode)
                    {
                        case "05":
                            response = "Please contact issuer";
                            break;
                        case "14":
                            response = "Invalid card number";
                            break;
                        case "33":
                            response = "Expired card";
                            break;
                        case "36":
                            response = "Restricted card";
                            break;
                        case "38":
                            response = "Allowable PIN tries exceeded";
                            break;
                        case "51":
                            response = "Insufficient funds";
                            break;
                        case "54":
                            response = "Expired card";
                            break;
                        case "55":
                            response = "Incorrect PIN";
                            break;
                        case "61":
                            response = "Exceeds withdrawal amount limit";
                            break;
                        case "62":
                            response = "Restricted Card";
                            break;
                        case "65":
                            response = "Exceeds withdrawal frequency limit";
                            break;
                        case "75":
                            response = "Allowable number PIN tries exceeded";
                            break;
                        case "76":
                            response = "Ineligible account";
                            break;
                        case "78":
                            response = "Refer to Issuer";
                            break;
                        case "91":
                            response = "Issuer is inoperative";
                            break;
                        default:
                            // for unlisted values, please generate a proper user-friendly message
                            response = "Unable to process transaction temporarily. Try again later or try using another card.";
                            break;
                    }
                }
                else if (result == "CANCELED")
                {
                    response = "Transaction was canceled by user.";
                }
                else if (result == "DENIED BY RISK")
                {
                    response = "Maximum number of transactions has exceeded the daily limit.";
                }
                else if (result == "HOST TIMEOUT")
                {
                    response = "Unable to process transaction temporarily. Try again later.";
                }
                Response.Write("REDIRECT=https://FlowerCenter.com/Order/BenefitPayApproved");
            }
            else
            {
                //Unable to process transaction temporarily. Try again later or try using another card.
                Response.Write("REDIRECT=https://FlowerCenter.com/Order/BenefitPayApproved");
            }
            return View();
        }
        public ActionResult BenefitPayApproved(int OrderID = 0)
        {
            if (OrderID != 0)
            {
                return RedirectToAction("OrderComplete", "Order", new { OrderID = OrderID });
            }
            else
            {
                return RedirectToAction("OrderComplete", "Order", new { OrderNo = "Reject" });
            }
        }
    }
}