using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Services.Implementation.Auth;
using Catansy.Domain.Auth;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catansy.Tests.Auth
{
    public class RegionServiceTests
    {
        private readonly Mock<IRegionRepository> _regionRepoMock = new();
        private readonly RegionService _sut;

        public RegionServiceTests()
        {
            _sut = new RegionService(_regionRepoMock.Object);
        }


        #region GetAllRegions
        [Fact]
        public async Task GetAllRegionsAsync_Should_Return_RegionDtos()
        {
            var regions = new List<Region>
            {
                new() { Id = Guid.NewGuid(), Name = "Europe", Enabled = true },
                new() { Id = Guid.NewGuid(), Name = "North America", Enabled = true }
            };

            _regionRepoMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(regions);

            var result = await _sut.GetAllRegionsAsync();

            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Europe");
        }
        #endregion


        #region GetAllRegionsWithServers
        [Fact]
        public async Task GetServersByRegionAsync_Should_Return_Servers()
        {
            var regionId = Guid.NewGuid();
            var servers = new List<Server>
            {
                new() { Id = Guid.NewGuid(), Name = "EU-1", RegionId = regionId, Enabled = true },
                new() { Id = Guid.NewGuid(), Name = "EU-2", RegionId = regionId, Enabled = true }
            };

            _regionRepoMock.Setup(r => r.GetServersByRegionAsync(regionId))
                           .ReturnsAsync(servers);

            var result = await _sut.GetServersByRegionAsync(regionId);

            result.Should().HaveCount(2);
            result.First().Name.Should().Be("EU-1");
        }
        #endregion


        #region GetAllRegionsWithServers
        [Fact]
        public async Task GetAllRegionsWithServersAsync_Should_Return_RegionsWithNestedServers()
        {
            // Arrange
            var region1Id = Guid.NewGuid();
            var region2Id = Guid.NewGuid();

            var regions = new List<Region>
            {
                new() { Id = region1Id, Name = "EU", Enabled = true },
                new() { Id = region2Id, Name = "NA", Enabled = true }
            };

            var serversRegion1 = new List<Server>
            {
                new() { Id = Guid.NewGuid(), Name = "EU-1", RegionId = region1Id, Enabled = true }
            };

            var serversRegion2 = new List<Server>
            {
                new() { Id = Guid.NewGuid(), Name = "NA-1", RegionId = region2Id, Enabled = true },
                new() { Id = Guid.NewGuid(), Name = "NA-2", RegionId = region2Id, Enabled = true }
            };

            _regionRepoMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(regions);

            _regionRepoMock.Setup(r => r.GetServersByRegionAsync(region1Id))
                           .ReturnsAsync(serversRegion1);

            _regionRepoMock.Setup(r => r.GetServersByRegionAsync(region2Id))
                           .ReturnsAsync(serversRegion2);

            // Act
            var result = await _sut.GetAllRegionsWithServersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First(r => r.Name == "EU").Servers.Should().HaveCount(1);
            result.First(r => r.Name == "NA").Servers.Should().HaveCount(2);
        }
        
        [Fact]
        public async Task GetAllRegionsWithServersAsync_Should_Only_Return_Enabled_Regions_And_Servers()
        {
            // Arrange
            var region1Id = Guid.NewGuid();
            var region2Id = Guid.NewGuid();

            var regions = new List<Region>
            {
                new() { Id = region1Id, Name = "EU", Enabled = true },
                new() { Id = region2Id, Name = "NA", Enabled = false }
            };

            var serversRegion1 = new List<Server>
            {
                new() { Id = Guid.NewGuid(), Name = "EU-1", RegionId = region1Id, Enabled = true },
                new() { Id = Guid.NewGuid(), Name = "EU-2", RegionId = region1Id, Enabled = false }
            };

            _regionRepoMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(regions);

            _regionRepoMock.Setup(r => r.GetServersByRegionAsync(region1Id))
                           .ReturnsAsync(serversRegion1);

            // Act
            var result = await _sut.GetAllRegionsWithServersAsync();

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("EU");

            result.First().Servers.Should().HaveCount(1);
            result.First().Servers.First().Name.Should().Be("EU-1");
        }
        #endregion
    }
}
