using BonusCardManager.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace BonusCardManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Private members

        private readonly ICustomerService customerService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/Customers/NonCard
        [HttpGet("NonCard")]
        public IActionResult GetNonCardCustomers()
        {
            logger.Info(nameof(GetNonCardCustomers));

            try
            {
                var customers = customerService.GetNonCardCustomers();

                if (customers == null)
                {
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }
    }
}
