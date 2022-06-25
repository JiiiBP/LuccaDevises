using LuccaDevisesIRepositories;
using LuccaDevisesRepositories;
using System;
using Unity;

namespace LuccaDevises
{
    class Program
    {
        /// <summary>
        /// Lancement du programme
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    if (File.Exists(args[0]))
                    {
                        DependancyInjectionByunityConfig.Start();
                        var consoleAdapter = DependancyInjectionByunityConfig.Container.Resolve<RunApp>();
                        consoleAdapter.Run(args[0]);
                    }
                    else
                    {
                        Console.WriteLine("Chemin de fichier non valide");
                    }
                }
                else
                {
                    Console.WriteLine("Veuillez renseigner un argument (Chemin du fichier contenant les devises et taux de change associés)");
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Une erreur est survenue, détail erreur:");
                Console.Error.WriteLine(e.Message);
            }
        }

        public class DependancyInjectionByunityConfig
        {
            public static IUnityContainer? Container;
            /// <summary>
            /// instance de unitycontainer
            /// </summary>
            public static void Start()
            {
                Container = new UnityContainer();
                Register();
            }

            /// <summary>
            /// Binding Interface à la classe business
            /// </summary>
            private static void Register()
            {
                Container.RegisterType<IBusinessClassLuccaRepository, BusinessClassLuccaRepository>();
                Container.RegisterType<IGraphRepository, GraphRepository>();
                Container.RegisterType<IGraphNodeRepository, GraphNodeRepository>();
                Container.RegisterType<IFileTraitmentRepository, FileTraitmentRepository>();
            }
        }
    }
}
