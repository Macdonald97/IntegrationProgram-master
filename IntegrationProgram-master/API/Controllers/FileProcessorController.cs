using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace API.Controllers
{
    public class FileProcessorController : ApiController
    {
        // GET: api/FileProcessor/5
        public List<Output> Get(string filePath, bool fromUI = true)
        {
            List<Input> lstOrders = File.ReadAllLines(filePath).Skip(1).Select(v => Input.FromCsv(v)).ToList();
            if(!fromUI)
            {
                string message = GetErrorMessage(lstOrders).ToString();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    StringBuilder errorMessage = new StringBuilder("The file is rejected for the following reason(s)" + Environment.NewLine);
                    errorMessage.Append(message);
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(errorMessage.ToString(), System.Text.Encoding.UTF8, "text/plain"),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    throw new HttpResponseException(response);
                }
            }            

            List<Output> lstOutput = new List<Output>();

            var ordersGrouped = lstOrders.GroupBy(order => order.Customer_ID);
            foreach (var group in ordersGrouped)
            {
                int customerId = group.Key;
                var ordersGroupedByDate = group.GroupBy(order => order.Order_Date);
                foreach (var grouped in ordersGroupedByDate)
                {
                    Output output = new Output();
                    output.Customer = customerId;
                    output.Date = grouped.Key;
                    output.Total = 0;
                    foreach (var order in grouped)
                    {
                        output.Total += order.Order_Amount;
                    }
                    lstOutput.Add(output);
                }
            }
            return lstOutput;
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
