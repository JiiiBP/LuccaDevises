using System;
using System.Collections.Generic;
using LuccaDevises;
using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using LuccaDevisesRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LuccaDevisesTESTS;

[TestClass]
public class FileTraitmentTests
{
    private string DATA_FOLDER_PATH_TEST = Directory.GetCurrentDirectory() + @"\JeuxDeTests";

    Mock<FileTraitmentRepository> mockFileT = new();
    static Mock<GraphNodeRepository> mockIGraphNodes = new();
    Mock<GraphRepository> mockGraphs = new(mockIGraphNodes.Object);

    [TestMethod]
    [ExpectedException(typeof(Exception), "Chaque fichier doit lever une exception différente en fonction des cas énumérés dans les titres de fichier.")]
    public void FileParse_InvalidFileShouldThrow()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        int NbExceptions = 0;
        var MyInvalidFiles = files.Where(a => a.Contains("invalid")).ToList();
        MyInvalidFiles.Add("UnFichierQuiNexistePas.txt");
        foreach (string file in MyInvalidFiles)
        {
            try
            {
                FileTraitment fileTraitment = new($"{file}", mockFileT.Object);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NbExceptions++;
                throw;
            }
        }
        Assert.AreEqual(NbExceptions, 39);        
    }

    [TestMethod]
    public void FileParser_ValidFileParsedGraphIsCorrect()
    {

        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        foreach (string file in files.Where(a => a.Contains("validExampleGraph.txt") || a.Contains("validExampleGraphWithEndingNewLine.txt")))
        {
            FileTraitment ft = new($"{file}", mockFileT.Object);

            Graph<string, double> MyGraph = mockGraphs.Object.CreateGraph(ft.From, ft.To, ft.exchangePaths);
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("AUD", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("CHF", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("JPY", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("KRW", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("INR", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("EUR", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingNode("USD", MyGraph));

            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("CHF", "AUD", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("AUD", "CHF", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("AUD", "JPY", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("JPY", "AUD", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("JPY", "KRW", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("KRW", "JPY", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("JPY", "INR", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("INR", "JPY", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("CHF", "EUR", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("EUR", "CHF", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("USD", "EUR", MyGraph));
            Assert.AreEqual(true, mockGraphs.Object.HasExistingPath("EUR", "USD", MyGraph));

            Assert.AreEqual(0.9661, mockGraphs.Object.ExecBFS("AUD", "CHF", MyGraph)[1].Item2);
            Assert.AreEqual(1.0351, mockGraphs.Object.ExecBFS("CHF", "AUD", MyGraph)[1].Item2);
            Assert.AreEqual(0.0116, mockGraphs.Object.ExecBFS("JPY", "AUD", MyGraph)[1].Item2);
            Assert.AreEqual(0.0762, mockGraphs.Object.ExecBFS("KRW", "JPY", MyGraph)[1].Item2);
            Assert.AreEqual(0.6571, mockGraphs.Object.ExecBFS("JPY", "INR", MyGraph)[1].Item2);
            Assert.AreEqual(1.5218, mockGraphs.Object.ExecBFS("INR", "JPY", MyGraph)[1].Item2);
            Assert.AreEqual(0.8297, mockGraphs.Object.ExecBFS("CHF", "EUR", MyGraph)[1].Item2);
            Assert.AreEqual(1.2053, mockGraphs.Object.ExecBFS("EUR", "CHF", MyGraph)[1].Item2);
            Assert.AreEqual(0.7699, mockGraphs.Object.ExecBFS("USD", "EUR", MyGraph)[1].Item2);
            Assert.AreEqual(1.2989, mockGraphs.Object.ExecBFS("EUR", "USD", MyGraph)[1].Item2);
            Assert.AreEqual(13.1151, mockGraphs.Object.ExecBFS("JPY", "KRW", MyGraph)[1].Item2);
            Assert.AreEqual(86.0305, mockGraphs.Object.ExecBFS("AUD", "JPY", MyGraph)[1].Item2);
        }

    }

    [TestMethod]
    public void FileParser_ValidFileParsedFromCurrencyIsCorrect()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        foreach (string file in files.Where(a => a.Contains("validExampleGraph.txt") || a.Contains("validExampleGraphWithEndingNewLine.txt")))
        {
            FileTraitment ft = new($"{file}", mockFileT.Object);
            Assert.AreEqual("EUR", ft.From);
        }
    }

    [TestMethod]
    public void FileParser_ValidFileParsedToCurrencyIsCorrect()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        foreach (string file in files.Where(a => a.Contains("validExampleGraph.txt") || a.Contains("validExampleGraphWithEndingNewLine.txt")))
        {
            FileTraitment ft = new($"{file}", mockFileT.Object);
            Assert.AreEqual("JPY", ft.To);
        }
    }

    [TestMethod]
    public void FileParser_ValidFileParsedInitialAmountIsCorrect()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        foreach (string file in files.Where(a => a.Contains("validExampleGraph.txt") || a.Contains("validExampleGraphWithEndingNewLine.txt")))
        {
            FileTraitment ft = new($"{file}", mockFileT.Object);
            Assert.AreEqual(550, ft.Source);
        }
    }
}

