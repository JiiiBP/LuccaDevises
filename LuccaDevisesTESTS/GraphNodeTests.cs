using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using LuccaDevisesRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace LuccaDevisesTESTS;

[TestClass]
public class GraphNodeTests
{
    GraphNodeRepository graphNodeRepository = new GraphNodeRepository();

    [TestMethod]
    public void AddNode_AddValidEdgeTest()
    {
        GraphNode<string, double> node = new("EUR");
        GraphNode<string, double> otherNode = new("USD");

        graphNodeRepository.AddEdge(otherNode, 1.0, node);

        Assert.AreEqual(1, node.Edges.Count);
        Assert.AreEqual("USD", node.Edges[0].Item1.Data);
        Assert.AreEqual(1.0, node.Edges[0].Item2);
    }

    [TestMethod]
    public void AddNode_AddMultipleValidEdgesTest()
    {
        GraphNode<string, double> firstnode = new("EUR");
        GraphNode<string, double> secondNode = new("USD");
        GraphNode<string, double> thirdNode = new("JPY");

        var graphNodeRepository = new GraphNodeRepository();
        graphNodeRepository.AddEdge(secondNode, 1.0, firstnode);
        graphNodeRepository.AddEdge(thirdNode, 1.0, firstnode);
        graphNodeRepository.AddEdge(secondNode, 2.0, firstnode);

        Assert.AreEqual(3, firstnode.Edges.Count);
        Assert.AreEqual("USD", firstnode.Edges[0].Item1.Data);
        Assert.AreEqual(1.0, firstnode.Edges[0].Item2);
        Assert.AreEqual("JPY", firstnode.Edges[1].Item1.Data);
        Assert.AreEqual(1.0, firstnode.Edges[1].Item2);
        Assert.AreEqual("USD", firstnode.Edges[2].Item1.Data);
        Assert.AreEqual(2.0, firstnode.Edges[2].Item2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Rajouter un chemin existant devrait lever une exception.")]
    public void AddNode_AddInValidEdgeTest()
    {
        GraphNode<string, double> node = new("EUR");
        GraphNode<string, double> otherNode = new("USD");
        graphNodeRepository.AddEdge(otherNode, 1.0, node);
        graphNodeRepository.AddEdge(otherNode, 1.0, node);
    }
}

