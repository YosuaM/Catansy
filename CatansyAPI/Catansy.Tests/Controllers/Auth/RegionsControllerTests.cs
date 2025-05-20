using Catansy.API.Controllers;
using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Services.Interfaces.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catansy.Tests.Controllers.Auth
{
    public class RegionsControllerTests
    {
        private readonly Mock<IRegionService> _regionServiceMock = new();
        private readonly RegionsController _controller;

        public RegionsControllerTests()
        {
            _controller = new RegionsController(_regionServiceMock.Object);
        }


        #region GetRegions
        [Fact]
        public async Task GetRegions_Should_Return_List_Of_Regions()
        {
            var regions = new List<RegionDto>
            {
                new() { Id = Guid.NewGuid(), Name = "EU" },
                new() { Id = Guid.NewGuid(), Name = "NA" }
            };

            _regionServiceMock.Setup(s => s.GetAllRegionsAsync())
                .ReturnsAsync(regions);

            var result = await _controller.GetRegions();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = okResult!.Value as IEnumerable<RegionDto>;
            value.Should().HaveCount(2);
        }
        #endregion


        #region GetServersByRegion
        [Fact]
        public async Task GetServersByRegion_Should_Return_Servers_If_Region_Exists()
        {
            var regionId = Guid.NewGuid();
            var servers = new List<ServerDto>
            {
                new() { Id = Guid.NewGuid(), Name = "EU-1", RegionId = regionId },
                new() { Id = Guid.NewGuid(), Name = "EU-2", RegionId = regionId }
            };

            _regionServiceMock.Setup(s => s.GetServersByRegionAsync(regionId))
                .ReturnsAsync(servers);

            var result = await _controller.GetServersByRegion(regionId);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = okResult!.Value as IEnumerable<ServerDto>;
            value.Should().HaveCount(2);
        }
        #endregion


        #region GetRegionsWithServers
        [Fact]
        public async Task GetRegionsWithServers_Should_Return_Regions_With_Their_Servers()
        {
            var regionsWithServers = new List<RegionWithServersDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "EU",
                    Servers = new List<ServerDto> {
                        new() { Id = Guid.NewGuid(), Name = "EU-1" }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "NA",
                    Servers = new List<ServerDto>
                    {
                        new() { Id = Guid.NewGuid(), Name = "NA-1" },
                        new() { Id = Guid.NewGuid(), Name = "NA-2" }
                    }
                }
            };

            _regionServiceMock.Setup(s => s.GetAllRegionsWithServersAsync())
                .ReturnsAsync(regionsWithServers);

            var result = await _controller.GetRegionsWithServers();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = okResult!.Value as IEnumerable<RegionWithServersDto>;
            value.Should().HaveCount(2);
            value!.First(r => r.Name == "NA").Servers.Should().HaveCount(2);
        }
        #endregion
    }
}
