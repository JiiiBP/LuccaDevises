using LuccaDevisesIRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesModels
{
    public class FileTraitment
    {
        /// <summary>Devise de départ</summary>
        public string From { get; }
        /// <summary>Convertir en cette devise</summary>
        public string To { get; }
        /// <summary>Montant initial</summary>
        public int Source { get; }
        /// <summary>
        /// Ensemble des taux d'échange des devises contenant les infos de la devise d'entrée, de sortie et le taux
        /// </summary>
        public List<Tuple<string, string, double>> exchangePaths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Chemin du fichier passé en argument</param>
        /// <exception cref="Exception"></exception>
        public FileTraitment(string filePath, IFileTraitmentRepository _IFileTraitmentRepository)
        {
            List<string> lines = _IFileTraitmentRepository.GetFileLines(filePath);

            if (lines.Count <= 2)
            {
                throw new Exception($"Le fichier doit contenir au moins 3 lignes");
            }

            var myTuple = _IFileTraitmentRepository.ParseConversionLine(lines[0]);
            From = myTuple.Item1;
            Source = myTuple.Item2;
            To = myTuple.Item3;

            List<Tuple<int, string>> exchageRateLinesWithLineNumber = lines
                .Select((line, index) => new Tuple<int, string>(index + 1, line))
                .ToList()
                .GetRange(2, lines.Count - 2)
                .FindAll(t => t.Item2.Length != 0);

            exchangePaths = _IFileTraitmentRepository.ParseExchangeRates(exchageRateLinesWithLineNumber);
        }
    }
}
