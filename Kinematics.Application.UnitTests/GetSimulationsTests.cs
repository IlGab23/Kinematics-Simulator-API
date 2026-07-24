using KinematicsSimulator.Application.Features.Simulations.Queries;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.Entities.ValueObjects;
using FluentAssertions;
using Moq;

namespace Kinematics.Application.UnitTests;

public class GetSimulationsTests
{
    [Fact]
    public async Task GetSimulationsHandler_GoodValues_ShouldReturnSimList()
    {
        var userId = Guid.NewGuid();
        var query = new GetSimulationsQuery(userId);

        var fakeSimRepo = new Mock<ISimulationRepository>();

        var sim1 = KinematicSimulation.Create(userId, SimulationType.Create("MRU").Value, ResultValue.Create(10.5).Value, DateTimeOffset.UtcNow).Value;
        var sim2 = KinematicSimulation.Create(userId, SimulationType.Create("MRUA").Value, ResultValue.Create(20.0).Value, DateTimeOffset.UtcNow).Value;
        var sim3 = KinematicSimulation.Create(userId, SimulationType.Create("MRU").Value, ResultValue.Create(30.2).Value, DateTimeOffset.UtcNow).Value;

        var mockSimulations = new List<KinematicSimulation> { sim1, sim2, sim3 };

        fakeSimRepo
            .Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockSimulations);

        var handler = new GetSimulationsHandler(fakeSimRepo.Object);
        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<SimulationsOutput>();
        result.Value.Simulations[0].Should().BeOfType<SimulationDTO>();
        result.Value.Simulations.Should().HaveCount(3);
        result.Value.Simulations[0].ResultValue.Should().Be(10.5);
    }
}
