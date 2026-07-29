using KinematicsSimulator.Application.Features.Simulations.Queries;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.Entities.ValueObjects;
using FluentAssertions;
using Moq;
using System.Linq;

namespace Kinematics.Application.UnitTests;

public class GetSimulationsTests
{
    [Fact]
    public async Task GetSimulationsHandler_GoodValues_ShouldReturnSimList()
    {
        var userId = Guid.NewGuid();
        int pageNumber = 2;
        int pageSize = 15;
        var query = new GetSimulationsQuery(userId, pageNumber, pageSize);

        var fakeSimRepo = new Mock<ISimulationRepository>();

        var sim1 = KinematicSimulation.Create(userId, SimulationType.Create("MRU").Value, ResultValue.Create(10.5).Value, DateTimeOffset.UtcNow).Value;
        var sim2 = KinematicSimulation.Create(userId, SimulationType.Create("MRUA").Value, ResultValue.Create(20.0).Value, DateTimeOffset.UtcNow).Value;
        var sim3 = KinematicSimulation.Create(userId, SimulationType.Create("MRU").Value, ResultValue.Create(30.2).Value, DateTimeOffset.UtcNow).Value;

        var mockSimulations = new List<KinematicSimulation> { sim1, sim2, sim3 };

        fakeSimRepo
            .Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((mockSimulations, mockSimulations.Count));

        var handler = new GetSimulationsHandler(fakeSimRepo.Object);
        var result = await handler.Handle(query, CancellationToken.None);

        fakeSimRepo.Verify(repo => repo.GetByUserIdAsync(userId, pageNumber, pageSize, It.IsAny<CancellationToken>()), Times.Once);

        result.errorList.Should().BeEmpty();
        result.Value.Should().NotBeNull();
        result.Value.Items.ToList()[0].Should().BeOfType<SimulationDTO>();
        result.Value.Items.Should().HaveCount(3);
        result.Value.Items.ToList()[0].ResultValue.Should().Be(10.5);
    }
}
