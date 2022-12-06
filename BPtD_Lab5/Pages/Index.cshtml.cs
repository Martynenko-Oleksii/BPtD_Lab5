using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace BPtD_Lab5.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string DataJson;
        public string Success;
        public string Failure;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            DataJson = "";
            Success = "";
            Failure = "";
        }

        public void OnPost(CallbackDto callback)
        {
            if (callback.Data != null && callback.Signature != null)
            {
                var calculatedSignature = Convert.ToBase64String(SHA1.HashData(Encoding.UTF8.GetBytes(CallbackDto.PrivateKey + callback.Data + CallbackDto.PrivateKey)));

                DataJson = Encoding.UTF8.GetString(Convert.FromBase64String(callback.Data!));
                if (callback.Signature!.Equals(calculatedSignature))
                {
                    Success = "Success";
                }
                else
                {
                    Failure = "Failure";
                }
            }
        }
    }
}