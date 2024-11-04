namespace Megaverse
{
    public class Soloon : AstralObject
    {
        public string Color { get; set; }

        public Soloon(int row, int column, string color) : base(row, column)
        {
            Color = color;
        }
    }
}
