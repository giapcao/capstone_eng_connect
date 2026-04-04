using System.Linq.Expressions;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Infrastructure.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Infrastructure.Services;

public class MessageBusWithOutboxServiceTests
{
    [Fact]
    public async Task PublishWithOutboxFallbackAsync_publishes_directly_when_bus_succeeds()
    {
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var messageBus = new Mock<IMessageBusService>(MockBehavior.Strict);

        messageBus
            .Setup(service => service.PublishAsync(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new MessageBusWithOutboxService(unitOfWork.Object, messageBus.Object, NullLogger<MessageBusWithOutboxService>.Instance);

        await sut.PublishWithOutboxFallbackAsync(new TestMessage("message"), "user", Guid.NewGuid());

        messageBus.Verify(service => service.PublishAsync(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(work => work.GetRepository<OutboxEvent, Guid>(), Times.Never);
        unitOfWork.Verify(work => work.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task PublishWithOutboxFallbackAsync_persists_outbox_when_bus_publish_fails()
    {
        var repository = new InMemoryOutboxRepository();
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var messageBus = new Mock<IMessageBusService>(MockBehavior.Strict);

        messageBus
            .Setup(service => service.PublishAsync(It.IsAny<TestMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Publish failed."));

        unitOfWork
            .Setup(work => work.GetRepository<OutboxEvent, Guid>())
            .Returns(repository);

        unitOfWork
            .Setup(work => work.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(1);

        var aggregateId = Guid.NewGuid();
        var sut = new MessageBusWithOutboxService(unitOfWork.Object, messageBus.Object, NullLogger<MessageBusWithOutboxService>.Instance);

        await sut.PublishWithOutboxFallbackAsync(new TestMessage("message"), "user", aggregateId);

        Assert.Single(repository.Events);
        Assert.Equal("user", repository.Events[0].AggregateType);
        Assert.Equal(aggregateId, repository.Events[0].AggregateId);
        unitOfWork.Verify(work => work.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
    }

    private sealed record TestMessage(string Content);

    private sealed class InMemoryOutboxRepository : IGenericRepository<OutboxEvent, Guid>
    {
        public List<OutboxEvent> Events { get; } = [];

        public Task<OutboxEvent?> FindByIdAsync(Guid id, bool tracking = true, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => Task.FromResult<OutboxEvent?>(Events.FirstOrDefault(@event => @event.Id == id));

        public Task<OutboxEvent?> FindSingleAsync(Expression<Func<OutboxEvent, bool>>? predicate = null, bool tracking = true, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => Task.FromResult(predicate == null ? Events.SingleOrDefault() : Events.AsQueryable().SingleOrDefault(predicate));

        public Task<OutboxEvent?> FindFirstAsync(Expression<Func<OutboxEvent, bool>>? predicate = null, bool tracking = true, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => Task.FromResult(predicate == null ? Events.FirstOrDefault() : Events.AsQueryable().FirstOrDefault(predicate));

        public Task<bool> AnyAsync(Expression<Func<OutboxEvent, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => Task.FromResult(predicate == null ? Events.Count != 0 : Events.AsQueryable().Any(predicate));

        public IQueryable<OutboxEvent> FindAll(Expression<Func<OutboxEvent, bool>>? predicate = null, bool tracking = false, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => predicate == null ? Events.AsQueryable() : Events.AsQueryable().Where(predicate);

        public IQueryable<OutboxEvent> FindFromSqlInterpolated(FormattableString sql, CancellationToken cancellationToken = default)
            => Events.AsQueryable();

        public Task<int> CountAsync(Expression<Func<OutboxEvent, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<OutboxEvent, object>>[] includes)
            => Task.FromResult(predicate == null ? Events.Count : Events.AsQueryable().Count(predicate));

        public void Add(OutboxEvent entity) => Events.Add(entity);

        public void Update(OutboxEvent entity)
        {
        }

        public void Delete(OutboxEvent entity) => Events.Remove(entity);

        public void AddRange(IEnumerable<OutboxEvent> entities) => Events.AddRange(entities);

        public void UpdateRange(IEnumerable<OutboxEvent> entities)
        {
        }

        public void DeleteRange(IEnumerable<OutboxEvent> entities)
        {
            foreach (var entity in entities.ToList())
            {
                Events.Remove(entity);
            }
        }
    }
}
