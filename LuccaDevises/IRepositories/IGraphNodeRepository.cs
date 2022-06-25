using LuccaDevisesModels;

namespace LuccaDevisesIRepositories
{
    public interface IGraphNodeRepository
    {
        void AddEdge(GraphNode<string, double> newEdge, double pathData, GraphNode<string, double> graphNode);
    }
}