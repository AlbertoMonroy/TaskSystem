using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Client.Models;

public class ApiService
{
    private static readonly Lazy<ApiService> _instance = new Lazy<ApiService>(() => new ApiService());
    public static ApiService Instance => _instance.Value;

    private readonly HttpClient _client;
    private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
    public string Token { get; private set; }

    private ApiService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl)
        };

        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task LoadUsersAsync()
    {
        var users = await _client.GetFromJsonAsync<List<UserModel>>("api/users");
    }

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        var response = await _client.GetAsync($"users/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserModel>(json);
    }

    public async Task<UserModel> LoginAsync(string username, string password)
    {
        var body = new { Username = username, Password = password };
        var response = await _client.PostAsync("users/login", ToJson(body));

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginResult>(json);

        Token = result.Token;

        // Agregar encabezado Authorization para futuras llamadas
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Token);

        return result.User;
    }

    public void ClearSession()
    {
        Token = null;
        _client.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        var response = await _client.GetAsync("tasks");
        if (!response.IsSuccessStatusCode)
        {
            return new List<TaskModel>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<TaskModel>>(json);
    }

    public async Task<TaskModel> CreateTaskAsync(TaskModel task)
    {
        var response = await _client.PostAsync("tasks", ToJson(task));
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TaskModel>(json);
    }

    public async Task UpdateTaskAsync(TaskModel task)
    {
        await _client.PutAsync($"tasks/{task.Id}", ToJson(task));
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _client.DeleteAsync($"tasks/{id}");
    }

    public async Task<List<PriorityModel>> GetPrioritiesAsync()
    {
        var response = await _client.GetAsync("priorities");
        if (!response.IsSuccessStatusCode)
        {
            return new List<PriorityModel>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<PriorityModel>>(json);
    }

    private StringContent ToJson(object data)
    {
        var json = JsonConvert.SerializeObject(data);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private class LoginResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }
    }

}