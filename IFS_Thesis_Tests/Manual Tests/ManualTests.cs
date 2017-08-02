using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Ifs.IFSGenerators;
using IFS_Thesis.Utils;
using IFS_Thesis_Tests.Properties;
using MoreLinq;
using NUnit.Framework;

namespace IFS_Thesis_Tests.Manual_Tests
{
    /// <summary>
    /// Class for testing some functionality manually
    /// </summary>
    [TestFixture]
    public class ManualTests
    {

        #region Tests

        [Test, Category("Manual"), Ignore("Manual Test")]
        [Apartment(ApartmentState.STA)]
        public void TestFitnessForIndividual()
        {
            var imageFolder = Settings.Default.WorkingDirectory;
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();
            var multiplier = 32;
            var dimensionX = 128;
            var dimensionY = 128;
            var dimensionZ = 128;

            var sourceIndividual = EaUtils.CreateIndividualFromSingelsString("[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0.5,0];[0.5,0,0,0,0.5,0,0,0,0.5,0.5,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0.5,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0,0]");

            var sourceVoxels = ifsGenerator.GenerateVoxelsForIfs(sourceIndividual.Singels, dimensionX, dimensionY, dimensionZ, multiplier);

            var fitnesses  = new List<float>();
            //var evolvedIndividual = CreateIndividualFromSingelsString("[0.6,0,0,0,0.7,0,0,0.1,0.5,0,0,0,0];[0.6,0,0,0,0.54,0,0,0,0.3,0.5,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0.7,0,0];[0.5,0,0,0,0.5,0,0,0.1,0.5,0,0.2,0.5,0]");
            var evolvedIndividual = EaUtils.CreateIndividualFromSingelsString("[0.6149,0.2856,-0.2272,0.18,0.2219,0.5073,-0.1698,0.4266,-0.1073,0.7594,-0.4679,-0.0044,0];[0.7991,0.3926,0.4658,0.0969,0.0288,-0.073,-0.047,0.7115,-0.0625,1.9231,-1.7239,0,0];[0.4061,0.1446,-0.3063,0.2224,0.1938,0.5675,-0.22,0.1806,-0.05105287,-0.1838,0.3627,0,0];[0.4685,0.3779,-0.1305,0.093,0.1718,0.306,0.5228,-0.0211,-0.1451,0.8557,-1.1848,0,0];[0.3827,0.3162,-0.1707,0.1098,0.1624,0.3576,0.5482,0.1271,-0.0796,0.5697,-1.2266,0,0];[0.7213,0.4016,-0.0277,0.0357,0.2602,0.3585,0.0085,0.3269,-0.1475,1.2998,-0.9138,0,0];[0.7915,0.3533,0.5438,0.1167,0.0466,-0.0714,-0.053,0.7019,-0.0707,1.6766,-1.6893,0,0];[0.7504,0.3597,0.5969,0.128,0.0336,-0.137,-0.0591,0.6903,-0.0735,1.6788,-1.5468,0,0]");

            for (int i = 0; i < 10; i++)
            {
                var fitness = new WeightedPointsCoverageObjectiveFitnessFunction().CalculateFitnessForIndividual(sourceVoxels, evolvedIndividual, ifsGenerator, dimensionX, dimensionY, dimensionZ, multiplier);
                fitnesses.Add(fitness);
            }
            
            Assert.That(fitnesses, Is.Not.Null);

            if (true)
            {
                var generatedVoxels = ifsGenerator.GenerateVoxelsForIfs(evolvedIndividual.Singels, dimensionX, dimensionY, dimensionZ, multiplier);
                var matchingVoxels = generatedVoxels.Intersect(sourceVoxels).ToHashSet();
                var voxelsDrawnOutside = generatedVoxels.Except(matchingVoxels).ToHashSet();

                ifsDrawer.SaveVoxelImage(imageFolder + "/source_voxels", sourceVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/generated_voxels", generatedVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/matching_voxels", matchingVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/voxels_drawn_outside", voxelsDrawnOutside, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelOverlayImage(imageFolder + "/voxels_overlay", sourceVoxels, generatedVoxels, ImageFormat3D.Obj);
            }
        }

        [Test, Category("Manual"), Ignore("Manual")]
        [Apartment(ApartmentState.STA)]
        public void GenerateImagesFromPopulationLogs()
        {
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();
            var multiplier = 32;
            var dimensionX = 128;
            var dimensionY = 128;
            var dimensionZ = 128;

            var txtFilepath = Settings.Default.WorkingDirectory + "/population.txt";
            var imagesFolderPath = Settings.Default.WorkingDirectory +
                                   "/testPopulation";
            Directory.CreateDirectory(imagesFolderPath);

            var populationString = File.ReadAllText(txtFilepath);

            var allIndividuals = EaUtils.CreateIndividualsFromPopulationString(populationString);

            allIndividuals = allIndividuals.OrderByDescending(x => x.ObjectiveFitness).ToList();

            for (var index = 0; index < allIndividuals.Count; index++)
            {
                var individual = allIndividuals[index];
                var generatedVoxels = ifsGenerator.GenerateVoxelsForIfs(individual.Singels, dimensionX, dimensionY,
                    dimensionZ, multiplier);
                ifsDrawer.SaveVoxelImage(
                    imagesFolderPath +
                    $"/individual{index + 1}_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}",
                    generatedVoxels, ImageFormat3D.Stl);
            }
        }

        [Test, Category("Manual"), Ignore("Manual Test")]
        public void TestCloningOfIndividual()
        {
            var individual = EaUtils.CreateIndividualFromSingelsString("[0.3194,0.7139,0.5867,0.9837722,0.6427,0.5526701,0.0483,0.0322,0.0079,1.6835,2.862625,3.3435,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,-9.081965,7.8529,0];[0.7322,0.9465162,0.8317,0.7455,0.4996,-0.748,0.0454,0.0021,-0.0087,8.103401,3.1998,-1.2937,0];[0.0285,0.0434,0.0665,0.223,0.04479644,-0.2702,0.0454,-0.0063,0.0081,5.6749,3.0677,2.9059,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,3.7962,7.8529,0]");
            var clone = (Individual) individual.Clone();

            Assert.That(individual.GetHashCode(), Does.Not.EqualTo(clone.GetHashCode()));
        }


        [Test, Category("Manual")]
        public void TestLinearRanking()
        {
            var individual1 = new Individual { ObjectiveFitness = 0.9f };
            var individual2 = new Individual { ObjectiveFitness = 0.1f };
            var individual3 = new Individual { ObjectiveFitness = 1.2f };
            var individual4 = new Individual { ObjectiveFitness = 0.3f };
            var individual5 = new Individual { ObjectiveFitness = 0.001f };

            var testPopulation = new List<Individual> { individual1, individual2, individual3, individual4, individual5 };
            var expectedRanked = new List<Individual> {individual5, individual2, individual4, individual1, individual3};

            var rankedPopulation = new LinearRankingFitnessFunction().AssignRankingFitnessToIndividuals(testPopulation,
                1.5f);

            Assert.That(rankedPopulation.OrderBy(i => i.RankFitness).ToList(), Is.EqualTo(expectedRanked));
        }

        #endregion
    }
}
