using AoiCryptoAPI.Models;
using AoiCryptoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AoiCryptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowlistController : ControllerBase
    {
        private readonly AllowlistService _allowlistService;

        public AllowlistController(AllowlistService allowlistService)
        {
            _allowlistService = allowlistService;
        }

        [HttpGet]
        public ActionResult<List<AllowlistEntry>> GetAll() => _allowlistService.Get();

        [HttpGet("pool/{poolAddress}")]
        public ActionResult<List<AllowlistEntry>> GetByPoolAddress(string poolAddress)
        {
            var entries = _allowlistService.GetByPoolAddress(poolAddress);
            if (entries == null || entries.Count == 0)
            {
                return NotFound();
            }
            return Ok(entries);
        }

        [HttpGet("user/{userAddress}")]
        public ActionResult<List<AllowlistEntry>> GetByUserAddress(string userAddress)
        {
            var entries = _allowlistService.GetByUserAddress(userAddress);
            if (entries == null || entries.Count == 0)
            {
                return NotFound();
            }
            return Ok(entries);
        }

        [HttpPost("bulk")]
        public ActionResult CreateBulk([FromBody] BulkAllowlistRequest request)
        {
            var entries = new List<AllowlistEntry>();

            foreach (var userAddress in request.UserAddresses)
            {
                entries.Add(new AllowlistEntry
                {
                    PoolAddress = request.PoolAddress,
                    UserAddress = userAddress,
                    Status = request.Status
                });
            }

            _allowlistService.CreateBulk(entries);

            return Ok(new { message = "Allowlist entries created successfully." });
        }

        [HttpGet("{id}")]
        public ActionResult<AllowlistEntry> GetById(string id)
        {
            var entry = _allowlistService.GetById(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var entry = _allowlistService.GetById(id);
            if (entry == null)
            {
                return NotFound();
            }
            _allowlistService.Delete(id);
            return NoContent();
        }

        [HttpPost("is-allowed/{poolAddress}/{userAddresses}")]
        public ActionResult IsAllow(string poolAddress, List<string> userAddresses)
        {
            var isAllowed = _allowlistService.IsAllowed(userAddresses, poolAddress);
            return Ok(new { isAllowed });
        }
    }

    public class BulkAllowlistRequest
    {
        public string PoolAddress { get; set; }
        public List<string> UserAddresses { get; set; }
        public string Status { get; set; }
    }
}
