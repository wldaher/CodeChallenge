using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge.Repository.DTOs
{
    public class FlooringDTO
    {
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string StyleKey { get; set; }
        public string Color { get; set; }
        public string ColorNumber { get; set; }
        public string Size { get; set; }
        public FlooringDTO()
        {
            this.Manufacturer = string.Empty;
            this.Type = string.Empty;
            this.Style = string.Empty;
            this.StyleKey = string.Empty;
            this.Color = string.Empty;
            this.ColorNumber = string.Empty;
            this.Size = string.Empty;
        }
    }
}
