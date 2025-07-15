using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiarioBienestar
{
    public class Registro
    {
        public DateOnly Fecha {  get; set; }
        public string Comentario {  get; set; }
        public double Intensidad {  get; set; }
        public double NivelFisico {  get; set; }

        public double NivelEnergia { get; set; }

        public Registro() { }
        public Registro(DateOnly fecha, string comentario, double intensidadEjercicio, double nivelFisico, 
            double nivelEnergia)
        {
            Fecha = fecha;
            Comentario = comentario;
            Intensidad = intensidadEjercicio;
            NivelFisico = nivelFisico;
            NivelEnergia = nivelEnergia;
        }
    }
}
