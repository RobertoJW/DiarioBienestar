namespace DiarioBienestar;

public partial class PaginaEstadistica : ContentPage
{
	Registro registro;
	RepositorioRegistros repositorio; 
	public PaginaEstadistica()
	{
		InitializeComponent();
		repositorio = new RepositorioRegistros();
		ActualizarEstadisticasUltimaSemana(); 
	}

	public void ActualizarEstadisticasUltimaSemana()
	{
		// obtenemos registros de la ultima semana
		var UltimaSemana = repositorio.RepositorioRegistro.
			Where(r => r.Fecha >= DateOnly.FromDateTime(DateTime.Now.AddDays(-7))).ToList();

		// En caso de que haya registros
		if (UltimaSemana.Any())
		{
			double promedioActividadFisica = UltimaSemana.Average(r => r.Intensidad);
			double promedioEnergiaSemanal = UltimaSemana.Average(r => r.NivelEnergia);

			/* Primeramente se divide entre 2 para calcular la media entre las dos sumas
			 * luego se divide entre 10 para sacar una nota entre el 0 al 10
			 */
			double progresoSemanal = (promedioActividadFisica + promedioEnergiaSemanal) / 2 / 10;

			PromedioActividadFisicaSemanal.Text = $"Promedio de actividad física: {promedioActividadFisica:F2}";
			PromedioEnergia.Text = $"Promedio de energía semanal: {promedioEnergiaSemanal:F2}";
			ProgressEstadistica.Progress = progresoSemanal;

			if (progresoSemanal < 4)
			{
				ProgressEstadistica.ProgressColor = Colors.Red; 
			}
			else if (progresoSemanal <= 7)
			{
				ProgressEstadistica.ProgressColor = Colors.Yellow;
			}
			else
			{
				ProgressEstadistica.ProgressColor = Colors.Green; 
			}
		}
		else
		{
            PromedioActividadFisicaSemanal.Text = "No hay actividad registrada en la última semana.";
            PromedioEnergia.Text = string.Empty;
            ProgressEstadistica.Progress = 0;
        }
	}
}