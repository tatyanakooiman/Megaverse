using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace main
{
    public class MegaverseService
    {
        private readonly HttpClient _httpClient;

        public MegaverseService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://challenge.crossmint.io/api/")
            };
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", candidateId);
        }

        public async Task AddPolyanet(int row, int column)
        {
            var polyanet = new Polyanet(row, column);
            var json = JsonConvert.SerializeObject(polyanet);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("polyanets", content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Polyanet added at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding Polyanet at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }

        public async Task DeletePolyanet(int row, int column)
        {
            var uri = $"polyanets?row={row}&column={column}";
            
            try
            {
                var response = await _httpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Polyanet deleted at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting Polyanet at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }

        public async Task AddSoloon(int row, int column, string color)
        {
            if (color != "blue" && color != "red" && color != "purple" && color != "white")
            {
                throw new ArgumentException("Color must be one of: blue, red, purple, white.");
            }

            var soloon = new Soloon(row, column, color);
            var json = JsonConvert.SerializeObject(soloon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("soloons", content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Soloon added at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding Soloon at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }

        public async Task DeleteSoloon(int row, int column)
        {
            var uri = $"soloons?row={row}&column={column}";
            
            try
            {
                var response = await _httpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Soloon deleted at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting Soloon at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }

        public async Task AddCometh(int row, int column, string direction)
        {
            if (direction != "up" && direction != "down" && direction != "left" && direction != "right")
            {
                throw new ArgumentException("Direction must be one of: up, down, left, right.");
            }

            var cometh = new Cometh(row, column, direction);
            var json = JsonConvert.SerializeObject(cometh);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("comeths", content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Cometh added at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding Cometh at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }

        public async Task DeleteCometh(int row, int column)
        {
            var uri = $"comeths?row={row}&column={column}";
            
            try
            {
                var response = await _httpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Cometh deleted at Row: {row}, Column: {column}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting Cometh at Row: {row}, Column: {column}. Details: {ex.Message}");
            }
        }
    }
}
