using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClientes
{
    public class LivroApiClient
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _accessor;

        public LivroApiClient(HttpClient httpClient, IHttpContextAccessor accessor) {
            this._httpClient = httpClient;
            this._accessor = accessor;
        }

        private void AddBearerToken()
        {
            var token = _accessor.HttpContext.User.Claims.First(c => c.Type == "Token").Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            AddBearerToken();
            HttpResponseMessage response = await this._httpClient.GetAsync($"ListaLeitura/{tipo}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Lista>();
        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage response = await this._httpClient.GetAsync($"Livros/{id}/capa");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage response = await this._httpClient.GetAsync($"Livros/{id}");
            response.EnsureSuccessStatusCode();

           return await response.Content.ReadAsAsync<LivroApi>();
        }
               
        public async Task DeleteLivroAsync(int id)
        {
            AddBearerToken();
            var response = await this._httpClient.DeleteAsync($"Livros/{id}");
            response.EnsureSuccessStatusCode();
        }
               
        public async Task PostLivroAsync(LivroUpload modal)
        {
            AddBearerToken();
            HttpContent contant = CreateMultiParteFormDataContent(modal);
            var response = await this._httpClient.PostAsync("Livros", contant);
            response.EnsureSuccessStatusCode();
        } 

        public async Task PutLivroAsync(LivroUpload modal)
        {
            HttpContent contant = CreateMultiParteFormDataContent(modal);
            var response = await this._httpClient.PutAsync("Livros", contant);
            response.EnsureSuccessStatusCode();
        }

        private string PrepareContent(string tag)
        {
            return $"\"{tag}\"";
        }

        private HttpContent CreateMultiParteFormDataContent(LivroUpload modal)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(modal.Titulo), PrepareContent("titulo"));
            content.Add(new StringContent(modal.Subtitulo), PrepareContent("subtitulo"));
            content.Add(new StringContent(modal.Resumo), PrepareContent("resumo"));
            content.Add(new StringContent(modal.Autor), PrepareContent("autor"));
            content.Add(new StringContent(modal.Lista.ToString()), PrepareContent("lista"));

            if (modal.Id > 0)
            {
                content.Add(new StringContent(modal.Id.ToString()), PrepareContent("id"));
            }

            if(modal.Capa != null)
            {
                var imagemCapa = new ByteArrayContent(modal.Capa.ConvertToBytes());
                imagemCapa.Headers.Add("content-type", "image/png");
                content.Add(imagemCapa, PrepareContent("capa"), PrepareContent("capa.png"));
            }

            return content;
        }
    }
}
