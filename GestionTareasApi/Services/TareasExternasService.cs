using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestionTareasApi.Models;

namespace GestionTareasApi.Services
{
    public class TareasExternasService
    {
        private readonly HttpClient _httpClient;

        public TareasExternasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<JsonPlaceholderTodo>> GetTodosAsync()
        {
            var response = await _httpClient.GetAsync("todos");
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<IEnumerable<JsonPlaceholderTodo>>()) ?? Array.Empty<JsonPlaceholderTodo>();
        }

        public async Task<JsonPlaceholderTodo?> GetTodoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"todos/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JsonPlaceholderTodo>();
        }
    }
}
