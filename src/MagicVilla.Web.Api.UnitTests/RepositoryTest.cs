using NUnit.Framework;
using Moq;
using MagicVilla.Api.Data;
using MagicVilla.Api.Repository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Web.Api.UnitTests
{
    [TestFixture]
    public class RepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<TestEntity>> _dbSetMock;
        private Repository<TestEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            // Create mock DbSet
            _dbSetMock = new Mock<DbSet<TestEntity>>();

            // Create mock ApplicationDbContext with a specific constructor
            var options = new DbContextOptions<ApplicationDbContext>();
            _dbContextMock = new Mock<ApplicationDbContext>(options);

            // Setup DbSet return value
            _dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(_dbSetMock.Object);

            // Initialize the repository with the mock context
            _repository = new Repository<TestEntity>(_dbContextMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldAddEntity()
        {
            var entity = new TestEntity();

            await _repository.CreateAsync(entity);

            _dbSetMock.Verify(dbSet => dbSet.AddAsync(entity, default), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveEntity()
        {
            var entity = new TestEntity();

            await _repository.RemoveAsync(entity);

            _dbSetMock.Verify(dbSet => dbSet.Remove(entity), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var data = new List<TestEntity> { new TestEntity(), new TestEntity() }.AsQueryable();
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetAsync_ShouldReturnEntity()
        {
            var data = new List<TestEntity> { new TestEntity { Id = 1 }, new TestEntity { Id = 2 } }.AsQueryable();
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<TestEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var result = await _repository.GetAsync(e => e.Id == 1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }

    public class TestEntity
    {
        public int Id { get; set; }
    }
}