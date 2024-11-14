namespace main
{
    public abstract class AstralObject
    {
        public string candidateId { get; set; }
        public int row { get; set; }
        public int column { get; set; }

        protected AstralObject(string candidateId)
        {
            this.candidateId = candidateId;
            row = 0;
            column = 0;
        }
        
        protected AstralObject(string candidateId, int row, int column)
        {
            this.candidateId = candidateId;
            this.row = row;
            this.column = column;
        }
    }
}
