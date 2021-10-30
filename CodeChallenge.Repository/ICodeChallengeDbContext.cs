using CodeChallenge.Repository.DTOs;
using CodeChallenge.Repository.Entities;

namespace CodeChallenge.Repository
{
    public interface ICodeChallengeDbContext
    {
        public IEnumerable<FlooringDTO> SearchFlooring(string? manufacturer, string? type, string? color, string? style);

        public IEnumerable<TypeEntity> GetTypes();
        public IEnumerable<ColorEntity> GetColors();
        public IEnumerable<StyleEntity> GetStyles();
        public IEnumerable<ManufacturerEntity> GetManufacturers();
    }
}
