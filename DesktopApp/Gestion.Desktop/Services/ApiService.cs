using System.Net.Http.Json;
using Gestion.Desktop.Models;

namespace Gestion.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7249/";

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                // Llama exactamente a la ruta que probaste en el navegador
                return await _httpClient.GetFromJsonAsync<List<Producto>>("api/productosapi")
                       ?? new List<Producto>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return new List<Producto>();
            }
        }

        // Crear un Producto nuevo
        public async Task<bool> CrearProductoAsync(Producto producto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/productosapi", producto);
            return response.IsSuccessStatusCode;
        }

        // Actualizar un Producto existente
        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/productosapi/{producto.Id}", producto);
            return response.IsSuccessStatusCode;
        }

        // Eliminar un Producto
        public async Task<bool> EliminarProductoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/productosapi/{id}");
            return response.IsSuccessStatusCode;
        }

        // Añadir una Tarea nueva
        public async Task<bool> CrearTareaAsync(Tarea nuevaTarea)
        {
            var response = await _httpClient.PostAsJsonAsync("api/productosapi/tarea", nuevaTarea);
            return response.IsSuccessStatusCode;
        }

        // Marcar como completada (actualizandola)
        public async Task<bool> ActualizarTareaAsync(Tarea tarea)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/productosapi/tarea/{tarea.Id}", tarea);
            return response.IsSuccessStatusCode;
        }

        // Eliminar tarea
        public async Task<bool> EliminarTareaAsync(int tareaId)
        {
            var response = await _httpClient.DeleteAsync($"api/productosapi/tarea/{tareaId}");
            return response.IsSuccessStatusCode;
        }
    }
}