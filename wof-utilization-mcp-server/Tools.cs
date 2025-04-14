using System.ComponentModel;
using ModelContextProtocol.Server;

namespace wof_utilization_mcp_server;

[McpServerToolType]
public class Tools
{
    [McpServerTool(Name = "get_wof_utilization"), Description("Retrieves a list of all WOF gyms with their current locker utilization.")]
    public async Task<List<WofFacility>> GetWofFacilitiesAsync()
    {
        var wofParser = new WofParser();
        return await wofParser.ParseWofFacilitiesAsync();
    }
}