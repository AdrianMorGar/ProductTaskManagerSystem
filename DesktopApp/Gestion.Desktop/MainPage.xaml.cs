using Gestion.Desktop.Models;
using Gestion.Desktop.Services;
using System.Globalization; // Para leer decimales correctamente

namespace Gestion.Desktop;

public partial class MainPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public MainPage()
    {
        InitializeComponent();
    }

    // Se ejecuta al arrancar o al volver de otra pagina
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarProductos();
    }

    private async Task CargarProductos()
    {
        var productos = await _apiService.GetProductosAsync();
        ListaProductos.ItemsSource = productos;
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        BtnRefrescar.Text = "Cargando...";
        await CargarProductos();
        BtnRefrescar.Text = "Refrescar Lista";
    }

    // CREAR PRODUCTO
    private async void OnCrearProductoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntNombre.Text))
        {
            await DisplayAlert("Error", "Pon un nombre", "OK");
            return;
        }

        // Parseo seguro de numeros
        decimal.TryParse(EntPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio);
        int.TryParse(EntStock.Text, out int stock);

        var nuevo = new Producto
        {
            Nombre = EntNombre.Text,
            Precio = precio,
            Stock = stock,
            Categoria = EntCategoria.Text
        };

        if (await _apiService.CrearProductoAsync(nuevo))
        {
            await DisplayAlert("Éxito", "Producto creado", "OK");
            EntNombre.Text = ""; EntPrecio.Text = ""; EntStock.Text = ""; EntCategoria.Text = ""; // Limpiamos
            await CargarProductos(); // Recargar lista
        }
        else
        {
            await DisplayAlert("Error", "Fallo al crear", "OK");
        }
    }

    // EDITAR PRODUCTO
    private async void OnEditarProductoClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var producto = (Producto)btn.BindingContext;

        // Navegamos a la pagina de edicion pasando el producto
        await Navigation.PushAsync(new EditarProductoPage(producto));
    }

    // BORRAR PRODUCTO
    private async void OnEliminarProductoClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var producto = (Producto)btn.BindingContext;

        bool confirmar = await DisplayAlert("Borrar", $"¿Eliminar {producto.Nombre}?", "Sí", "No");
        if (!confirmar) return;

        if (await _apiService.EliminarProductoAsync(producto.Id))
        {
            await CargarProductos(); // Recargar lista
        }
        else
        {
            await DisplayAlert("Error", "No se pudo borrar", "OK");
        }
    }

    // NAVEGAR A DETALLES
    private async void OnProductoSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Producto producto)
        {
            await Navigation.PushAsync(new DetalleProductoPage(producto));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}