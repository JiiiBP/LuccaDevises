using LuccaDevisesIRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesRepositories
{
    public class FileTraitmentRepository : IFileTraitmentRepository
    {

        /// <summary>
        /// Lit le fichier et retourne une liste de ligne
        /// </summary>
        /// <param name="filePath">Chemin du fichier passé en argument</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetFileLines(string filePath)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (string line in File.ReadLines(filePath))
                {
                    lines.Add(line.TrimEnd('\r', '\n'));
                }
                return lines;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        /// <summary>
        /// Renvoit un tuple pour la première ligne du fichier (Devise depart;Valeur de début;Devise arrivée)
        /// </summary>
        /// <param name="line">Ligne à formatter</param>
        /// <returns>Renvoit un tuple pour une ligne de fichier (Devise depart;Valeur de début;Devise arrivée)</returns>
        /// <exception cref="Exception">Si la ligne est mal formattée</exception>
        public Tuple<string, int, string> ParseConversionLine(string line)
        {
            string[] data = line.Split(';');
            if (data.Length != 3)
            {
                throw new Exception($"La première ligne doit avoir le format 'D1;M;D2' où D1 et D2 doit être une chaine de caractère de 3 and M un nombre entier");
            }
            if (data[0].Length != 3)
            {
                throw new Exception($"Erreur formattage(D1;M;D2) première ligne, D1 est invalide : il doit être une chaine de 3 caractères");
            }
            int initialAmount;
            if (!Int32.TryParse(data[1], out initialAmount) || initialAmount <= 0)
            {
                throw new Exception($"Erreur formattage (D1;M;D2) première ligne, M est invalide : M un nombre entier");
            }
            if (data[2].Length != 3)
            {
                throw new Exception($"Erreur formattage (D1;M;D2) première ligne, D2 est invalide : il doit être une chaine de 3 caractères");
            }
            if (data[0] == data[2])
            {
                throw new Exception($"Erreur formattage (D1;M;D2) première ligne, D1 et D2 ne doivent pas être identiques");
            }
            return new(data[0], initialAmount, data[2]);
        }

        /// <summary>
        /// Formatte chaque ligne contenant un taux d'une devise à une autre
        /// </summary>
        /// <param name="lines">Les lignes à formatter doivent être au format suivant 'DD;DA;T'</param>
        /// <returns>Une liste de tuples qui contient pour chacun la devise de départ, le taux d'échange et la devise d'arrivée</returns>
        /// <exception cref="Exception">Si la ligne est invalide</exception>
        public List<Tuple<string, string, double>> ParseExchangeRates(List<Tuple<int, string>> lines)
        {
            List<Tuple<string, string, double>> exchangeRates = new();
            foreach ((int lineNumber, string line) in lines)
            {
                exchangeRates.Add(ParseExchangeRate(lineNumber, line));
            }
            return exchangeRates;
        }

        /// <summary>
        /// Formate en tuple une string (line) issue du fichier
        /// </summary>
        /// <param name="lineNumber">Le num de la ligne pour le debuggage coté utilisateur</param>
        /// <param name="line">La ligne à formatter au format 'DD;DA;T'</param>
        /// <returns>un tuple contenant la devise de depart, la devise d'arrivée et le taux de change</returns>
        /// <exception cref="Exception">Si la ligne est mal formattée</exception>
        public Tuple<string, string, double> ParseExchangeRate(int lineNumber, string line)
        {
            string[] data = line.Split(';');
            if (data.Length != 3)
            {
                throw new Exception($"Erreur à la ligne {lineNumber}, son format doit être celui-ci : 'DD;DA;T' où DD et DA sont une chaine de caractère égale à 3 et T un nombre à 4 décimales");
            }
            if (data[0].Length != 3)
            {
                throw new Exception($"Erreur à la ligne {lineNumber}, son format doit être celui-ci : 'DD;DA;T', DD est invalide : il doit être une chaine de 3 caractères");
            }
            if (data[1].Length != 3)
            {
                throw new Exception($"Erreur à la ligne {lineNumber}, son format doit être celui-ci : 'DD;DA;T', DA est invalide : il doit être une chaine de 3 caractèresg");
            }
            double exchangeRate;
            if (data[2].Split('.').Length != 2 || data[2].Split('.')[1].Length != 4)
            {
                throw new Exception($"Erreur à la ligne {lineNumber}, son format doit être celui-ci : 'DD;DA;T', T est invalide, T un nombre à 4 décimales séparé par '.'");
            }
            if (!Double.TryParse(data[2], NumberStyles.Number, CultureInfo.InvariantCulture, out exchangeRate) || exchangeRate <= 0)
            {
                throw new Exception($"Erreur à la ligne {lineNumber}, son format doit être celui-ci : 'DD;DA;T', T est invalide, T un nombre à 4 décimales séparé par '.'");
            }
            return new(data[0], data[1], exchangeRate);
        }
    }
}
