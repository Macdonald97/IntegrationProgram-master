using API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Web.Http;

namespace API.Tests.Controllers
{
    [TestClass]
    public class FileProcessorControllerTest
    {
        [TestMethod]
        public void ProcessFile_Returns_The_Processed_Records()
        {
            //Arrange
            var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\File.csv";
            var controller = new FileProcessorController();

            // Act
            var result = controller.Get(filePath, false);

            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void ProcessFile_Returns_Exception_Amount_Less_Or_Equal_Zero()
        {
            try
            {
                //Arrange
                var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\AmountLessThanZero.csv";
                var controller = new FileProcessorController();

                // Act
                var result = controller.Get(filePath, false);
            }
            catch (HttpResponseException ex)
            {
                //Assert                
                Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode, "Wrong response type");
            }

        }

        [TestMethod]
        public void ProcessFile_Returns_Exception_Amount_Greater_Than_R4000()
        {
            try
            {
                //Arrange
                var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\AmountGreaterThanR4000.csv";
                var controller = new FileProcessorController();

                // Act
                var result = controller.Get(filePath, false);
            }
            catch (HttpResponseException ex)
            {
                //Assert                
                Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode, "Wrong response type");
            }

        }

        [TestMethod]
        public void ProcessFile_Returns_Exception_Order_Date_In_Future()
        {
            try
            {
                //Arrange
                var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\OrderDateInFuture.csv";
                var controller = new FileProcessorController();

                // Act
                var result = controller.Get(filePath, false);
            }
            catch (HttpResponseException ex)
            {
                //Assert                
                Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode, "Wrong response type");
            }

        }

        [TestMethod]
        public void ProcessFile_Returns_Exception_Order_Date_On_Sunday()
        {
            try
            {
                //Arrange
                var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\OrderDateOnSunday.csv";
                var controller = new FileProcessorController();

                // Act
                var result = controller.Get(filePath, false);
            }
            catch (HttpResponseException ex)
            {
                //Assert                
                Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode, "Wrong response type");
            }

        }

        [TestMethod]
        public void ProcessFile_Returns_Exception_CustomerId_Less_Than_Zero()
        {
            try
            {
                //Arrange
                var filePath = @"C:\Users\kgabo\source\repos\IntegrationProgram\API.Tests\TestFiles\CustomerIdLessThanZero.csv";
                var controller = new FileProcessorController();

                // Act
                var result = controller.Get(filePath, false);
            }
            catch (HttpResponseException ex)
            {
                //Assert                
                Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode, "Wrong response type");
            }

        }
    }
}
