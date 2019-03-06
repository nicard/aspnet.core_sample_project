using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebAPI.API.Modelos
{
    public static class PaginacaoExtensions
    {
        public static LivroPaginacao ToListPaginate(this IQueryable<LivroApi> query, Paginacao paginacao)
        {
            int total_itens = query.Count();
            int total_paginas = (int)Math.Ceiling(total_itens / (double)paginacao.Tamanho);
            return new LivroPaginacao
            {
                Total = total_itens,
                TotalPaginas = total_paginas,
                NumeroPagina = paginacao.Pagina,
                TamanhoPagina = paginacao.Tamanho,
                Resultado = query.Skip(paginacao.Tamanho * (paginacao.Pagina - 1)).Take(paginacao.Tamanho).ToList(),
                Anterior = (paginacao.Pagina > 1) ? $"livros?tamanho={paginacao.Tamanho}&pagina={paginacao.Pagina - 1}" : "",
                Proximo = (paginacao.Pagina < total_paginas) ? $"livros?tamanho={paginacao.Tamanho}&pagina={paginacao.Pagina + 1}" : ""
            };
        }
    }

    public class LivroPaginacao
    {
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IList<LivroApi> Resultado { get; set; }
        public string Anterior { get; set; }
        public string Proximo { get; set; }
    }

    public class Paginacao
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 25;
    }
}
