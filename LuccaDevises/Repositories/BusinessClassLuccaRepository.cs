using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesRepositories
{
    public class BusinessClassLuccaRepository : IBusinessClassLuccaRepository
    {
        private IFileTraitmentRepository _IFileTraitmentRepository;

        private IGraphRepository _IGraphRepository;
        /// <summary>
        /// Injection de dépendance de l'interface du graph et celui du traitement de fichier
        /// </summary>
        /// <param name="IGraphRepository"></param>
        /// <param name="IFileTraitmentRepository"></param>
        public BusinessClassLuccaRepository(IGraphRepository IGraphRepository, IFileTraitmentRepository IFileTraitmentRepository)
        {
            _IGraphRepository = IGraphRepository;
            _IFileTraitmentRepository = IFileTraitmentRepository;
        }

        /// <summary>
        /// Renvoie la valeur attendu de la conversion voulue en passant en param le chemin du fichier d'entrée
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int ConvertCurrency(string filePath)
        {
            FileTraitment ft = new FileTraitment(filePath, _IFileTraitmentRepository);
            double finalResult = ft.Source;

            Graph<string, double> MyGraph = _IGraphRepository.CreateGraph(ft.From, ft.To, ft.exchangePaths);
            List<Tuple<string, double>> steps = _IGraphRepository.ExecBFS(ft.From, ft.To, MyGraph);

            foreach ((string currency, double exchangeRate) in steps)
            {
                if (currency == ft.From)
                    continue;
                finalResult = Math.Round(finalResult * exchangeRate, 4);
            }
            return (int)Math.Round(finalResult);
        }
    }
}
