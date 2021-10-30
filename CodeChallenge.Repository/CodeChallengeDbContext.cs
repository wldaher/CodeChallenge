using CodeChallenge.Repository.DTOs;
using CodeChallenge.Repository.Entities;

using Dapper;

using System.Data.SqlClient;

namespace CodeChallenge.Repository
{
    public class CodeChallengeDbContext: ICodeChallengeDbContext
    {
        private readonly string _connString;

        public CodeChallengeDbContext(string connString)
        {
            this._connString = connString;
        }

        public IEnumerable<FlooringDTO> SearchFlooring(string? manufacturer, string? type, string? color, string? style)
        {
            var sql = @"SELECT 
                            m.name AS manufacturer, 
                            t.name AS type, 
                            s.name AS style, 
                            s.style_key AS styleKey, 
                            c.name AS color, 
                            c.color_number AS colorNumber, 
                            f.size
                        FROM flooring f
                        INNER JOIN manufacturers m ON f.manufacturer_id = m.id
                        INNER JOIN styles s ON f.style_id = s.id
                        INNER JOIN types t ON f.type_id = t.id
                        INNER JOIN colors c ON f.color_id = c.id
                        WHERE 
                            (@Manufacturer IS NULL OR m.name LIKE @Manufacturer)
                            AND (@Type IS NULL OR t.name LIKE @Type)
                            AND (@Color IS NULL OR c.name LIKE @Color)
                            AND (@Style IS NULL OR s.name LIKE @Style)";
            IEnumerable<FlooringDTO> dtos = new List<FlooringDTO>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                var dictionary = new Dictionary<string, object>
                {
                    { "@Manufacturer", string.IsNullOrWhiteSpace(manufacturer) ? null : $"%{manufacturer.Trim()}%" },
                    { "@Type", string.IsNullOrWhiteSpace(type) ? null : $"%{type.Trim()}%" },
                    { "@Color", string.IsNullOrWhiteSpace(color) ? null : $"%{color.Trim()}%" },
                    { "@Style", string.IsNullOrWhiteSpace(style) ? null : $"%{style.Trim()}%" }
                };
                var parameters = new DynamicParameters(dictionary);
                dtos = connection.Query<FlooringDTO>(sql, parameters);
            }

            return dtos;
        }

        public IEnumerable<ManufacturerEntity> GetManufacturers()
        {
            var sql = @"SELECT m.id, m.name
                        FROM manufacturers m";
            IEnumerable<ManufacturerEntity> dtos = new List<ManufacturerEntity>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                dtos = connection.Query<ManufacturerEntity>(sql);
            }

            return dtos;
        }

        public IEnumerable<ColorEntity> GetColors()
        {
            var sql = @"SELECT c.id, c.name, c.color_number AS colornumber
                        FROM colors c";
            IEnumerable<ColorEntity> dtos = new List<ColorEntity>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                dtos = connection.Query<ColorEntity>(sql);
            }

            return dtos;
        }

        public IEnumerable<TypeEntity> GetTypes()
        {
            var sql = @"SELECT t.id, t.name
                        FROM types t";
            IEnumerable<TypeEntity> dtos = new List<TypeEntity>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                dtos = connection.Query<TypeEntity>(sql);
            }

            return dtos;
        }

        public IEnumerable<StyleEntity> GetStyles()
        {
            var sql = @"SELECT s.id, s.name, s.style_key AS stylekey
                        FROM styles s";
            IEnumerable<StyleEntity> dtos = new List<StyleEntity>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                dtos = connection.Query<StyleEntity>(sql);
            }

            return dtos;
        }
    }
}
