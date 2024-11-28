using GoldenRaspberryAwards.Entities;

namespace GoldenRaspberryAwards.Repository.Interfaces
{
    public interface IMovieRepository
    {
        List<MovieEntity> GetAllMoviesEntities();
    }
}