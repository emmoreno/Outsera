using CsvHelper;
using CsvHelper.Configuration;
using GoldenRaspberryAwards.Entities;
using GoldenRaspberryAwards.Entities.CsvMapper;
using GoldenRaspberryAwards.Repository.Interfaces;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace GoldenRaspberryAwards.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly SqliteConnection _connection;
        private static bool _isInitialized = false;
        public MovieRepository(SqliteConnection connection)
        {
            _connection = connection;
            if (!_isInitialized)
            {
                _isInitialized = LoadMovies();
            }
        }
        #region Métodos privados
        private bool LoadMovies()
        {
            try
            {
                string csvFilePath = "../movielist.csv";

                // Criar tabela
                CreateTable(_connection);

                // Ler o arquivo CSV e inserir no banco de dados
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(csvFilePath))

                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<MovieEntityMap>();
                    var movies = new List<MovieEntity>(csv.GetRecords<MovieEntity>());

                    Insert(_connection, movies);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void Insert(SqliteConnection connection, List<MovieEntity> movies)
        {
            foreach (var movie in movies)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                            INSERT INTO Movies (Year, Title, Studios, Producers, Winner)
                            VALUES ($year, $title, $studios, $producers, $winner);
                        ";
                    command.Parameters.AddWithValue("$year", movie.Year);
                    command.Parameters.AddWithValue("$title", movie.Title);
                    command.Parameters.AddWithValue("$studios", movie.Studios);
                    command.Parameters.AddWithValue("$producers", movie.Producers);
                    command.Parameters.AddWithValue("$winner", movie.Winner);

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateTable(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE Movies (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Year INTEGER,
                        Title TEXT,
                        Studios TEXT,
                        Producers TEXT,
                        Winner BOOLEAN
                    );";
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Métodos publicos

        public List<MovieEntity> GetAllMoviesEntities()
        {
            var movies = new List<MovieEntity>();
            var command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM Movies;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var movie = new MovieEntity
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? null : reader.GetString(reader.GetOrdinal("Year")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        Studios = reader.IsDBNull(reader.GetOrdinal("Studios")) ? null : reader.GetString(reader.GetOrdinal("Studios")),
                        Producers = reader.IsDBNull(reader.GetOrdinal("Producers")) ? null : reader.GetString(reader.GetOrdinal("Producers")),
                        Winner = reader.IsDBNull(reader.GetOrdinal("Winner")) ? null : reader.GetString(reader.GetOrdinal("Winner"))
                    };

                    movies.Add(movie);
                }
            }

            return movies;
        }

        #endregion
    }
}
