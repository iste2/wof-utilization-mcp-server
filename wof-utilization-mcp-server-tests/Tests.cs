using wof_utilization_mcp_server;

namespace wof_utilization_mcp_server_tests;

public class Tests
{
    [Test]
    public async Task ParseWofFacilitiesAsync_TestNotEmptyAndReasonableResultsAsync()
    {
        // Arrange
        var wofParser = new WofParser();
        
        // Act
        var results = await wofParser.ParseWofFacilitiesAsync();

        // Assert
        Assert.IsNotNull(results);
        Assert.IsNotEmpty(results);
        Assert.That(results, Has.All.Matches<WofFacility>(f => !string.IsNullOrEmpty(f.FacilityCode)));
        Assert.That(results, Has.All.Matches<WofFacility>(f => !string.IsNullOrEmpty(f.FacilityName)));
        Assert.That(results, Has.All.Matches<WofFacility>(f => f.CurrentLockerUtilization is >= 0 and <= 100));
        Assert.That(results, Has.All.Matches<WofFacility>(f => f.FacilityCode.StartsWith("WOF")));
    }
}