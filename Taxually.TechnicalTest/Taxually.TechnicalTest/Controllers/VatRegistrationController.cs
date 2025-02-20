using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Core.Exceptions;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Infrastructure.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    /// <summary>
    /// Registers a company for a VAT number in a given country
    /// </summary>
    [ApiController]
    [Route("api/vat")]
    public class VatRegistrationController : ControllerBase
    {
        private readonly VatRegistrationService _service;

        public VatRegistrationController(VatRegistrationService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterVat([FromBody] VatRegistrationRequest request)
        {
            try
            {
                await _service.RegisterVatAsync(request);
                return Ok(ApiResponse.SuccessResponse("VAT registration successful."));
            }
            catch (UnsupportedCountryException ex)
            {
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (VatRegistrationException ex)
            {
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse($"An unexpected error occurred: {ex.Message}"));
            }
        }
    }
}
