using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public static class Program
    {
        private static readonly Random rand = new Random();
        private static void Main()
        {
            var param = new Dictionary<string, int> {
                {"Dim", 81 },
                {"EliteSize", 5 },
                {"ChildSize", 6 },
                {"PopSize", 11 },
                {"Crossover", 50 },
                {"Mutation", 4 },
                {"MaxIteration", 50 }
            };
            OneMaxGA(param);
        }
        private static int OneMax(bool[] gene)
        {
            return gene.Count(x => x);
        }
        private static void OneMaxGA(Dictionary<string, int> param)
        {
            var pop = Enumerable.Range(0, param["EliteSize"])
                .Select(_ => Enumerable.Range(0, param["Dim"])
                    .Select(__ => rand.NextDouble() < 0.5)
                    .ToArray())
                .Select(x => new { Fitness = OneMax(x), Gene = x })
                .OrderByDescending(x => x.Fitness)
                .ToArray();
            Console.WriteLine(pop[0].Fitness);
            for (var iteration = 2; iteration < param["MaxIteration"]; iteration++)
            {
                var chiildGene = Enumerable.Range(0, param["ChildSize"] / 2)
                    .SelectMany(_ => CrossOver(
                        (bool[])pop[rand.Next(param["EliteSize"])].Gene.Clone(),
                        (bool[])pop[rand.Next(param["EliteSize"])].Gene.Clone(),
                        param["Crossover"] / 100.0))
                    .Select(x => Mutation(x, param["Mutation"]))
                    .Select(x => new { Fitness = OneMax(x), Gene = x });
                pop = pop
                    .Concat(chiildGene)
                    .OrderByDescending(x => x.Fitness)
                    .Take(param["EliteSize"])
                    .ToArray();
                Console.WriteLine(pop[0].Fitness);
            }
        }
        private static bool[][] CrossOver(bool[] gene1, bool[] gene2, double cr)
        {
            var crossOverPoints = Enumerable.Range(0, gene1.Length)
                .OrderBy(i => Guid.NewGuid())
                .Take(rand.PoissonRand(gene1.Length * cr));
            foreach (var i in crossOverPoints)
            {
                (gene1[i], gene2[i]) = (gene2[i], gene1[i]);
            }
            return new bool[][] { gene1, gene2 };
        }
        private static bool[] Mutation(bool[] gene, double mu)
        {
            var trues = Enumerable.Range(0, gene.Length).Where(i => gene[i]);
            var falses = Enumerable.Range(0, gene.Length).Where(i => !gene[i]);
            gene = Change(gene, mu, trues);
            gene = Change(gene, mu, falses);
            return gene;
        }
        private static bool[] Change(bool[] gene, double mu, IEnumerable<int> vs)
        {
            var mutationPoints = vs
                .OrderBy(i => Guid.NewGuid())
                .Take(rand.PoissonRand(mu));
            foreach (var i in mutationPoints)
            {
                gene[i] = !gene[i];
            }
            return gene;
        }
    }
    public static class RandomExtensions
    {
        public static int PoissonRand(this Random rand, double lambda)
        {
            double p = 1.0, L = Math.Exp(-lambda);
            int k = 0;
            while (p > L)
            {
                k++;
                p *= rand.NextDouble();
            }
            return k - 1;
        }
    }
}
