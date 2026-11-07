using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.ViewModels
{
    // Lista con paginación para tablas largas (carreras, cursos, docentes).
    public class ListaPaginada<T>
    {
        public List<T> Elementos { get; }
        public int Pagina { get; }
        public int TotalPaginas { get; }
        public int TotalElementos { get; }

        public bool TieneAnterior => Pagina > 1;
        public bool TieneSiguiente => Pagina < TotalPaginas;

        private ListaPaginada(List<T> elementos, int total, int pagina, int tamano)
        {
            Elementos = elementos;
            TotalElementos = total;
            Pagina = pagina;
            TotalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamano));
        }

        public static async Task<ListaPaginada<T>> CrearAsync(IQueryable<T> consulta, int pagina, int tamano)
        {
            var total = await consulta.CountAsync();
            var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamano));
            pagina = Math.Clamp(pagina, 1, totalPaginas);

            var elementos = await consulta.Skip((pagina - 1) * tamano).Take(tamano).ToListAsync();
            return new ListaPaginada<T>(elementos, total, pagina, tamano);
        }
    }
}
