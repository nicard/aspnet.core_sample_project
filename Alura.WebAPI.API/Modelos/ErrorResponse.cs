using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alura.WebAPI.API.Modelos
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ErrorResponse InnterError { get; set; }
        public string[] Detalhes { get; set; }

        public static ErrorResponse From(Exception e)
        {
            if(e == null)
            {
                return null;
            }

            return new ErrorResponse
            {
                Code = e.HResult,
                Message = e.Message,
                InnterError = ErrorResponse.From(e.InnerException)
            };
        }

        public static ErrorResponse FromModelState(ModelStateDictionary model)
        {
            var erros = model.Values.SelectMany(m => m.Errors);
            return new ErrorResponse
            {
                Code = 100,
                Message = "Houve um erro no envio de requisição",
                Detalhes = erros.Select( e => e.ErrorMessage).ToArray()
            };
        }
    }
}
