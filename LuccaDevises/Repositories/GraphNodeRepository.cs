using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesRepositories
{
    public class GraphNodeRepository : IGraphNodeRepository
    {
        /// <summary>
        /// Ajouter un nouveau chemin à partir de ce noeud
        /// </summary>
        /// <param name="newEdge">noeud d'arrivée depuis le noeud en court</param>
        /// <param name="graphNode">Graph</param>
        /// <param name="pathData">valeur associée au chemin (taux)</param>
        /// <exception cref="ArgumentException">Si le même chemin existe, renvoi d'exception</exception>
        public void AddEdge(GraphNode<string, double> newEdge, double pathData, GraphNode<string, double> graphNode)
        {
            Tuple<GraphNode<string, double>, double> listElement = new(newEdge, pathData);
            if (graphNode.Edges.Contains(listElement))
            {
                throw new ArgumentException($"Le chemin {newEdge} existe déjà");
            }
            graphNode.Edges.Add(listElement);
        }
    }
}
