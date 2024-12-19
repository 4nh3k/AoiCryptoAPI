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
            return Ok(entries.GroupBy(e => e.PoolAddress));
        }

        [HttpGet("user/{userAddress}")]
        public ActionResult<List<AllowlistEntry>> GetByUserAddress(string userAddress)
        {
            var entries = _allowlistService.GetByUserAddress(userAddress);
            if (entries == null || entries.Count == 0)
            {
                return NotFound();
            }
            return Ok(entries.GroupBy(e => e.UserAddress));
        }

        [HttpPost("bulk")]
        public ActionResult CreateBulk([FromBody] BulkAllowlistRequest request)
        {
            var entries = new List<AllowlistEntry>();

            foreach (var userInfor in request.UserInfors)
            {
                entries.Add(new AllowlistEntry
                {
                    PoolAddress = request.PoolAddress,
                    UserAddress = userInfor.UserAddress,
                    EmailAddress = userInfor.EmailAddress,
                    UserFullName = userInfor.UserFullName,
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

        [HttpGet("is-allowed/{poolAddress}/{userAddress}")]
        public ActionResult IsAllow(string poolAddress, string userAddress)
        {
            var isAllowed = _allowlistService.IsAllowed(userAddress, poolAddress);
            return Ok(new { isAllowed });
        }
    }

    public class BulkAllowlistRequest
    {
        public string PoolAddress { get; set; }
        public List<UserInformation> UserInfors { get; set; }
        public string Status { get; set; }
    }

    public class UserInformation
    {
        public string UserAddress { get; set; }
        public string EmailAddress { get; set; }
        public string UserFullName { get; set; }
    }
}
