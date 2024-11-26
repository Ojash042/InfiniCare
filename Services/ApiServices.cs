using System.Net;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Infinicare_Ojash_Devkota.Services;

public class ApiServices {
    private string _foreignRateURL =
        "https://www.nrb.org.np/";
    
    public ApiServices() {}

    public async Task<double?> GetRates() {
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri(_foreignRateURL);
            try {
                String uri = $"/api/forex/v1/rates?page=1&per_page=5&from={DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Date}&to={DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.OK) {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseData);
                    JArray ratesList = (JArray)json["data"]["payload"].First["rates"];
                    var MyrRate = ratesList.SingleOrDefault(rate=> rate["currency"]["iso3"].ToString().Equals("MYR"));
                    if (MyrRate != null) {
                        return (double) MyrRate["buy"] / (double) MyrRate["currency"]["unit"];
                    }
                }
            }
            catch (Exception e) {
               Console.WriteLine(e); 
            }
        }
        return null;
    }
}