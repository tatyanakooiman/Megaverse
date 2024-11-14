using System.Threading.Tasks;
using main;

public class Program
{
    public static async Task Main(string[] args)
    {
        var candidateId = "a5d9cfa9-fa99-4a3b-aa21-512fcbe824a9";
        var service = new MegaverseService(candidateId);
        
        await service.PopulateMegaverse();
    }
}
