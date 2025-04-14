namespace wof_utilization_mcp_server;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class WofParser
{
    private readonly HttpClient _httpClient;
    
    public WofParser()
    {
        _httpClient = new HttpClient();
    }
    
    public WofParser(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public async Task<List<WofFacility>> ParseWofFacilitiesAsync(string url = "https://app.wof.de/besucher/")
    {
        // Fetch the HTML content
        var htmlContent = await _httpClient.GetStringAsync(url);
        return ParseWofFacilitiesFromHtml(htmlContent);
    }

    private static List<WofFacility> ParseWofFacilitiesFromHtml(string htmlContent)
    {
        var facilities = new List<WofFacility>();
        
        // Regular expression to find facility rows
        const string facilityPattern = @"<tr>\s*<td>\s*(WOF\s+\d+\s+–\s+[^<]+)\s*</td>\s*<td>[^<]*<div[^>]*>\s*<div[^>]*width:\s*(\d+)%[^>]*>(\d+)%\s*</div>";
        
        var matches = Regex.Matches(htmlContent, facilityPattern, RegexOptions.Singleline);
        
        foreach (Match match in matches)
        {
            if (match.Groups.Count < 4) continue;
            var fullName = match.Groups[1].Value.Trim();
                
            // Extract facility code (WOF X)
            var codeMatch = Regex.Match(fullName, @"(WOF\s+\d+)");
            var facilityCode = codeMatch.Success ? codeMatch.Groups[1].Value.Replace(" ", "") : string.Empty;
                
            // Extract utilization percentage
            var utilization = 0;
            if (int.TryParse(match.Groups[3].Value, out var result))
            {
                utilization = result;
            }
                
            facilities.Add(new WofFacility(facilityCode, fullName, utilization));
        }
        
        return facilities;
    }
}

public record WofFacility(string FacilityCode, string FacilityName, int CurrentLockerUtilization);