namespace Megaverse
{
    public abstract class AstralObject
    {
        public int Row { get; set; }
        public int Column { get; set; }

        protected AstralObject(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
