using System.Collections.ObjectModel;

namespace DiarioBienestar;

public partial class PaginaRegistro : ContentPage
{
	RepositorioRegistros repositorio; 
	ObservableCollection<Registro> registros;

	public PaginaRegistro()
	{
		InitializeComponent();
		repositorio = new RepositorioRegistros(); 
		registros = repositorio.RepositorioRegistro;

        // Asigna el BindingContext de la página para vincular los datos
        BindingContext = this; 
		miCollectionView.ItemsSource = registros;
		MostrarFechasPorOrden(); 
	}
	private void OnSelectionChanged(object sender, EventArgs e)
	{

}

	private async void DeleteBtn(object sender, EventArgs e)
	{
		//obtener registro seleccionado en el CollectionView
		var registroSeleccionado = miCollectionView.SelectedItem as Registro;
		bool respuesta = await DisplayAlert("Atención", "¿Estás seguro de que quieres borrar el registro?", "Sí", "No");
		
		if(respuesta)
		{
			repositorio.DeleteRegistro(registroSeleccionado);
			registros.Remove(registroSeleccionado);
		}
		else if(respuesta == null)
		{
			await DisplayAlert("Aviso", "Por favor, seleccione un registro para eliminar.", "Ok");
		}
	}

	//este metodo nos servira para ordenar los regsitros de la app según la fecha.
	private void MostrarFechasPorOrden()
	{
		var registrosOrdenados = registros.OrderBy(r => r.Fecha).ToList();
		registros.Clear();

		foreach (var registro in registrosOrdenados)
		{
			registros.Add(registro);
		}
	}
}