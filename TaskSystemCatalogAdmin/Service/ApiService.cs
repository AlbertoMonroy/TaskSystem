using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaskSystemCatalogAdmin.Models;

public class ApiService
{
    public static string Token { get; private set; }

    private static readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"])
    };

    #region Users
    public static async Task<bool> CreateUserAsync(UserModel user)
    {
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("users", content);
        return response.IsSuccessStatusCode;
    }
    public static async Task<List<UserModel>> GetUsersAsync()
    {
        var response = await _client.GetAsync("users");
        if (!response.IsSuccessStatusCode) return new List<UserModel>();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<UserModel>>(json);
    }
    public static async Task<bool> UpdateUserAsync(UserModel user)
    {
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"users/{user.Id}", content);
        return response.IsSuccessStatusCode;
    }
    public static async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _client.DeleteAsync($"users/{id}");
        return response.IsSuccessStatusCode;
    }

    #endregion

    #region Priorities
    public static async Task<bool> CreatePriorityAsync(PriorityModel priority)
    {
        var json = JsonConvert.SerializeObject(priority);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("priorities", content);
        return response.IsSuccessStatusCode;
    }

    public static async Task<List<PriorityModel>> GetPrioritiesAsync()
    {
        var response = await _client.GetAsync("priorities");
        if (!response.IsSuccessStatusCode) return new List<PriorityModel>();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<PriorityModel>>(json);
    }

    public static async Task<bool> UpdatePriorityAsync(PriorityModel priority)
    {
        var json = JsonConvert.SerializeObject(priority);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"priorities/{priority.Id}", content);
        return response.IsSuccessStatusCode;
    }

    public static async Task<bool> DeletePriorityAsync(int id)
    {
        var response = await _client.DeleteAsync($"priorities/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion
}
