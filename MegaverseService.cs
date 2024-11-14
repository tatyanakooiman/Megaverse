using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace main
{
    public class MegaverseService
    {
        private readonly HttpClient _httpClient;
        private const int MaxRetries = 5;
        private const int InitialDelay = 500;
        private readonly string _candidateId;
        private readonly Dictionary<string, AstralObject> _astralObjects;

        public MegaverseService(string candidateId)
        {
            _candidateId = candidateId;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://challenge.crossmint.io/api/")
            };

            _astralObjects = new Dictionary<string, AstralObject>
            {
                {"POLYANET", new Polyanet(candidateId)},
                {"BLUE_SOLOON", new Soloon(candidateId, "blue")},
                {"RED_SOLOON", new Soloon(candidateId, "red")},
                {"PURPLE_SOLOON", new Soloon(candidateId, "purple")},
                {"WHITE_SOLOON", new Soloon(candidateId, "white")},
                {"UP_COMETH", new Cometh(candidateId, "up")},
                {"DOWN_COMETH", new Cometh(candidateId, "down")},
                {"LEFT_COMETH", new Cometh(candidateId, "left")},
                {"RIGHT_COMETH", new Cometh(candidateId, "right")}
            };
        }

        public async Task PopulateMegaverse()
        {
            try
            {
                // Get the Megaverse goal map
                var response = await _httpClient.GetAsync($"map/{_candidateId}/goal");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(responseBody);
                    
                    if (jsonResponse == null)
                    {
                        Console.WriteLine("The response body was null or empty or invalid JSON.");
                        return;
                    }
                    
                    var goal = jsonResponse["goal"];
    
                    // Follow the map coordinates to add the astral objects
                    if (goal != null)
                    {
                        for (var i=0; i<goal.Count; i++)
                        {
                            var row = goal[i];
                            for (var j=0; j<row.Count; j++)
                            {
                                if (!_astralObjects.ContainsKey(row[j])) continue;
                                if (!_astralObjects.TryGetValue(row[j], out var astralObject)) continue;
                                
                                astralObject.row = i;
                                astralObject.column = j;
                                await AddAstralObject(astralObject);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error getting the goal map: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
        
        private async Task AddAstralObject(AstralObject astralObject)
        {
            var json = JsonConvert.SerializeObject(astralObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var type = astralObject.GetType().Name;
            var retries = 0;
            var delay = InitialDelay;

            try
            {
                while (retries < MaxRetries)
                {
                    var response = await _httpClient.PostAsync($"{type.ToLower()}s", content);
                
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{type} added at row: {astralObject.row}, column: {astralObject.column}.");
                        break;
                    }
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        // 429 error - check if there is a Retry-After header
                        var retryAfter = response.Headers.RetryAfter?.Delta;
                        if (retryAfter != null)
                        {
                            Console.WriteLine($"Rate limited - retrying after {retryAfter.Value.TotalSeconds} seconds.");
                            await Task.Delay(retryAfter.Value);
                        }
                        else
                        {
                            // No Retry-After header - use exponential backoff
                            Console.WriteLine($"Rate limited - retrying in {delay / 1000} seconds.");
                            await Task.Delay(delay);
                            delay *= 2;
                        }
                    }
                    else
                    {
                        // Not a 429 error
                        Console.WriteLine($"Error adding {type} at row: {astralObject.row}, column: {astralObject.column}. " +
                                          $"Details: {response.StatusCode}, {response.ReasonPhrase}");
                        break;
                    }
                
                    retries++;
                }

                if (retries == MaxRetries)
                {
                    // All the retries are exhausted
                    Console.WriteLine($"Error adding {type} at row: {astralObject.row}, column: {astralObject.column}. " +
                                      $"Max server retries exceeded");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
        
        private async Task DeleteAstralObject(AstralObject astralObject)
        {
            var json = JsonConvert.SerializeObject(astralObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var type = astralObject.GetType().Name;
            var retries = 0;
            var delay = InitialDelay;
            
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_httpClient.BaseAddress}{type.ToLower()}s")
            {
                Content = content
            };

            try
            {
                while (retries < MaxRetries)
                {
                    var response = await _httpClient.SendAsync(request);
                
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{type} deleted at row: {astralObject.row}, column: {astralObject.column}.");
                        break;
                    }
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        // 429 error - check if there is a Retry-After header
                        var retryAfter = response.Headers.RetryAfter?.Delta;
                        if (retryAfter != null)
                        {
                            Console.WriteLine($"Rate limited - retrying after {retryAfter.Value.TotalSeconds} seconds.");
                            await Task.Delay(retryAfter.Value);
                        }
                        else
                        {
                            // No Retry-After header - use exponential backoff
                            Console.WriteLine($"Rate limited - retrying in {delay / 1000} seconds.");
                            await Task.Delay(delay);
                            delay *= 2;
                        }
                    }
                    else
                    {
                        // Not a 429 error
                        Console.WriteLine($"Error deleting {type} at row: {astralObject.row}, column: {astralObject.column}. " +
                                          $"Details: {response.StatusCode}, {response.ReasonPhrase}");
                        break;
                    }
                
                    retries++;
                }
                
                if (retries == MaxRetries)
                {
                    // All the retries are exhausted
                    Console.WriteLine($"Error deleting {type} at row: {astralObject.row}, column: {astralObject.column}. " +
                                      $"Max server retries exceeded");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
