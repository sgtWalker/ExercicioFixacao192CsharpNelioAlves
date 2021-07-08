using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using ExercicioFixacao192.Entities;

namespace ExercicioFixacao192
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pathFile = SolicitarPathArquivo(false);
                ValidarExtensao(pathFile);
                RealizarProcessamentoArquivo(pathFile);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ocorreu uma IOException:");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu uma exception:");
                Console.WriteLine(ex.Message);
            }

            static string SolicitarPathArquivo(bool arquivoSaida)
            {
                if (arquivoSaida)
                    Console.WriteLine("Informe o path de saída: ");
                else
                    Console.WriteLine("Informe o path de entrada: ");

                return Console.ReadLine();
            }
            static void ValidarExtensao(string pathFile)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(pathFile);
                if (directoryInfo.Extension != ".csv")
                    throw new IOException("Informe apenas arquivos com a extensão .csv !");
            }

            static List<Product> RealizarImportacao(string pathFile)
            {
                
                List<Product> produtos = new List<Product>();
                int cont = 0;
                using (var fileStream = new FileStream(pathFile, FileMode.Open))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            cont++;
                            string linha = streamReader.ReadLine();
                            var dadosProduto = linha.Split(',');

                            if (dadosProduto != null)
                            {
                                produtos.Add(new Product()
                                {
                                    Name = dadosProduto[0].ToString(),
                                    Price = double.Parse(dadosProduto[1], CultureInfo.InvariantCulture),
                                    Quantity = int.Parse(dadosProduto[2])
                                });
                            }
                            else
                            {
                                throw new IOException($"Não foi possível realizar a leitura do arquivo, verifique a linha: {cont.ToString()}");
                            }
                        }
                    };
                };

                return produtos;
            }
            static void RealizarProcessamentoArquivo(string pathFile)
            {
                Console.WriteLine();
                Console.WriteLine("Iniciando o processamento do arquivo!");

                var produtos = RealizarImportacao(pathFile);

                if (produtos.Count != 0)
                {
                    GerarArquivoSaida(produtos, pathFile);
                }
                else
                {
                    throw new IOException("Arquivo sem registros, por gentileza, verifique!");
                }
            }
            static void GerarArquivoSaida(List<Product> produtos, string pathFile)
            {
                string diretorioSaida = $"{Path.GetDirectoryName(pathFile)}\\out";
                Directory.CreateDirectory(diretorioSaida);

                using (var fileStream = new FileStream($"{diretorioSaida}\\sumary.csv", FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        foreach (Product produto in produtos)
                            streamWriter.WriteLine(produto.ToString());
                    };
                };

                Console.WriteLine($"Processamento do arquivo realizado com sucesso, verifique o arquivo gerado no diretório: {diretorioSaida}");
            }
        }
    }
}
