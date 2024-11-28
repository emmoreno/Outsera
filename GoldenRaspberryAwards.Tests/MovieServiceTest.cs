using GoldenRaspberryAwards.Entities;
using GoldenRaspberryAwards.Repository.Interfaces;
using GoldenRaspberryAwards.Services;
using Moq;

namespace GoldenRaspberryAwards.Tests
{
    public class MovieServiceTest
    {


        [Fact]
        public void GetIntervaloDePremiosWhenIsNull()
        {
            
            var mockRepository = new Mock<IMovieRepository>();
            mockRepository.Setup(repo => repo.GetAllMoviesEntities()).Returns((List<MovieEntity>?)null);

            var service = new MovieService(mockRepository.Object);

            // Act
            var result = service.GetIntervaloDePremios();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetIntervaloDePremiosWhenNoWinners()
        {
            
            var mockRepository = new Mock<IMovieRepository>();
            var movies = new List<MovieEntity>{
                new MovieEntity { Year = "2000", Producers = "Producer1", Winner = "No" }};
            mockRepository.Setup(repo => repo.GetAllMoviesEntities()).Returns(movies);

            var service = new MovieService(mockRepository.Object);

            // Act
            var result = service.GetIntervaloDePremios();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetIntervaloDePremiosWhenWinnersExist()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            var movies = new List<MovieEntity>{
                    new MovieEntity { Year = "2000", Producers = "Producer1", Winner = "Yes" },
                    new MovieEntity { Year = "2002", Producers = "Producer1", Winner = "Yes" },
                    new MovieEntity { Year = "2010", Producers = "Producer2", Winner = "Yes" },
                    new MovieEntity { Year = "2015", Producers = "Producer2", Winner = "Yes" }};

            mockRepository.Setup(repo => repo.GetAllMoviesEntities()).Returns(movies);

            var service = new MovieService(mockRepository.Object);

            
            var result = service.GetIntervaloDePremios();

            
            Assert.NotNull(result);
           
        }
    }
}