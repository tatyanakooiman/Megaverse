using System;
using System.Linq;

namespace main
{
    public class Soloon : AstralObject
    {
        public string color { get; set; }
        private readonly string[] validColors = { "blue", "red", "purple", "white" };

        public Soloon(string candidateId, string color) : base(candidateId)
        {
            AddColor(color);
        }
        
        public Soloon(string candidateId, int row, int column, string color) : base(candidateId, row, column)
        {
            AddColor(color);
        }

        private void AddColor(string color)
        {
            if (!validColors.Contains(color))
            {
                throw new ArgumentException("Invalid soloon color specified.");
            }
            
            this.color = color;
        }
    }
}
