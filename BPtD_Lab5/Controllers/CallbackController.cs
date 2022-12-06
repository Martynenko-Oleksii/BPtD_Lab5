using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BPtD_Lab5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly ILogger<CallbackController> _logger;

        public CallbackController(ILogger<CallbackController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task Callback(CallbackDto callback)
        {
            if (callback.Data != null && callback.Signature != null)
            {
                _logger.LogInformation($"data (base64) = {callback.Data}");
                var dataJson = Convert.FromBase64String(callback.Data);
                _logger.LogInformation($"data (JSON) = {Encoding.UTF8.GetString(dataJson)}");

                _logger.LogInformation($"signature = {callback.Signature}");

                var calculatedSignature = Convert.ToBase64String(SHA1.HashData(Encoding.UTF8.GetBytes(CallbackDto.PrivateKey + callback.Data + CallbackDto.PrivateKey)));
                _logger.LogInformation($"Success = {callback.Signature.Equals(calculatedSignature)}");
            }

            return Task.CompletedTask;
        }
    }
}
