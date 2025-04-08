﻿using combined_wordlist.Server.Models;
using combined_wordlist.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace CombinedWordlist.Server.Tests
{
    public class WordleServiceTests
    {
        private readonly WordleService _service;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMemoryCache _memoryCache;
        private readonly DefaultHttpContext _httpContext;

        public WordleServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContext = new DefaultHttpContext();

           
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                       .Verifiable();
            sessionMock.Setup(x => x.Get(It.IsAny<string>())).Returns((byte[]?)null);

            _httpContext.Session = sessionMock.Object;

            _service = new WordleService(_httpContextAccessorMock.Object, _memoryCache);
        }

        //[Fact]
        //public void GetGame_Should_Create_New_Game()
        //{
        //    // Act
        //    var game = _service.GetGame();

        //    // Assert
        //    Assert.NotNull(game);
        //    Assert.IsType<WordleGame>(game);
        //    Assert.Equal(5, game.WordToGuess.Length); 
        //}

        //[Fact]
        //public void GetGame_Should_Return_Same_Game_From_Cache()
        //{
        //    // Act
        //    var firstGame = _service.GetGame();
        //    var secondGame = _service.GetGame();

        //    // Assert
        //    Assert.Same(firstGame, secondGame); 
        //}

        //[Fact]
        //public void ResetGame_Should_Create_Different_Game()
        //{
        //    // Arrange
        //    var originalGame = _service.GetGame();

        //    // Act
        //    _service.ResetGame();
        //    var newGame = _service.GetGame();

        //    // Assert
        //    Assert.NotSame(originalGame, newGame); 
        //}
    }
}
