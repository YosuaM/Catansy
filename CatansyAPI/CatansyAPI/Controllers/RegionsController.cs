using Catansy.Applicaton.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catansy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegionsController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionsController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _regionService.GetAllRegionsAsync();
        return Ok(regions);
    }

    [HttpGet("{regionId}/Servers")]
    public async Task<IActionResult> GetServersByRegion(Guid regionId)
    {
        var servers = await _regionService.GetServersByRegionAsync(regionId);
        return Ok(servers);
    }

    [HttpGet("Full")]
    public async Task<IActionResult> GetRegionsWithServers()
    {
        var result = await _regionService.GetAllRegionsWithServersAsync();
        return Ok(result);
    }
}
