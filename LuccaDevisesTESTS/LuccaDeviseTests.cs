using LuccaDevisesIRepositories;
using LuccaDevisesModels;
using LuccaDevisesRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LuccaDevisesTESTS;

[TestClass]
public class LuccaDeviseTests
{
    private string DATA_FOLDER_PATH_TEST = Directory.GetCurrentDirectory() + @"\JeuxDeTests";

    Mock<FileTraitmentRepository> mockFileT = new();
    static Mock<GraphNodeRepository> mockIGraphNodes = new();
    Mock<GraphRepository> mockGraphs = new(mockIGraphNodes.Object);

    [TestMethod]
    [ExpectedException(typeof(Exception), "Chaque fichier doit lever une exception différente en fonction des cas énumérés dans les titres de fichier.")]
    public void ConvertCurrency_InvalidFileShouldThrow()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        int NbExceptions = 0;
        var MyInvalidFiles = files.Where(a => a.Contains("invalid")).ToList();
        MyInvalidFiles.Add($"{DATA_FOLDER_PATH_TEST}/UnFichierQuiNexistePas.txt");
        foreach (string file in MyInvalidFiles)
        {
            try
            {
                BusinessClassLuccaRepository mockFileBlR = new(mockGraphs.Object, mockFileT.Object);
                mockFileBlR.ConvertCurrency($"{file}");
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
    [ExpectedException(typeof(Exception), "Il n'y a pas de conversion possible, throw exception.")]
    public void ConvertCurrency_NoConversionPathShouldThrow()
    {
        string file = @"\validGraphNoConversionPath.txt";
        string fullPath = $"{DATA_FOLDER_PATH_TEST}{file}";
        BusinessClassLuccaRepository mockFileBlR = new(mockGraphs.Object, mockFileT.Object);

        mockFileBlR.ConvertCurrency(fullPath);
    }

    [TestMethod]
    public void ConvertCurrency_ValidFileShouldConvert()
    {
        string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
        BusinessClassLuccaRepository mockFileBlR = new(mockGraphs.Object, mockFileT.Object);
        foreach (string file in files.Where(a => a.Contains("validExampleGraph.txt") || a.Contains("validExampleGraphWithEndingNewLine.txt")))
        {
            int result = mockFileBlR.ConvertCurrency(file);
            Assert.AreEqual(59033, result);
        }
    }
}


