using EngConnect.BuildingBlock.Infrastructure.MessageBus;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.BuildingBlocks.Infrastructure.MessageBus;

public class MassTransitServiceTests
{
    [Fact]
    public async Task PublishAsync_and_SendAsync_delegate_to_bus()
    {
        EnsureEndpointConventionMapped();

        var bus = new Mock<IBus>(MockBehavior.Strict);
        var sendEndpoint = new Mock<ISendEndpoint>(MockBehavior.Strict);

        bus.Setup(value => value.Publish(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        bus.Setup(value => value.GetSendEndpoint(It.IsAny<Uri>()))
            .ReturnsAsync(sendEndpoint.Object);

        sendEndpoint.Setup(value => value.Send(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new MassTransitService(bus.Object, NullLogger<MassTransitService>.Instance);

        await sut.PublishAsync(new TestMessage("publish"));
        await sut.SendAsync(new TestMessage("send"));

        bus.Verify(value => value.Publish(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        bus.Verify(value => value.GetSendEndpoint(It.IsAny<Uri>()), Times.Once);
        sendEndpoint.Verify(value => value.Send(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishManyAsync_and_SendManyAsync_delegate_each_message_to_bus()
    {
        EnsureEndpointConventionMapped();

        var bus = new Mock<IBus>(MockBehavior.Strict);
        var sendEndpoint = new Mock<ISendEndpoint>(MockBehavior.Strict);

        bus.Setup(value => value.Publish(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        bus.Setup(value => value.GetSendEndpoint(It.IsAny<Uri>()))
            .ReturnsAsync(sendEndpoint.Object);

        sendEndpoint.Setup(value => value.Send(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new MassTransitService(bus.Object, NullLogger<MassTransitService>.Instance);
        var messages = new[] { new TestMessage("one"), new TestMessage("two") };

        await sut.PublishManyAsync(messages);
        await sut.SendManyAsync(messages);

        bus.Verify(value => value.Publish(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(messages.Length));
        bus.Verify(value => value.GetSendEndpoint(It.IsAny<Uri>()), Times.Exactly(messages.Length));
        sendEndpoint.Verify(value => value.Send(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(messages.Length));
    }

    [Fact]
    public async Task PublishAsync_with_runtime_type_delegates_to_bus()
    {
        var bus = new Mock<IBus>(MockBehavior.Strict);
        bus.Setup(value => value.Publish(It.IsAny<object>(), typeof(TestMessage), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new MassTransitService(bus.Object, NullLogger<MassTransitService>.Instance);

        await sut.PublishAsync(new TestMessage("runtime"), typeof(TestMessage));

        bus.Verify(value => value.Publish(It.IsAny<object>(), typeof(TestMessage), It.IsAny<CancellationToken>()), Times.Once);
    }

    private sealed record TestMessage(string Content);

    private static void EnsureEndpointConventionMapped()
    {
        try
        {
            EndpointConvention.Map<TestMessage>(new Uri("queue:test-message"));
        }
        catch
        {
        }
    }
}
