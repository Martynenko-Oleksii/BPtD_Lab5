using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BPtD_Lab5.Pages
{
    public class DataDto
    {
        public int version { get; set; }

        public string action { get; set; }

        public string amount { get; set; }

        public string currency { get; set; }

        public string description { get; set; }

        public string public_key { get; set; }

        public string language { get; set; }

        public int subscribe { get; set; }

        public string subscribe_date_start { get; set; }

        public string subscribe_periodicity { get; set; }
    }

    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DataDto _data;

        public string Data;
        public string Signature;

        public string DataJson;
        public string Success;
        public string Failure;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _data = new DataDto()
            {
                version = 3,
                action = "subscribe",
                amount = "40",
                currency = "UAH",
                description = "Premium",
                public_key = CallbackDto.PublicKey,
                language = "uk",
                subscribe = 1,
                subscribe_date_start = "now",
                subscribe_periodicity = "month"
            };
        }

        public void OnGet()
        {
            Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data)));
            Signature = Convert.ToBase64String(SHA1.HashData(Encoding.UTF8.GetBytes(CallbackDto.PrivateKey + Data + CallbackDto.PrivateKey)));

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