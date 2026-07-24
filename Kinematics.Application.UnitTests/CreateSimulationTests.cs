using FluentAssertions;
using KinematicsSimulator.Application.Features.Simulations.Commands;
using KinematicsSimulator.Application.Interfaces;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using Moq;

namespace Kinematics.Application.UnitTests;

public class CreateSimulationTests
{
    [Fact]
    public async Task CreateSimulationHandler_InvalidSimType_ShoundReturnSimulationTypeCreationError()
    {
        const string BadSimType = "MRU_BAD";

        var command = new CreateSimulationCommand(Guid.NewGuid(), BadSimType, null, 10.0, 4.0, null, 7.0, null, "S");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "SimulationType.WrongType");

        fakeSimRepo.Verify(repo => repo.AddAsync(It.IsAny<KinematicSimulation>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateSimulationHandler_InvalidParams_ShoundReturnInvalidParametersError()
    {
        const string SimType = "MRU";

        var command = new CreateSimulationCommand(Guid.NewGuid(), SimType, null, null, null, null, null, null, "S");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "CreateSimulation.InvalidParameters");

        fakeSimRepo.Verify(repo => repo.AddAsync(It.IsAny<KinematicSimulation>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateSimulationHandler_InvalidTargetVarParam_ShoundReturnInvalidTargetError()
    {
        const string SimType = "MRU";

        var command = new CreateSimulationCommand(Guid.NewGuid(), SimType, null, 10.0, 4.0, null, 7.0, null, "");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "CreateSimulation.MissingTargetVariable");

        fakeSimRepo.Verify(repo => repo.AddAsync(It.IsAny<KinematicSimulation>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateSimulationHandler_EmptyUserId_ShoundReturnKinematicSimulationCreationError()
    {
        const string SimType = "MRU";

        var command = new CreateSimulationCommand(Guid.Empty, SimType, null, 10.0, 4.0, null, 7.0, null, "S");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "KinematicSimulation.EmptyUserId");

        fakeSimRepo.Verify(repo => repo.AddAsync(It.IsAny<KinematicSimulation>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateSimulationHandler_EmptyUserId_ShoundReturnResultCalculationError()
    {
        const string SimType = "MRU";

        var command = new CreateSimulationCommand(Guid.Empty, SimType, 10.0, null, 0.0, null, null, null, "T");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "PhysicsEngine.MRU_TimeZeroVelocityValue");

        fakeSimRepo.Verify(repo => repo.AddAsync(It.IsAny<KinematicSimulation>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task CreateSimulationHandler_ValidMruRequest_ShouldCalculateAndSaveSimulation()
    {
        var userId = Guid.NewGuid();
        var command = new CreateSimulationCommand(userId, "MRU", null, 5.0, 10.0, null, 0.0, null, "S");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Result.Should().Be(50.0);

        fakeSimRepo.Verify(repo => repo.AddAsync(It.Is<KinematicSimulation>(s => 
            s.UserId == userId && 
            s.SimulationType.Value == "MRU" && 
            s.ResultValue.Value == 50.0
        ), It.IsAny<CancellationToken>()), Times.Once);

        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSimulationHandler_ValidMruaRequest_ShouldCalculateAndSaveSimulation()
    {
        var userId = Guid.NewGuid();
        var command = new CreateSimulationCommand(userId, "MRUA", null, 4.0, null, 2.0, null, 5.0, "V");

        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakeSimRepo = new Mock<ISimulationRepository>();

        var handler = new CreateSimulationHandler(fakeAppDbContext.Object, fakeSimRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Result.Should().Be(13.0);

        fakeSimRepo.Verify(repo => repo.AddAsync(It.Is<KinematicSimulation>(s => 
            s.UserId == userId && 
            s.SimulationType.Value == "MRUA" && 
            s.ResultValue.Value == 13.0
        ), It.IsAny<CancellationToken>()), Times.Once);

        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
