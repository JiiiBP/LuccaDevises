using System;
using System.Collections.Generic;
using LuccaDevisesModels;
using Moq;
using LuccaDevisesIRepositories;
using LuccaDevisesRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LuccaDevisesTESTS;

[TestClass]
public class GraphTests
{
    Graph<string, double> graph = new();
    static Mock<GraphNodeRepository> mockIGraphNodes = new();
    Mock<GraphRepository> mockGraphs = new(mockIGraphNodes.Object);

    [TestMethod]
    public void AddNode_AddNewValidNodeTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        Assert.IsTrue(graph.nodes.Count == 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Adding a node that currently does exist should throw an argument exception.")]
    public void AddNode_AddAlreadyExistingNodeTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("AUD", graph);
    }

    [TestMethod]
    public void AddPath_AddNewValidPathTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("CHF", graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Rajouter un chemin avec un premier noeud inexistant doit lever une exception.")]
    public void AddNode_AddInvalidFirstNodePathTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("ZZZ", "EUR", 1.0, graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Rajouter un chemin avec un dernier noeud inexistant doit lever une exception.")]
    public void AddNode_AddInvalidLastNodePathTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("JPY", "ZZZ", 1.0, graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Rajouter un chemin avec un dernier noeud et un premier noeud inexistants doit lever une exception.")]
    public void AddNode_AddInvalidFirstAndLastNodePathTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("ZZZ", "ZZZ", 1.0, graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Rajouter un chemin déjà existant doit lever une exception.")]
    public void AddNode_AddAlreadyExistingPathTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("ZZZ", "ZZZ", 1.0, graph);
        mockGraphs.Object.AddPath("ZZZ", "ZZZ", 1.0, graph);
    }

    [TestMethod]
    public void AddNode_AddValidSameNodePathButDifferentPathDataTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("CHF", graph);
        mockGraphs.Object.AddPath("AUD", "CHF", 1.0, graph);
        mockGraphs.Object.AddPath("AUD", "CHF", 1.5, graph);
    }

    [TestMethod]
    public void ExecBFS_ValidTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("CHF", graph);
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("KWU", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddNode("USD", graph);
        mockGraphs.Object.AddNode("INR", graph);

        mockGraphs.Object.AddPath("AUD", "CHF", 0.9661, graph);
        mockGraphs.Object.AddPath("CHF", "AUD", Math.Round(1.0 / 0.9661, 4), graph);
        mockGraphs.Object.AddPath("JPY", "KWU", 13.1151, graph);
        mockGraphs.Object.AddPath("KWU", "JPY", Math.Round(1.0 / 13.1151, 4), graph);
        mockGraphs.Object.AddPath("EUR", "CHF", 1.2053, graph);
        mockGraphs.Object.AddPath("CHF", "EUR", Math.Round(1.0 / 1.2053, 4), graph);
        mockGraphs.Object.AddPath("AUD", "JPY", 86.0305, graph);
        mockGraphs.Object.AddPath("JPY", "AUD", Math.Round(1.0 / 86.0305, 4), graph);
        mockGraphs.Object.AddPath("EUR", "USD", 1.2989, graph);
        mockGraphs.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), graph);
        mockGraphs.Object.AddPath("JPY", "INR", 0.6571, graph);
        mockGraphs.Object.AddPath("INR", "JPY", Math.Round(1.0 / 0.6571, 4), graph);

        List<Tuple<string, double>> result = mockGraphs.Object.ExecBFS("EUR", "JPY", graph);

        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("EUR", result[0].Item1);
        Assert.AreEqual("CHF", result[1].Item1);
        Assert.AreEqual("AUD", result[2].Item1);
        Assert.AreEqual("JPY", result[3].Item1);
        Assert.AreEqual(1.2053, result[1].Item2);
        Assert.AreEqual(1.0351, result[2].Item2);
        Assert.AreEqual(86.0305, result[3].Item2);
    }

    [TestMethod]
    public void ExecBFS_ValidCircularGraphTest()
    {
        mockGraphs.Object.AddNode("A", graph);
        mockGraphs.Object.AddNode("B", graph);
        mockGraphs.Object.AddNode("C", graph);
        mockGraphs.Object.AddNode("D", graph);
        mockGraphs.Object.AddNode("E", graph);
        mockGraphs.Object.AddNode("F", graph);
        mockGraphs.Object.AddNode("G", graph);

        mockGraphs.Object.AddPath("A", "B", 1, graph);
        mockGraphs.Object.AddPath("B", "A", 2, graph);
        mockGraphs.Object.AddPath("A", "C", 3, graph);
        mockGraphs.Object.AddPath("C", "A", 4, graph);
        mockGraphs.Object.AddPath("B", "D", 5, graph);
        mockGraphs.Object.AddPath("D", "B", 6, graph);
        mockGraphs.Object.AddPath("C", "E", 7, graph);
        mockGraphs.Object.AddPath("E", "C", 8, graph);
        mockGraphs.Object.AddPath("D", "F", 9, graph);
        mockGraphs.Object.AddPath("F", "D", 10, graph);
        mockGraphs.Object.AddPath("E", "F", 11, graph);
        mockGraphs.Object.AddPath("F", "E", 12, graph);
        mockGraphs.Object.AddPath("F", "G", 13, graph);
        mockGraphs.Object.AddPath("G", "F", 14, graph);
        mockGraphs.Object.AddPath("A", "D", 15, graph);
        mockGraphs.Object.AddPath("D", "A", 16, graph);
        mockGraphs.Object.AddPath("A", "E", 17, graph);
        mockGraphs.Object.AddPath("E", "A", 18, graph);

        List<Tuple<string, double>> result = mockGraphs.Object.ExecBFS("A", "G", graph);

        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("A", result[0].Item1);
        Assert.AreEqual("D", result[1].Item1);
        Assert.AreEqual("F", result[2].Item1);
        Assert.AreEqual("G", result[3].Item1);
        Assert.AreEqual(15, result[1].Item2);
        Assert.AreEqual(9, result[2].Item2);
        Assert.AreEqual(13, result[3].Item2);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Un chemin n'est pas trouvé, donc envoi d'exception.")]
    public void ExecBFS_ThrowIfNoPathWasFoundTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddNode("USD", graph);

        mockGraphs.Object.AddPath("EUR", "USD", 1.2989, graph);
        mockGraphs.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), graph);

        mockGraphs.Object.ExecBFS("JPY", "EUR", graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Un chemin est manquant, insolvable, donc envoi d'exception.")]
    public void ExecBFS_NoSolutionCircularGraphTest()
    {
        mockGraphs.Object.AddNode("A", graph);
        mockGraphs.Object.AddNode("B", graph);
        mockGraphs.Object.AddNode("C", graph);
        mockGraphs.Object.AddNode("D", graph);

        mockGraphs.Object.AddPath("A", "B", 1, graph);
        mockGraphs.Object.AddPath("B", "A", 2, graph);
        mockGraphs.Object.AddPath("A", "C", 3, graph);
        mockGraphs.Object.AddPath("C", "A", 4, graph);
        mockGraphs.Object.AddPath("B", "C", 5, graph);
        mockGraphs.Object.AddPath("C", "B", 6, graph);

        mockGraphs.Object.ExecBFS("A", "D", graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Un noeud n'est pas trouvé, donc envoi d'exception.")]
    public void ExecBFS_ThrowIfAnInvalidNodeIsProvidedTest()
    {
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddNode("USD", graph);
        
        mockGraphs.Object.AddPath("EUR", "USD", 1.2989, graph);
        mockGraphs.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), graph);
        mockGraphs.Object.ExecBFS("AUD", "EUR", graph);
    }

    [TestMethod]
    public void HasExistingNode_EqualTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("AUD", graph));
    }

    [TestMethod]
    public void HasExistingNode_NotEqualEmptyGraphTest()
    {
        Assert.AreNotEqual(true, mockGraphs.Object.HasExistingNode("AUD", graph));
    }

    [TestMethod]
    public void HasExistingNode_NotEqualTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        Assert.AreNotEqual(true, mockGraphs.Object.HasExistingNode("EUR", graph));
    }

    [TestMethod]
    public void HasExistingPath_EqualTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("AUD", "EUR", 1, graph);
        Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("AUD", "EUR", graph));
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Pas de chemin entre 2 noeuds, donc envoi d'exception.")]
    public void HasExistingPath_NotEqualMissingFirstNodeTest()
    {
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.HasExistingPath("AUD", "EUR", graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Pas de chemin entre 2 noeuds, donc envoi d'exception.")]
    public void HasExistingPath_NotEqualMissingSecondNodeTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("JPY", graph);
        mockGraphs.Object.HasExistingPath("AUD", "EUR", graph);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Pas de chemin entre 2 noeuds sur un graph vide, donc envoi d'exception.")]
    public void HasExistingPath_NotEqualEmptyGraphTest()
    {
        mockGraphs.Object.HasExistingPath("AUD", "EUR", graph);
    }

    [TestMethod]
    public void HasExistingPath_NotEqualTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        Assert.AreNotEqual(true, mockGraphs.Object.HasExistingPath("AUD", "EUR", graph));
    }

    [TestMethod]
    public void HasExistingPath_InversePathShouldNotEqualTest()
    {
        mockGraphs.Object.AddNode("AUD", graph);
        mockGraphs.Object.AddNode("EUR", graph);
        mockGraphs.Object.AddPath("EUR", "AUD", 1, graph);
        Assert.AreNotEqual(true, mockGraphs.Object.HasExistingPath("AUD", "EUR", graph));
    }
}