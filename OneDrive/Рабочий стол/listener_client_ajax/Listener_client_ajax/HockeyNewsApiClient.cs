using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Listener_client_ajax;

public class HockeyNewsApiClient
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://sports-information.p.rapidapi.com/nhl/news"),
        DefaultRequestHeaders = 
    {
        {
            "x-rapidapi-host", "sports-information.p.rapidapi.com"
        },
        {
            "x-rapidapi-key", "1b82a42f4amsh138fed80b713d8ap1389f7jsn7d5356c7c3db"
        }
    }
    };
    public async Task<string> GetHockeyNewsAsync()
    {
        var response = await _httpClient.GetAsync("/nhl/news");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();

    }
}
