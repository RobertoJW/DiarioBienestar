using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiarioBienestar
{
    public class RepositorioRegistros
    {
        string? directorioBase; 

        private readonly string rutaArchivo =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "registro_usuario.json");
        public ObservableCollection<Registro> RepositorioRegistro { get; } = new ObservableCollection<Registro>();
        
        public RepositorioRegistros()
        {
            directorioBase = AppDomain.CurrentDomain.BaseDirectory;
            CargarRegistrosDesdeJson();
        }

        public void AddRegistro(Registro registro)
        {
            RepositorioRegistro.Add(registro);
            GuardarEnJSON(); 
        }

        public void DeleteRegistro(Registro registro)
        {
            RepositorioRegistro.Remove(registro);
            GuardarEnJSON(); 
        }

        public void BorrarTodosLosRegistros()
        {
            try
            {
                RepositorioRegistro.Clear();
                GuardarEnJSON();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error al borrar los registros.");
            }
        }
        public bool EsFechaRepetida(DatePicker fechaEscogida)
        {
            foreach (var registro in RepositorioRegistro)
            {
                DateOnly fechaEscogidaOnly = DateOnly.FromDateTime(fechaEscogida.Date);
                if (registro.Fecha == fechaEscogidaOnly)
                {
                    return true; 
                }
            }
            return false; 
        }

        public void GuardarEnJSON()
        {
            if (!File.Exists(rutaArchivo))
            {
                using (var fileStream = File.Create(rutaArchivo)) {}
            }
            string json = JsonSerializer.Serialize(RepositorioRegistro, 
                new JsonSerializerOptions { WriteIndented = true});

            File.WriteAllText(rutaArchivo, json);
        }

        private void CargarRegistrosDesdeJson()
        {
            if (File.Exists(rutaArchivo))
            {
                string json = File.ReadAllText(rutaArchivo);
                var registro = JsonSerializer.Deserialize<ObservableCollection<Registro>>(json);

                if (registro != null)
                {
                    foreach (var registros in registro)
                    {
                        RepositorioRegistro.Add(registros);
                    }
                }
            }
        }
    }
}
