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
    public class BonusCardsController : ControllerBase
    {
        #region Private members

        private readonly IBonusCardService bonusCardService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public BonusCardsController(IBonusCardService bonusCardService)
        {
            this.bonusCardService = bonusCardService;
        }

        // GET: api/BonusCards/ByCardNumber/{cardNumber}
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

        // GET: api/BonusCards/ByPhoneNumber/{customePhone}
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

        // POST: api/BonusCards
        [HttpPost]
        public IActionResult Create([FromBody] BonusCardDto bonusCard)
        {
            logger.Info(nameof(Create));

            try
            {
                var newBonusCard = bonusCardService.CreateBonusCard(bonusCard);

                return CreatedAtAction(nameof(GetByCardNumber), new { cardNumber = newBonusCard.Number}, newBonusCard);
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

        // PUT: api/BonusCards/{id}/Accrual
        [HttpPut("{id}/Accrual")]
        public IActionResult AccrualBalance(int id, [FromBody] decimal amount)
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

        // PUT: api/BonusCards/{id}/WriteOff
        [HttpPut("{id}/WriteOff")]
        public IActionResult WriteOffBalance(int id, [FromBody] decimal amount)
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
