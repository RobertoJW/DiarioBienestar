using Microsoft.Maui.Storage;

namespace DiarioBienestar
{
    public partial class MainPage : ContentPage
    {
        Random random = new Random();
        RepositorioRegistros repositorio; 

        // lista de frases que se mostrarán al inicio de la app.
        string[] frases = { "Cada día es una nueva oportunidad para crecer.", "Tú eres más fuerte de lo que crees.",
            "Los sueños grandes requieren esfuerzo grande.", "El éxito comienza con el primer paso.",
            "Cree en ti y todo será posible.", "Las pequeñas acciones generan grandes cambios." };
        private string? fraseGeneradaAleatoriamente; 

        public MainPage()
        {
            InitializeComponent();

            string NombreUsuario = Preferences.Get("NombreUsuario", string.Empty);
            repositorio = new RepositorioRegistros(); 

            if (!string.IsNullOrEmpty(NombreUsuario))
            {
                MostrarSaludo(NombreUsuario);
                EntradaStack.IsVisible = false;
            }
            else
            {
                BienvenidaStack.IsVisible = false;
            }
        }

        private async void BtnAddUser(object sender, EventArgs e)
        {
            string NombreUsuario = EntryNombre.Text;

            if (string.IsNullOrEmpty(NombreUsuario))
            {
                await DisplayAlert("Aviso.","Por favor, rellene el campo vacío.","Ok");
            }
            else
            {
                Preferences.Set("NombreUsuario", NombreUsuario);
                EntradaStack.IsVisible = false;
                MostrarSaludo(NombreUsuario);
                EntryNombre.Text = ""; 
            }
        }

        private async void DeleteBtn(object sender, EventArgs e)
        {
            bool pregunta = await DisplayAlert("Aviso","¿Seguro que quieres " +
                "eliminar el usuario?. Perderá todos los registros.", "Si", "No");

            if (pregunta)
            {
                Preferences.Remove("NombreUsuario");
                repositorio.BorrarTodosLosRegistros();
                EntradaStack.IsVisible = true;
                BienvenidaStack.IsVisible = false;
                await DisplayAlert("Baja", "Se ha eliminado al usuario.", "Ok");
            }
        }

        private void MostrarSaludo(string nombreUsuario)
        {
            int horaActual = DateTime.Now.Hour;
            string saludo;

            if (horaActual >= 6 && horaActual < 12)
            {
                saludo = "¡Buenos días";
            }
            else if (horaActual >= 12 && horaActual < 21)
            {
                saludo = "¡Buenas tardes";
            }
            else
            {
                saludo = "¡Buenas noches";
            }
            BienvenidaLabel.Text = $"{saludo}, {nombreUsuario}!";
            FraseMotivacional.Text = GenerarFrasesMotivacionalesAleatorios();
            BienvenidaStack.IsVisible = true;
        }

        private string GenerarFrasesMotivacionalesAleatorios()
        {
            List<string> frasesMotivadoras = new List<string>(frases);
            int indiceFrase = random.Next(frasesMotivadoras.Count);
            fraseGeneradaAleatoriamente = frasesMotivadoras[indiceFrase];

            return fraseGeneradaAleatoriamente.ToString();
        }
    }
}
