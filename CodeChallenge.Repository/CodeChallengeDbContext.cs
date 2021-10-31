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

        public IEnumerable<FlooringDTO> SearchFlooring(string? manufacturer, int? type, string? color, string? style, int? size)
        {
            var sql = @"SELECT
                            f.id,
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
                            AND (@Type IS NULL OR t.id = @Type)
                            AND (@Color IS NULL OR c.name LIKE @Color)
                            AND (@Style IS NULL OR s.name LIKE @Style)
                            AND (@Size IS NULL 
                                OR f.size LIKE @Width 
                                OR f.size LIKE @Depth 
                                OR f.size LIKE @Size)";
            IEnumerable<FlooringDTO> dtos = new List<FlooringDTO>();

            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();
                var dictionary = new Dictionary<string, object>
                {
                    { "@Manufacturer", string.IsNullOrWhiteSpace(manufacturer) ? null : $"%{manufacturer.Trim()}%" },
                    { "@Type", type },
                    { "@Color", string.IsNullOrWhiteSpace(color) ? null : $"%{color.Trim()}%" },
                    { "@Style", string.IsNullOrWhiteSpace(style) ? null : $"%{style.Trim()}%" },
                    { "@Size", size == null ? null : $"{size}'%"},
                    { "@Width", size == null ? null : $"{size} x %"},
                    { "@Depth", size == null ? null : $"%x {size}"}
                };
                var parameters = new DynamicParameters(dictionary);
                dtos = connection.Query<FlooringDTO>(sql, parameters);
            }

            return dtos;
        }

        public FlooringDTO SaveFlooring(EditableFlooringDTO editableFlooringDTO)
        {
            if(editableFlooringDTO.Id == null)
            {
                return this.InsertFlooring(editableFlooringDTO);
            }
            else
            {
                return this.UpdateFlooring(editableFlooringDTO);
            }
        }

        private FlooringDTO InsertFlooring(EditableFlooringDTO editableFlooringDTO)
        {
            using (var connection = new SqlConnection(this._connString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                if (!editableFlooringDTO.ManufacturerId.HasValue)
                {
                    editableFlooringDTO.ManufacturerId = this.InsertManufacturer(editableFlooringDTO.Manufacturer, connection, transaction);
                }
                if (!editableFlooringDTO.TypeId.HasValue)
                {
                    editableFlooringDTO.TypeId = this.InsertType(editableFlooringDTO.Type, connection, transaction);
                }
                if (!editableFlooringDTO.StyleId.HasValue)
                {
                    editableFlooringDTO.StyleId = this.InsertStyle(editableFlooringDTO.Style, connection, transaction);
                }
                if (!editableFlooringDTO.ColorId.HasValue)
                {
                    editableFlooringDTO.ColorId = this.InsertColor(editableFlooringDTO.Color, connection, transaction);
                }

                var existingRecordSql = 
                    @"SELECT 
                        f.id
                    FROM flooring f
                    INNER JOIN manufacturers m ON f.manufacturer_id = m.id
                    INNER JOIN styles s ON f.style_id = s.id
                    INNER JOIN types t ON f.type_id = t.id
                    INNER JOIN colors c ON f.color_id = c.id
                    WHERE m.id = @ManufacturerId 
                        AND t.id = @TypeId
                        AND s.id = @StyleId
                        AND c.id = @ColorId
                        AND f.size = @Size";
                var dictionary = new Dictionary<string, object>
                {
                    { "@ManufacturerId", editableFlooringDTO.ManufacturerId },
                    { "@TypeId", editableFlooringDTO.TypeId },
                    { "@StyleId", editableFlooringDTO.StyleId },
                    { "@ColorId", editableFlooringDTO.ColorId },
                    { "@Size", editableFlooringDTO.Size }
                };
                var existingRecordId = connection.ExecuteScalar<int?>(existingRecordSql, dictionary, transaction);

                if (existingRecordId.HasValue)
                {
                    throw new Exception("Duplicate record detected.  Verify values against existing records and resubmit.");
                }

                var sql = @"INSERT INTO flooring(manufacturer_id, style_id, color_id, type_id, size) VALUES 
                                (@ManufacturerId, @StyleId, @ColorId, @TypeId, @Size);
                            SELECT 
                                f.id,
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
                            WHERE m.id = @ManufacturerId 
                                AND t.id = @TypeId
                                AND s.id = @StyleId
                                AND c.id = @ColorId
                                AND f.size = @Size";
                var parameters = new DynamicParameters(dictionary);
                var result = connection.QueryFirst<FlooringDTO>(sql, parameters, transaction);

                transaction.Commit();

                return result;
            }
        }

        private FlooringDTO UpdateFlooring(EditableFlooringDTO editableFlooringDTO)
        {
            throw new NotImplementedException();
        }

        private int InsertManufacturer(ManufacturerEntity manufacturer, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"INSERT INTO manufacturers(name) VALUES (@ManufacturerName); SELECT id from manufacturers WHERE name = @ManufacturerName";
            var dictionary = new Dictionary<string, object>
            {
                { "@ManufacturerName", manufacturer.Name.Trim() }
            };
            var parameters = new DynamicParameters(dictionary);
            var result = connection.ExecuteScalar<int>(sql, parameters, transaction);
            return result;
        }

        private int InsertType(TypeEntity typeEntity, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"INSERT INTO types(name) VALUES (@TypeName); SELECT id from types WHERE name = @TypeName";
            var dictionary = new Dictionary<string, object>
            {
                { "@TypeName", typeEntity.Name.Trim() }
            };
            var parameters = new DynamicParameters(dictionary);
            var result = connection.ExecuteScalar<int>(sql, parameters, transaction);
            return result;
        }

        private int InsertColor(ColorEntity color, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"INSERT INTO colors(name, color_number) VALUES (@ColorName, @ColorNumber); SELECT id from colors WHERE name = @ColorName AND color_number = @ColorNumber";
            var dictionary = new Dictionary<string, object>
            {
                { "@ColorName", color.Name.Trim() },
                { "@ColorNumber", color.ColorNumber.Trim() }
            };
            var parameters = new DynamicParameters(dictionary);
            var result = connection.ExecuteScalar<int>(sql, parameters, transaction);
            return result;
        }

        private int InsertStyle(StyleEntity style, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"INSERT INTO styles(name, style_key) VALUES (@StyleName, @StyleKey); SELECT id from styles WHERE name = @StyleName AND style_key = @StyleKey";
            var dictionary = new Dictionary<string, object>
            {
                { "@StyleName", style.Name.Trim() },
                { "@StyleKey", style.StyleKey.Trim() }
            };
            var parameters = new DynamicParameters(dictionary);
            var result = connection.ExecuteScalar<int>(sql, parameters, transaction);
            return result;
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
