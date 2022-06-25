using LuccaDevisesModels;

namespace LuccaDevisesIRepositories
{
    public interface IGraphRepository
    {
        void AddNode(string nodeData, Graph<string, double> graph);
        void AddPath(string nodeA, string nodeB, double pathData, Graph<string, double> graph);
        Graph<string, double> CreateGraph(string startNode, string endNode, List<Tuple<string, string, double>> paths);
        List<Tuple<string, double>> ExecBFS(string from, string to, Graph<string, double> graph);
        bool HasExistingNode(string nodeData, Graph<string, double> graph);
        bool HasExistingPath(string from, string to, Graph<string, double> graph);
    }
}