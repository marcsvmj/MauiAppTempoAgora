using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade, string lingua)
        {
            Tempo t = null;

            string chave = "42ec69b5ce30b72dbd780b15af0caf30";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" + $"lang={lingua}&" + $"q={cidade}&units=metric&appid={chave}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (!resp.IsSuccessStatusCode)
                    {
                        switch (resp.StatusCode)
                        {
                            case System.Net.HttpStatusCode.NotFound:
                                throw new Exception("Cidade não encontrada.");
                            case System.Net.HttpStatusCode.Unauthorized:
                                throw new Exception("Chave da API inválida.");
                            case System.Net.HttpStatusCode.BadRequest:
                                throw new Exception("Requisição mal formada.");
                            default:
                                throw new Exception($"Erro HTTP: {resp.StatusCode}");
                        }
                    }

                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; // Fechar o objeto t

                }

            } catch (HttpRequestException) // tratar sem internet
            {
                throw new Exception("Sem conexão com a internet.");

            } catch (Exception ex) 
            {
                throw new Exception("Erro inesperado: " + ex.Message);
            }

            return t;
        }
    }
}