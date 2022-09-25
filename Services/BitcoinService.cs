using API.DTOs;
using Newtonsoft.Json.Linq;

namespace API.Services
{
    public class BitcoinService : IBitcoinService
    {
        private readonly HttpClient _httpClient;
        public BitcoinService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("APIBLAZOR");
        }

        public async Task<List<BitcoinDataDTO>> FindBy(DateTime startDate)
        {
            var response = await _httpClient.GetAsync("time-series?start=" + startDate.ToString("yyyy-MM-dd") + "&interval=1d");


            var jsonResult = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            Console.WriteLine("Testando Api", jsonResult);

            JObject jObject = JObject.Parse(jsonResult);
            var values = jObject.SelectToken("data.values").ToString();
            if (string.IsNullOrWhiteSpace(values))
                return new List<BitcoinDataDTO>();

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<decimal[]>>(values);
           
            return data.Select(d => new BitcoinDataDTO(new DateTime(1970, 1, 1).AddMilliseconds((long)d[0]), d[3])).ToList();

         
        }
    }
}
