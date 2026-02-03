using System.Collections.ObjectModel;
using Gestion.Desktop.Models;
using Gestion.Desktop.Services;

namespace Gestion.Desktop;

public partial class DetalleProductoPage : ContentPage
{
    private Producto _producto;
    private readonly ApiService _apiService = new ApiService();
    public ObservableCollection<Tarea> TareasVisibles { get; set; }

    public DetalleProductoPage(Producto producto)
    {
        InitializeComponent();
        _producto = producto;
        LblProducto.Text = $"Tareas de: {_producto.Nombre}";

        // Inicializamos la coleccion
        TareasVisibles = new ObservableCollection<Tarea>();
        ListaTareas.ItemsSource = TareasVisibles;
    }

    // Se ejecuta al mostrar la pagina
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarTareas();
    }

    // Metodo para refrescar la lista desde el servidor
    private async Task CargarTareas()
    {
        var productos = await _apiService.GetProductosAsync();
        var productoActualizado = productos.FirstOrDefault(p => p.Id == _producto.Id);

        if (productoActualizado != null)
        {
            // Actualizamos la lista visual limpiando y añadiendo de nuevo
            TareasVisibles.Clear();
            if (productoActualizado.Tareas != null)
            {
                foreach (var tarea in productoActualizado.Tareas)
                {
                    TareasVisibles.Add(tarea);
                }
            }
        }
    }

    // AÑADIR TAREA
    private async void OnAddTareaClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntNuevaTarea.Text)) return;

        var nueva = new Tarea { Descripcion = EntNuevaTarea.Text, ProductoId = _producto.Id, EstaCompletada = false };

        if (await _apiService.CrearTareaAsync(nueva))
        {
            EntNuevaTarea.Text = string.Empty;
            await CargarTareas(); // Refrescamos la lista completa
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear la tarea", "OK");
        }
    }

    // EDITAR TAREA (ventana emergente)
    private async void OnEditTareaClicked(object sender, EventArgs e)
    {
        var tarea = (Tarea)((Button)sender).BindingContext;

        string nuevoNombre = await DisplayPromptAsync("Editar Tarea", "Cambia la descripción:", "Guardar", "Cancelar", tarea.Descripcion);

        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            tarea.Descripcion = nuevoNombre;

            if (await _apiService.ActualizarTareaAsync(tarea))
            {
                await CargarTareas(); // Refrescamos la lista completa
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar", "OK");
            }
        }
    }

    private async void OnTareaCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // 1. Obtenemos la tarea desde el contexto del CheckBox
        if (sender is CheckBox cb && cb.BindingContext is Tarea tarea)
        {
            // 2. Forzamos que el objeto tenga el valor del CheckBox
            tarea.EstaCompletada = e.Value;

            // 3. Llamamos a la API directamente
            bool exito = await _apiService.ActualizarTareaAsync(tarea);

            if (!exito)
            {
                // Si falla la conexión o la BD, avisamos para saber qué pasa
                await DisplayAlert("Error de Almacén", "No se pudo guardar en la base de datos", "OK");
            }
        }
    }

    // BORRAR TAREA
    private async void OnDeleteTareaClicked(object sender, EventArgs e)
    {
        var tarea = (Tarea)((Button)sender).BindingContext;

        bool confirmar = await DisplayAlert("Borrar", "¿Eliminar esta tarea?", "Sí", "No");
        if (!confirmar) return;

        if (await _apiService.EliminarTareaAsync(tarea.Id))
        {
            await CargarTareas(); // Refrescamos la lista completa
        }
        else
        {
            await DisplayAlert("Error", "No se pudo borrar", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}