namespace main
{
    public abstract class AstralObject
    {
        public string candidateId { get; set; }
        public int row { get; set; }
        public int column { get; set; }

        protected AstralObject(int row, int column)
        {
            candidateId = "a5d9cfa9-fa99-4a3b-aa21-512fcbe824a9";
            this.row = row;
            this.column = column;
        }
    }
}
