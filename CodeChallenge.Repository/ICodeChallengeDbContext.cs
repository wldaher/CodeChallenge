using CodeChallenge.Repository.DTOs;
using CodeChallenge.Repository.Entities;

namespace CodeChallenge.Repository
{
    public interface ICodeChallengeDbContext
    {
        public IEnumerable<FlooringDTO> SearchFlooring(string? manufacturer, int? type, string? color, string? style, int? size);
        public FlooringDTO SaveFlooring(EditableFlooringDTO editableFlooringDTO);
        public int DeleteFlooring(int flooringId);
        public IEnumerable<TypeEntity> GetTypes();
        public IEnumerable<ColorEntity> GetColors();
        public IEnumerable<StyleEntity> GetStyles();
        public IEnumerable<ManufacturerEntity> GetManufacturers();
    }
}
