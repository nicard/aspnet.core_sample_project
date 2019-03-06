using Alura.ListaLeitura.Modelos;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Alura.WebAPI.API.Modelos
{

    public static class LiVroOrderExtensions
    {
        public static IQueryable<Livro> AplicaOrder(this IQueryable<Livro> query, LivroOrder filtro)
        {
            if (filtro != null)
            {
                if (!string.IsNullOrEmpty(filtro.OrdenarPor))
                {
                    query = query.OrderBy(filtro.OrdenarPor);
                }
            }
            return query;
        }
    }

    public class LivroOrder
    {
        public string OrdenarPor { get; set; }
    }
}
