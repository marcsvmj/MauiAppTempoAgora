using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text) && !string.IsNullOrEmpty(txt_lingua.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text, txt_lingua.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = "Dados Básicos:\n" +
                                         $"Temperatura Máxima: {t.temp_max}°C \n" +
                                         $"Temperatura Mínima: {t.temp_min}°C \n" +
                                         $"Visibilidade: {t.visibility / 1000}Km \n" +
                                         $"Velocidade do Vento: {t.speed}Km/h \n\n" +
                                         "Dados Avançados:\n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Descrição do Clima: {t.description} \n";

                        lbl_res.Text = dados_previsao;

                    } else
                    {
                        lbl_res.Text = "Sem dados para a previsão. \n Tente novamente mais tarde.";
                    }

                } else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }

            } catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
