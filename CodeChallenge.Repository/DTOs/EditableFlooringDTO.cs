using CodeChallenge.Repository.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge.Repository.DTOs
{
    public class EditableFlooringDTO
    {
        public int? Id { get; set; }
        public string Size { get; set; }
        public int? ManufacturerId { get; set; }
        public int? TypeId { get; set; }
        public int? ColorId { get; set; }
        public int? StyleId { get; set; }
        public ManufacturerEntity Manufacturer { get; set; }
        public TypeEntity Type { get; set; }
        public StyleEntity Style { get; set; }
        public ColorEntity Color { get; set; }
    }
}
