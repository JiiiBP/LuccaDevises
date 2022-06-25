using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesModels
{
    /// <summary>
    /// Objet : Noeud
    /// </summary>
    /// <typeparam name="Ts">Valeur du noeud</typeparam>
    /// <typeparam name="Td">Quantification de la valeur du noeud</typeparam>
    public class GraphNode<Ts, Td>
    {
        /// <summary> Valeur du noeud </summary>
        public string Data { get; }
        /// <summary> Chemins vers d'autres noeuds depuis ce noeud </summary>
        public List<Tuple<GraphNode<string, double>, double>> Edges { get; }

        // Utilisé pour l'algorithme permettant d'explorer le graph
        /// <summary> Booléen pour savoir si on est passé par ce noeud ou pas </summary>
        public bool Explored { get; set; }
        /// <summary> Parent du noeud (par quel noeud on passe pour arriver sur le noeud en question) </summary>
        public Tuple<GraphNode<string, double>, double>? ParentNode { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">On passe la valeur du noeud en param</param>
        public GraphNode(string data)
        {
            ParentNode = null;
            Explored = false;
            Data = data;
            Edges = new List<Tuple<GraphNode<string, double>, double>>();
        }
    }
}
