using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevisesRepositories
{
    public class GraphRepository : IGraphRepository
    {
        IGraphNodeRepository _IGraphNodeRepository;
        /// <summary>
        /// Injection de dépendance par constructeur
        /// </summary>
        /// <param name="iGraphNodeRepository"></param>
        public GraphRepository(IGraphNodeRepository iGraphNodeRepository)
        {
            _IGraphNodeRepository = iGraphNodeRepository;
        }

        /// <summary>
        /// Ajouter un noeud au graph passé en param
        /// </summary>
        /// <param name="nodeData">valeur du noeud</param>
        /// <param name="graph">graph</param>
        /// <exception cref="ArgumentException">Renvoi une exception si le noeud existe</exception>
        public void AddNode(string nodeData, Graph<string, double> graph)
        {
            GraphNode<string, double> newNode = new(nodeData);
            if (HasExistingNode(nodeData, graph))
            {
                throw new ArgumentException($"Noeud {nodeData} existant");
            }
            graph.nodes.Add(newNode);
        }

        /// <summary>
        /// Ajouter un chemin d'un noeud à un autre mais dans un sens unique A vers B seulement mais pas B vers A
        /// </summary>
        /// <param name="nodeA">Noeud de départ</param>
        /// <param name="graph">graph</param>
        /// <param name="nodeB">Noeud d'arrivée</param>
        /// <param name="pathData">Valeur du chemin reliant les 2 noeuds</param>
        /// <exception cref="ArgumentException">renvoi une excpetion si une des valeurs est nulle</exception>
        /// <exception cref="Exception">Si un noeud n'est pas trouvé</exception>
        public void AddPath(string nodeA, string nodeB, double pathData, Graph<string, double> graph)
        {
            GraphNode<string, double>? graphNodeA = GetNode(nodeA, graph);
            GraphNode<string, double>? graphNodeB = GetNode(nodeB, graph);

            _IGraphNodeRepository.AddEdge(graphNodeB, pathData, graphNodeA);
        }

        /// <summary>
        /// Effectue une recherche du noeud de départ au noeud d'arrivée
        /// </summary>
        /// <param name="from">Noeud de départ</param>
        /// <param name="graph">graph</param>
        /// <param name="to">Noeud d'arrivée</param>
        /// <returns>Une liste de l'ensemble des chemins empruntés pour atteindre le noeud d'arrivée</returns>
        /// <exception cref="Exception">Si un chemin n'est pas trouvé entre deux noeuds</exception>
        /// <exception cref="ArgumentException">Si une des valeurs des noeuds est nulle</exception>
        /// <exception cref="Exception">Si un des deux noeuds est introuvable</exception>
        public List<Tuple<string, double>> ExecBFS(string from, string to, Graph<string, double> graph)
        {
            ResetExploredNodes(graph);
            GraphNode<string, double> startNode = GetNode(from, graph);
            GraphNode<string, double> endNode = GetNode(to, graph);

            Queue<GraphNode<string, double>> toExplore = new();
            startNode.Explored = true;
            toExplore.Enqueue(startNode);

            while (toExplore.Count != 0)
            {
                GraphNode<string, double> currentNode = toExplore.Dequeue();
                if (currentNode == endNode)
                {
                    return Traceback(currentNode);
                }
                foreach (Tuple<GraphNode<string, double>, double> edge in currentNode.Edges)
                {
                    if (!edge.Item1.Explored)
                    {
                        edge.Item1.Explored = true;
                        edge.Item1.ParentNode = new(currentNode, edge.Item2);
                        toExplore.Enqueue(edge.Item1);
                    }
                }
            }

            throw new Exception($"Aucun chemin trouvé de {from} à {to}");
        }

        /// <summary>
        /// Crée une liste d'étapes (ensemble de chemins) pour aller au noeud sans parent depuis le noeud en cours
        /// </summary>
        /// <param name="node">Retracer le chemin à partir de ce noeud</param>
        /// <returns>Une liste d'étapes avec à chaque fois les données de chemin qui ont été associées (la première étape a une valeur par défaut en tant que données de chemin car il ne s'agit pas d'une étape effectuée)</returns>
        private static List<Tuple<string, double>> Traceback(GraphNode<string, double> node)
        {
            List<Tuple<string, double>> path = new();
            while (node != null)
            {
                if (node.ParentNode == null)
                {
                    path.Add(new(node.Data, default));
                    break;
                }
                path.Add(new(node.Data, node.ParentNode.Item2));
                node = node.ParentNode.Item1;
            }
            path.Reverse();
            return path;
        }

        /// <summary> Réinitialiser toutes les données du graph </summary>
        private void ResetExploredNodes(Graph<string, double> graph)
        {
            graph.nodes.ForEach(n =>
            {
                n.Explored = false;
                n.ParentNode = null;
            });
        }

        /// <summary>
        /// Booleen pour savoir si le noeud est existant
        /// </summary>
        /// <param name="graph">graph</param>
        /// <param name="nodeData">valeur du noeud</param>
        /// <returns>vrai s'il existe un nœud existant avec les mêmes données de nœud, sinon c'est faux</returns>
        public bool HasExistingNode(string nodeData, Graph<string, double> graph)
        {
            GraphNode<string, double>? node = graph.nodes.Find(n => EqualityComparer<string>.Default.Equals(n.Data, nodeData));
            return node != null;
        }

        /// <summary>
        /// Booleen pour savoir si le chemin existe
        /// </summary>
        /// <param name="from">Noeud de départ</param>
        /// <param name="graph">graph</param>
        /// <param name="to">Noeud d'arrivée</param>
        /// <returns>vrai si un chemin existe, faux sinon</returns>
        /// <exception cref="ArgumentException">Si au moins une des données de nœud données est nulle</exception>
        public bool HasExistingPath(string from, string to, Graph<string, double> graph)
        {
            GraphNode<string, double> nodeFrom = GetNode(from, graph);
            GraphNode<string, double> nodeTo = GetNode(to, graph);
            return nodeFrom.Edges.Find(t => EqualityComparer<string>.Default.Equals(t.Item1.Data, nodeTo.Data)) != null;
        }

        /// <summary>
        /// Avoir le noeud grace à sa valeur
        /// </summary>
        /// <param name="graph">graph</param>
        /// <param name="nodeData">Valeur recherchée</param>
        /// <returns>Noeud existant</returns>
        /// <exception cref="ArgumentException">Si la donnée du nœud est nulle</exception>
        private GraphNode<string, double> GetNode(string nodeData, Graph<string, double> graph)
        {
            if (nodeData == null)
            {
                throw new ArgumentException("La donnée du nœud ne doit pas être nulle");
            }
            GraphNode<string, double>? node = graph.nodes.Find(n => EqualityComparer<string>.Default.Equals(n.Data, nodeData));
            if (node == null)
            {
                throw new Exception(nodeData.ToString());
            }
            return node;
        }

        /// <summary>
        /// Créer un graphique basé sur les nœuds existants avec les chemins (des taux de change) qui les relient
        /// </summary>
        /// <param name="startNode">Noeud de départ</param>
        /// <param name="endNode">Noeud d'arrivée</param>
        /// <param name="paths">Les différents taux de change qui existent</param>
        /// <returns>Un graphique qui représente tous les échanges possibles</returns>
        /// <exception cref="Exception">Dans le cas où un chemin est défini plusieurs fois pour les mêmes devises</exception>
        public Graph<string, double> CreateGraph(string startNode, string endNode, List<Tuple<string, string, double>> paths)
        {
            Graph<string, double> graph = new Graph<string, double>();

            AddNode(startNode, graph);
            AddNode(endNode, graph);

            foreach ((string from, string to, double exchangeRate) in paths)
            {
                if (!HasExistingNode(from, graph))
                {
                    AddNode(from, graph);
                }
                if (!HasExistingNode(to, graph))
                {
                    AddNode(to, graph);
                }
                if (!HasExistingPath(from, to, graph))
                {
                    AddPath(from, to, exchangeRate, graph);
                    AddPath(to, from, Math.Round(1.0 / exchangeRate, 4), graph);
                }
                else
                {
                    throw new Exception($"Erreur: la conversion de {from} à {to} est définie plusieurs fois");
                }
            }

            return graph;
        }
    }
}
