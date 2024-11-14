using System;
using System.Linq;

namespace main
{
    public class Cometh : AstralObject
    {
        public string direction { get; set; }
        private readonly string[] validDirections = { "up", "down", "left", "right" };

        public Cometh(string candidateId, string direction) : base(candidateId)
        {
            AddDirection(direction);
        }
        
        public Cometh(string candidateId, int row, int column, string direction) : base(candidateId, row, column)
        {
            AddDirection(direction);
        }

        private void AddDirection(string direction)
        {
            if (!validDirections.Contains(direction))
            {
                throw new ArgumentException("Invalid cometh direction specified.");
            }
            
            this.direction = direction;
        }
    }
}
