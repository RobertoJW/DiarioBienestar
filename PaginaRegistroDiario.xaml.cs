using Microsoft.Maui.Controls; // Para controles de interfaz
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Dispatching; // Para MainThread
using System.Diagnostics; // Para colores

namespace DiarioBienestar;

public partial class PaginaRegistroDiario : ContentPage
{
    Registro registro;
    public RepositorioRegistros repositorio;

    public PaginaRegistroDiario()
    {
        InitializeComponent();
        registro = new Registro(DateOnly.MinValue, "", 0, 0, 0);
        repositorio = new RepositorioRegistros();
    }

    private async void SliderChanged(object sender, ValueChangedEventArgs e)
    {
        double valorSlider = e.NewValue;
        progressBar.Progress = valorSlider / 10;
        etiquetaSlider.Text = $"Esfuerzo físico: {valorSlider:F0}";

        await progressBar.ProgressTo(valorSlider / 10, 250, Easing.Linear);

        if (valorSlider <= 3)
        {
            progressBar.ProgressColor = Colors.Red;  // Rojo
        }
        else if (valorSlider <= 7)
        {
            progressBar.ProgressColor = Colors.Orange;  // Naranja
        }
        else
        {
            progressBar.ProgressColor = Colors.Green;  // Verde
        }
    }

    private async void AddRegistroBtn(object sender, EventArgs e)
    {
        var registro = new Registro(DateOnly.FromDateTime(fechaRegistro.Date), editorTexto.Text, slider.Value,
            progressBar.Progress, nivelEnergia.Value);

        if (string.IsNullOrEmpty(editorTexto.Text))
        {
            await DisplayAlert("Aviso", "Rellene el comentario.", "Ok");
        }
        else if (repositorio.EsFechaRepetida(fechaRegistro))
        {
            bool respuesta = await DisplayAlert("Fecha Repetida", "Ya existe un registro con esta fecha. ¿Desea reemplazarlo?", "Sí", "No");

            if(respuesta)
            {
                ReemplazarFecha(fechaRegistro);
            }
        }
        else
        {
            repositorio.AddRegistro(registro);
            await DisplayAlert("Añadido", "Se ha añadido el registro.", "Ok");

            editorTexto.Text = "";
            progressBar.Progress = 0;
            slider.Value = 0;
            fechaRegistro.Date = DateTime.Now;
            nivelEnergia.Value = 1;
        }
    }

    private void CleanRegistroBtn(object sender, EventArgs e)
    {
        editorTexto.Text = "";
        progressBar.Progress = 0;
        slider.Value = 0;
        fechaRegistro.Date = DateTime.Now;
        nivelEnergia.Value = 1;
    }

    private void StepperValue(object sender, ValueChangedEventArgs e)
    {
        try
        {
            if (lblnivelEnergia == null)
            {
                Debug.WriteLine("lblnivelEnergia es nulo en StepperValue");
            }
            else
            {
                lblnivelEnergia.Text = $"{e.NewValue:F0}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en StepperValue: " + ex.ToString());
        }
    }

    private void ReemplazarFecha(DatePicker fechaEscogida)
    {
        DateOnly fechaEscogidaOnly = DateOnly.FromDateTime(fechaEscogida.Date);

        for (int i = 0; i < repositorio.RepositorioRegistro.Count; i++)
        {
            if (repositorio.RepositorioRegistro[i].Fecha == fechaEscogidaOnly)
            {
                var actualizarRegistro = new Registro(
                    fechaEscogidaOnly,
                    editorTexto.Text,
                    slider.Value,
                    progressBar.Progress,
                    nivelEnergia.Value);

                //reemplazar el registro en el repositorio
                repositorio.RepositorioRegistro[i] = actualizarRegistro;
                repositorio.GuardarEnJSON();
                return; 
            }
        }
    }
}
