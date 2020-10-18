using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace BonusCardManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusCardController : ControllerBase
    {
        #region Private members

        private readonly IBonusCardService bonusCardService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public BonusCardController(IBonusCardService bonusCardService)
        {
            this.bonusCardService = bonusCardService;
        }

        // GET: api/BonusCard/ByCardNumber/{cardNumber}
        [HttpGet("ByCardNumber/{cardNumber}")]
        public IActionResult GetByCardNumber(int cardNumber)
        {
            logger.Info(nameof(GetByCardNumber));

            try
            {
                var bonusCard = bonusCardService.GetBonusCard(cardNumber);

                if (bonusCard == null)
                {
                    return NotFound(cardNumber);
                }

                return Ok(bonusCard);
            }
            catch (ArgumentException e)
            {
                logger.Warn(e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }

        // GET: api/BonusCard/ByPhoneNumber/{customePhone}
        [HttpGet("ByPhoneNumber/{customePhone}")]
        public IActionResult GetByPhoneNumber(string customePhone)
        {
            logger.Info(nameof(GetByPhoneNumber));

            try
            {
                var bonusCard = bonusCardService.GetBonusCard(customePhone);

                if(bonusCard == null)
                {
                    return NotFound(customePhone);
                }

                return Ok(bonusCard);
            }
            catch (ArgumentException e)
            {
                logger.Warn(e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }

        // POST: api/BonusCard
        [HttpPost]
        public IActionResult Create([FromBody] BonusCardDto bonusCard)
        {
            logger.Info(nameof(Create));

            try
            {
                bonusCardService.CreateBonusCard(bonusCard);

                return CreatedAtAction(nameof(GetByPhoneNumber), new { customePhone = bonusCard.CustomerPhoneNumber}, bonusCard);
            }
            catch (ArgumentException e)
            {
                logger.Warn(e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }

        // PUT: api/BonusCard/{id}/Accrual/{amount}
        [HttpPut("{id}/Accrual/{amount}")]
        public IActionResult AccrualBalance(int id, decimal amount)
        {
            logger.Info(nameof(AccrualBalance));

            try
            {
                bonusCardService.AccrualBalance(id, amount);

                return Ok();
            }
            catch (ArgumentException e)
            {
                logger.Warn(e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }

        // PUT: api/BonusCard/{id}/WriteOff/{amount}
        [HttpPut("{id}/WriteOff/{amount}")]
        public IActionResult WriteOffBalance(int id, decimal amount)
        {
            logger.Info(nameof(WriteOffBalance));

            try
            {
                bonusCardService.WriteOffBalance(id, amount);

                return Ok();
            }
            catch (ArgumentException e)
            {
                logger.Warn(e.Message);

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return StatusCode(500);
            }
        }
    }
}
