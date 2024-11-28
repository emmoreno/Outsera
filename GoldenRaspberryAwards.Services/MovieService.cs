using GoldenRaspberryAwards.Entities;
using GoldenRaspberryAwards.Repository.Interfaces;
using GoldenRaspberryAwards.Services.Interfaces;
using System.Reflection.Metadata;

namespace GoldenRaspberryAwards.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        #region Métodos públicos

        public IntervaloPremioEntity? GetIntervaloDePremios()
        {

            var movies = _movieRepository.GetAllMoviesEntities();
            if (movies == null)
                return null;
            if (movies.Count == 0)
                return null;

            var filmesGanhadores = movies.Where(x => x.Winner.ToUpper().Equals("YES"));
            if (!filmesGanhadores.Any())
                return null;

            var produtorVencedor = new Dictionary<string, List<int>>();
            FillProdutoresPorAno(filmesGanhadores, produtorVencedor);

            var intervalos = new List<ProdutorEntity>();
            CalcularIntervaloPorProdutor(produtorVencedor, intervalos);

            var intervaloPremios = new IntervaloPremioEntity();
            intervaloPremios.Max.Add(intervalos.OrderByDescending(i => i.Interval).FirstOrDefault());
            intervaloPremios.Min.Add(intervalos.OrderBy(i => i.Interval).FirstOrDefault());

            return intervaloPremios;
        }

        #endregion
        #region Metódos privados

        private static void CalcularIntervaloPorProdutor(Dictionary<string, List<int>> produtorVencedor, List<ProdutorEntity> intervalos)
        {
            foreach (var produtor in produtorVencedor)
            {
                var years = produtor.Value.OrderBy(y => y).ToList();

                for (int i = 1; i < years.Count; i++)
                {
                    intervalos.Add(new ProdutorEntity
                    {
                        Producer = produtor.Key,
                        Interval = years[i] - years[i - 1],
                        PreviousWin = years[i - 1],
                        FollowingWin = years[i]
                    });
                }
            }
        }

        private static void FillProdutoresPorAno(IEnumerable<MovieEntity> filmesGanhadores, Dictionary<string, List<int>> produtorVencedor)
        {
            foreach (var filme in filmesGanhadores)
            {
                var year = int.Parse(filme.Year!);
                var produtores = filme.Producers?.Split(',').Select(p => p.Trim()) ?? Enumerable.Empty<string>();

                foreach (var produtor in produtores)
                {
                    if (!produtorVencedor.ContainsKey(produtor))
                        produtorVencedor[produtor] = new List<int>();

                    produtorVencedor[produtor].Add(year);
                }
            }
        }
        #endregion
    }
}
