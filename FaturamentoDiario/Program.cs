using System.Text.Json;
using FaturamentoDiario.Entidade;
using Newtonsoft.Json;

namespace FaturamentoDiario
{
    public class Program
    {
        public static void Main()
        {
            string diretorioAtual = Environment.CurrentDirectory;
            string diretorioCompleto = Directory.GetParent(diretorioAtual).Parent.Parent.FullName;
            string arquivo = diretorioCompleto + @"\dados.json";
            
            int dataMinima = 0, dataMaxima = 0;
            double minimo = int.MaxValue, maximo = int.MinValue;
            double mediaMensal = 0.0, contador = 0.0;

            using (StreamReader reader = new(arquivo))
            {
                string jsonString = reader.ReadToEnd();
                List<DadosDeFaturamento> dados = JsonConvert.DeserializeObject<List<DadosDeFaturamento>>(jsonString);
                double mediaCalculada = 0;

                foreach (var item in dados)
                {
                    double valorAtual = item.Valor;
                    int dataAtual = item.Dia;

                    mediaCalculada += item.Valor;

                    // obtém o menor valor (sendo maior que 0)
                    if (valorAtual > 0 && valorAtual < minimo)
                    {
                        dataMinima = dataAtual;
                        minimo = valorAtual;
                    }
                    
                    // obtém o maior valor
                    if (valorAtual > maximo)
                    {
                        dataMaxima = dataAtual;
                        maximo = valorAtual;
                    }

                    // conta os valores para cálculo de média
                    if (valorAtual > 0)
                    {
                        contador++;
                    }
                }

                Console.WriteLine($"Data: {dataMinima} - Menor valor: {minimo}");
                Console.WriteLine($"Data: {dataMaxima} - Maior valor: {maximo}");

                mediaMensal = mediaCalculada / contador;

                Console.WriteLine("\nDias cujo faturamento diário superou a média mensal:");
                foreach (var item in dados)
                {
                    if (item.Valor > mediaMensal)
                    {
                        Console.WriteLine($"- Dia: {item.Dia}");
                    }
                }

                Console.WriteLine($"Média mensal: {mediaMensal}");
            }
        }
    }
}