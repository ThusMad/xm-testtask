using Microsoft.Extensions.Logging;
using Moq;
using XmPriceAgg.BLL.AggregationAlgorithms;
using XmPriceAgg.BLL.Data.Interfaces;
using XmPriceAgg.BLL.Interfaces;
using XmPriceAgg.DAL.Entities;
using XmPriceAgg.DAL.Interfaces;
using XmPriceAgg.DAL.Repositories.Interfaces;
using static NUnit.Framework.Assert;

namespace XmPriceAgg.BLL.Tests
{
    [TestFixture]
    public class AggregationServiceTests
    {
        private Mock<ILogger<AggregationService>> _mockLogger;
        private Mock<IDataSources> _mockSources;
        private Mock<IUnitOfWorkFactory> _mockUnitOfWorkFactory;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAggregationRepository> _mockAggregationRepository;
        private AggregationService _aggregationService;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<AggregationService>>();
            _mockSources = new Mock<IDataSources>();
            _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAggregationRepository = new Mock<IAggregationRepository>();

            _mockUnitOfWorkFactory.Setup(m => m.Create()).Returns(_mockUnitOfWork.Object);
            _mockUnitOfWork.Setup(m => m.AggregationRepository).Returns(_mockAggregationRepository.Object);

            _aggregationService = new AggregationService(
                _mockLogger.Object,
                _mockSources.Object,
                new AverageAggregationAlgorithm((new Mock<ILogger<AverageAggregationAlgorithm>>()).Object),
                _mockUnitOfWorkFactory.Object);
        }

        [Test]
        public async Task RetrievePriceAsync_ReturnsCachedPrice_WhenPriceIsCached()
        {
            const int timestamp = 1234567890;
            const float cachedPrice = 1000f;

            _mockAggregationRepository.Setup(m => m.GetStoredPriceAsync(timestamp)).ReturnsAsync(new StoredPriceEntity { Timestamp = timestamp, Price = cachedPrice });

            var result = await _aggregationService.RetrievePriceAsync(timestamp);
            That(result, Is.EqualTo(cachedPrice));
        }

        [Test]
        public async Task RetrievePriceAsync_ReturnsAverageAggregatedPrice_WhenPriceIsNotCached()
        {
            const int timestamp = 1234567890;
            var pricesFromSources = new List<float> { 1500, 2500, 2000 };
            var aggregatedPrice = pricesFromSources.Sum() / 3;
            var providersList = new List<IPriceProvider>();

            foreach (var pricesFromSource in pricesFromSources)
            {
                var provider = new Mock<IPriceProvider>();
                provider.Setup(m => m.GetPriceAsync(timestamp, CancellationToken.None)).ReturnsAsync(pricesFromSource);
                providersList.Add(provider.Object);
            }

            _mockAggregationRepository.Setup(m => m.GetStoredPriceAsync(timestamp)).ReturnsAsync((StoredPriceEntity?)null);
            _mockSources.Setup(m => m.GetProviders()).Returns(providersList);

            var result = await _aggregationService.RetrievePriceAsync(timestamp);
            That(result, Is.EqualTo(aggregatedPrice));
        }
    }
}