using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using Converter.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Converter.WebApi.Controllers
{
    [ApiController]
    [Route("convert")]
    public class ConverterController : ControllerBase
    {

        private readonly ILogger<ConverterController> logger;

        public ConverterController(ILogger<ConverterController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("{value}")]
        public IActionResult Get([Required, FromRoute] string value) 
        {
            // validate input
            if (!CurrencyToWordConverter.IsValidNumericString(value) ||
                !decimal.TryParse(
                    WebUtility.UrlDecode(value).Replace(",","."),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out var number)) {

                return BadRequest(new {
                    Error = $"Incorrect input. Use digits only, no leading zeros and ',' as a decimal separator wih max 2 fractional numbers",
                    Result = (string)null,
                });
            }

            // convert
            try {
                return Ok(new {
                    Error = (string)null,
                    Result = CurrencyToWordConverter.ToWords(number),
                });
            }
            catch (ArgumentOutOfRangeException are) {
                return BadRequest(new {
                    Error = are.Message,
                    Result = (string)null,
                });
            }
        }
    }
}
