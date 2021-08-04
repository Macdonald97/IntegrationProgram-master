using FileProcessorUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Http;
using System.Text;

namespace FileProcessorUI.Controllers
{
    public class FileProcessorController : Controller
    {
        private const string URL = "https://localhost:44300/api/FileProcessor";
        StringBuilder errorMessage = new StringBuilder("The file is rejected for the following reason(s)" + Environment.NewLine);
        // GET: FileProcessor
        public ActionResult Index(string filePath)
        {
            List<Output> lstOutput = new List<Output>();
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    List<Input> lstOrders = System.IO.File.ReadAllLines(filePath).Skip(1).Select(v => Input.FromCsv(v)).ToList();
                    string message = GetErrorMessage(lstOrders).ToString();
                    if(!string.IsNullOrWhiteSpace(message))
                    {
                        errorMessage.Append(message);
                        ViewBag.Error = errorMessage;
                    }                        
                    else
                        lstOutput = CallAPIFileProcessor(filePath);
                }                    
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(lstOutput);
        }

        protected List<Output> CallAPIFileProcessor(string filePath)
        {
            try
            {
                string requestUri = "?filePath=" + filePath;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(requestUri).Result;                
                string responseBody = response.Content.ReadAsStringAsync().Result;
                string jsonResult = JsonConvert.DeserializeObject(responseBody).ToString();
                return JsonConvert.DeserializeObject<List<Output>>(jsonResult);                
            }
            catch (HttpResponseException ex)
            {
                throw;
            }
        }

        public StringBuilder GetErrorMessage(List<Input> lstOrders)
        {
            int zeroOrders = lstOrders.Where(o => o.Order_Amount <= 0).Count();
            int ordersGreaterThan4000 = lstOrders.Where(o => o.Order_Amount > 4000).Count();
            int futureOrderDate = lstOrders.Where(o => o.Order_Date > DateTime.Today).Count();
            int sundays = lstOrders.Where(o => o.Order_Date.DayOfWeek == DayOfWeek.Sunday).Count();
            int customerIdLessThanZero = lstOrders.Where(o => o.Customer_ID < 0).Count();

            StringBuilder errorMessage = new StringBuilder();

            if (zeroOrders > 0)
                errorMessage.Append(" - There are " + zeroOrders + " orders with amounts of R0 or less. " + Environment.NewLine);
            if (ordersGreaterThan4000 > 0)
                errorMessage.Append(" - There are " + ordersGreaterThan4000 + " orders with amounts greater than R4000. " + Environment.NewLine);
            if (futureOrderDate > 0)
                errorMessage.Append(" - There are " + futureOrderDate + " orders whos order date is in the future. " + Environment.NewLine);
            if (sundays > 0)
                errorMessage.Append(" - There are " + sundays + " orders whos order date is on a Sunday. " + Environment.NewLine);
            if (customerIdLessThanZero > 0)
                errorMessage.Append(" - There are " + customerIdLessThanZero + " orders with customer_id that is less than 0. " + Environment.NewLine);

            return errorMessage;
        }
    }
}
