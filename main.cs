using System.Threading.Tasks;
using Megaverse;

public class Program
{
    public static async Task Main(string[] args)
    {
      string candidateId = "a5d9cfa9-fa99-4a3b-aa21-512fcbe824a9";
      var service = new MegaverseService(candidateId);      
      
      /* row and column values needed to form an X-shape on the map:
          (2, 2)
          (2, 8)
          (3, 3)
          (3, 7)
          (4, 4)
          (4, 6)
          (5, 5)
          (6, 4)
          (6, 6)
          (7, 3)
          (7, 7)
          (8, 2)
          (8, 8) */      
      
      for (int i=2, j=8; i<9; i++, j--)
      {
        await service.AddPolyanet(i, i);

        if (i != j)
          await service.AddPolyanet(i, j);
      }
    }
}