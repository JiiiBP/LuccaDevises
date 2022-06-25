using LuccaDevisesIRepositories;
using LuccaDevisesRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace LuccaDevises
{
    public class RunApp
    {
        private IBusinessClassLuccaRepository _IBusinessClassLucca;
        public RunApp(IBusinessClassLuccaRepository IbusinessClassLucca)
        {
            _IBusinessClassLucca = IbusinessClassLucca;
        }
        public void Run(string filePath)
        {
            Console.WriteLine($"{_IBusinessClassLucca.ConvertCurrency(filePath)}");
        }
    }
}
