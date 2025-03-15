using System;

using Microsoft.EntityFrameworkCore;

using Xunit;
using HashidsNet;


using Short_URL_INFORCE.Data;
using Short_URL_INFORCE.Data.UrlRepository;
using Short_URL_INFORCE.Services;
using Short_URL_INFORCE.Models;


//An assembly specified in the app dependencies manifest (testhost.deps.json) was not found: package MicrosoftTestPlatform.CommunicationUtilities

namespace Short_URL_INFORCE.Tests.Repositories
{
    public class UrlRepositoryTest
    {
        private readonly AppDBContext _context;
        private readonly UrlRepository _repository;
        private readonly HashidsService _hashidsService;

        public UrlRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDBContext(options);

            var hashids = new Hashids("TestSalt", 6);
            _hashidsService = new HashidsService(hashids);

            _repository = new UrlRepository(_context, _hashidsService);
        }

        [Fact]
        public void CreateUrl_ShouldGenerateShortUrl()
        {
            // Arrange
            string originalUrl = "https://example.com/home/page";
            string userId = "testUser123";

            // Act
            var result = _repository.CreateUrl(originalUrl, userId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ShortUrl);
            Assert.Equal("https://example.com/abobus/page/3", result.BaseUrl);
            Assert.Equal("/home/page", result.PathUrl);
        }

    }
}
