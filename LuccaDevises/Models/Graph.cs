using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesModels
{
    /// <summary>
    /// Objet : Graph
    /// </summary>
    /// <typeparam name="TCurrency"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Graph<TCurrency, TValue>
    {
        /// <summary> Une liste des noeuds du graph </summary>
        public List<GraphNode<string, double>> nodes;

        /// <summary> Constructeur </summary>
        public Graph()
        {
            nodes = new();
        }
    }
}
