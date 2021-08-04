using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Input
    {
        public int Message_ID { get; set; }
        public int Customer_ID { get; set; }
        public int Order_ID { get; set; }
        public decimal Order_Amount { get; set; }
        public DateTime Order_Date { get; set; }

        public static Input FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Input inputValues = new Input();
            inputValues.Message_ID = Convert.ToInt32(values[0]);
            inputValues.Customer_ID = Convert.ToInt32(values[1]);
            inputValues.Order_ID = Convert.ToInt32(values[2]);
            inputValues.Order_Amount = Decimal.Parse(values[3], CultureInfo.InvariantCulture);
            inputValues.Order_Date = Convert.ToDateTime(values[4]);
            return inputValues;
        }
    }
}