namespace Megaverse
{
    public class Cometh : AstralObject
    {
        public string Direction { get; set; }

        public Cometh(int row, int column, string direction) : base(row, column)
        {
            Direction = direction;
        }
    }
}
