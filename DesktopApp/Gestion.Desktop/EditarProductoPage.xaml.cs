using Gestion.Desktop.Models;
using Gestion.Desktop.Services;
using System.Globalization;

namespace Gestion.Desktop;

public partial class EditarProductoPage : ContentPage
{
    private Producto _producto;
    private ApiService _apiService = new ApiService();

    public EditarProductoPage(Producto producto)
    {
        InitializeComponent();
        _producto = producto;

        EntNombre.Text = _producto.Nombre;
        EntPrecio.Text = _producto.Precio.ToString(CultureInfo.InvariantCulture);
        EntStock.Text = _producto.Stock.ToString();
        EntCategoria.Text = _producto.Categoria;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        _producto.Nombre = EntNombre.Text;
        _producto.Categoria = EntCategoria.Text;

        if (decimal.TryParse(EntPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            _producto.Precio = precio;

        if (int.TryParse(EntStock.Text, out int stock))
            _producto.Stock = stock;

        if (await _apiService.ActualizarProductoAsync(_producto))
        {
            await DisplayAlert("Éxito", "Producto actualizado", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}