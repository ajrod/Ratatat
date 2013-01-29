//Copyright (C) 2013 Alex Rodrigues
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this 
//software and associated documentation files (the "Software"), to deal in the Software without 
//restriction, including without limitation the rights to use, copy, modify, merge, publish, 
//distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom 
//the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or 
//substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
//ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
//SOFTWARE.

//Author: Alex Rodrigues

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ratatat
{

    /// <summary>
    /// The Engine for the genetic algorithm.
    /// </summary>
    public class GeneticAlgorithm
    {
        /// <summary>
        /// The number of chromosomes in the genome.
        /// </summary>
        private int NUMB_CHROMS;
        /// <summary>
        /// The number of bits used to encode a gene.
        /// </summary>
        private int BITS_PER_GENE;
        /// <summary>
        /// The crossover rate. This affects how often two parents will breed.
        /// </summary>
        private double CROSS_OVER_RATE;
        /// <summary>
        /// The mutation rate. This affects how often a mutation will occur in the bits of each gene.
        /// Mutation only occurs with new offspring.
        /// </summary>
        private double MUTATION_RATE;
        /// <summary>
        /// The size of the population of rats.
        /// </summary>
        public int POPULATION_SIZE;
        /// <summary>
        /// This determines the percentage of elite rats that has their genome preserved to the 
        /// next generation. 
        /// </summary>
        private double ELITISM;
        /// <summary>
        /// The total fitness score for the current generation.
        /// </summary>
        public double generationScore;
        /// <summary>
        /// The current rat in the population being tested.
        /// </summary>
        public int currentRat;
        /// <summary>
        /// The number of generations that has past.
        /// </summary>
        public int generation;
        /// <summary>
        /// The best fitness score seen so far in all the trials.
        /// </summary>
        public double bestScoreSeen;
        /// <summary>
        /// The number of elites rats that has their genome preserved to the next generation.
        /// </summary>
        private int numOfElites;
        /// <summary>
        /// The current generation's population of rats.
        /// </summary>
        private List<Rat> population;
        /// <summary>
        /// The last generation's population of rats. This is used as a backup incase the current generation needs to be reset.
        /// </summary>
        private List<Rat> backupPopulation;


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneticAlgorithm"/> class.
        /// </summary>
        /// <param name="bits_per_gene">The bits_per_gene.</param>
        /// <param name="numb_chroms">The numb_chroms.</param>
        /// <param name="pop_size">The pop_size.</param>
        /// <param name="cross_over_rate">The cross_over_rate.</param>
        /// <param name="mutation_rate">The mutation_rate.</param>
        public GeneticAlgorithm(int bits_per_gene, int numb_chroms, int pop_size, double cross_over_rate, double mutation_rate, double rate_of_elitism)
        {
            bestScoreSeen = 0;
            generation = 0;
            currentRat = 0;
            population = new List<Rat>();
            this.CROSS_OVER_RATE = cross_over_rate;
            this.MUTATION_RATE = mutation_rate;
            this.POPULATION_SIZE = pop_size;
            this.NUMB_CHROMS = numb_chroms;
            this.BITS_PER_GENE = bits_per_gene;
            this.ELITISM = rate_of_elitism;
            numOfElites = ((int)((POPULATION_SIZE * ELITISM) / 2)) * 2; //forces number of elites to be divisible by 2
            this.CreateStartingPop();
            backupPopulation = population;
        }

        /// <summary>
        /// Returns a randomly generated chromosome.
        /// </summary>
        /// <returns></returns>
        private String GetRandomChromosome(Random rand)
        {
            String current_code = String.Empty;
            int i, j;
            i = j = 0;
            while (j < NUMB_CHROMS)
            {
                while (i < BITS_PER_GENE)
                {
                    current_code += GetRandomBit(rand);
                    ++i;
                }
                i = 0;
                ++j;
            }
            return current_code;
        }

        /// <summary>
        /// Get a random bit.
        /// </summary>
        /// <param name="rand">An instance of random.</param>
        /// <returns></returns>
        private char GetRandomBit(Random rand)
        {
            if (rand.Next(2) == 1)
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }

        /// <summary>
        /// Creates the starting population of rats with randomly generated chromosomes.
        /// </summary>
        private void CreateStartingPop()
        {
            int ct = 0;
            Random r = new Random((int)DateTime.Now.Ticks);
            while (ct < POPULATION_SIZE)
            {
                Rat tmp = new Rat(MainForm.START_POSITION, 1, 1, 1);
                tmp.code = this.GetRandomChromosome(r);
                tmp.decoded = this.Decode(tmp.code);
                population.Add(tmp);
                ++ct;
            }
        }

        /// <summary>
        /// Decodes the rats chromosome.
        /// </summary>
        /// <param name="code">The code, the string of bits representing a series of directions.</param>
        /// <returns></returns>
        private String Decode(String code)
        {
            String decoded_path = String.Empty;
            String current_chrom = String.Empty;
            int i, j;
            i = j = 0;
            while (j < NUMB_CHROMS)
            {
                while (i < BITS_PER_GENE)
                {
                    current_chrom += code[i + 2 * j];
                    ++i;
                }

                decoded_path += BinToDecimal(current_chrom);
                current_chrom = string.Empty;
                i = 0;
                ++j;
            }
            return decoded_path;
        }

        /// <summary>
        /// Return the current rats in the population to be tested.
        /// </summary>
        /// <returns></returns>
        public Rat[] CurrentRats()
        {
            Rat[] rats = new Rat[MainForm.current_rats_per_trial];
            int ct = 0;
            while (ct < MainForm.current_rats_per_trial)
            {
                rats[ct] = population[currentRat + ct];
                ++ct;
            }
            return rats;
        }

        /// <summary>
        /// Sets next rat in the population to be tested.
        /// </summary>
        public bool Next()
        {
            currentRat += MainForm.current_rats_per_trial;
            if (currentRat == POPULATION_SIZE)
            {
                ScoreGeneration();
                List<Rat> new_population = new List<Rat>();
                String[] elites = GetElites(numOfElites);
                Random r = new Random((int)DateTime.Now.Ticks);
                if (numOfElites >= 2)
                {
                    BreedElites(r, new_population, elites);
                }
                String parent1, parent2;
                generation += 1;
                currentRat = 0;
                int ct = numOfElites;
                while (ct < POPULATION_SIZE)
                {
                    parent1 = RouletteWheelSelection(r);
                    parent2 = RouletteWheelSelection(r);
                    Breed(r, new_population, parent1, parent2);
                    ct += 2;
                }

                population.Clear();
                foreach (Rat rat in new_population)
                {
                    population.Add(rat);
                }
                new_population.Clear();
                backupPopulation = population;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a binary string to decimal.
        /// </summary>
        /// <param name="binaryString">The string of bits, a representation of a binary number.</param>
        /// <returns></returns>
        private String BinToDecimal(String binaryString)
        {
            int ct = 0;
            int total = 0;
            while (ct < binaryString.Length)
            {
                int tmp;
                int.TryParse(binaryString[ct].ToString(), out tmp);
                total += tmp * (int)Math.Pow(2, (binaryString.Length - ct - 1));
                ++ct;
            }
            return total.ToString();
        }

        /// <summary>
        /// Calculates the total net score of the population at the end of a generation.
        /// </summary>
        private void ScoreGeneration()
        {
            generationScore = 0;
            foreach (Rat rat in population)
            {
                generationScore += rat.score;
            }
        }

        /// <summary>
        /// Uses roulette wheel selection to select a genome from the population. This is a selection
        /// that gives a probability to each genome that is proportional to its fitness
        /// </summary>
        /// <param name="rand">The rand.</param>
        /// <returns></returns>
        private String RouletteWheelSelection(Random rand)
        {
            double spin = rand.NextDouble();
            double ct = 0;
            string code = string.Empty;
            foreach (Rat rat in population)
            {
                if (spin <= ((rat.score / generationScore) + ct))
                {
                    code = rat.code;
                    break;
                }
                else
                {
                    ct += (rat.score / generationScore);
                }
            }

            Debug.Assert(code != string.Empty);
            return code;
        }

        /// <summary>
        /// Breeds two parents to create two offspring.
        /// </summary>
        /// <param name="rand">The random a new instance of a Random.</param>
        /// <param name="parent1">The parent1's Genome.</param>
        /// <param name="parent2">The parent2's Genome.</param>
        /// <param name="baby1">The baby1, an offspring of parent1 and parent2's Genome.</param>
        /// <param name="baby2">The baby2, an offspring of parent1 and parent2's Genome.</param>
        private void CrossOver(Random rand, String parent1, String parent2, ref String baby1, ref String baby2)
        {
            if (rand.NextDouble() <= CROSS_OVER_RATE)
            {
                int total = NUMB_CHROMS * BITS_PER_GENE;
                int slice = rand.Next(total - 1) + 1;
                int ct = 0;
                while (ct < total)
                {
                    if (ct < slice)
                    {
                        baby1 += parent1[ct].ToString();
                        baby2 += parent2[ct].ToString();
                    }
                    else
                    {
                        baby1 += parent2[ct].ToString();
                        baby2 += parent1[ct].ToString();
                    }
                    ++ct;
                }
            }
            else
            {
                baby1 = parent1;
                baby2 = parent2;
            }
        }

        /// <summary>
        /// Mutates the specified genome by flipping bits randomly. The amount of mutation is based on
        /// the mutation rate.
        /// </summary>
        /// <param name="rand">The random object.</param>
        /// <param name="baby">The baby.</param>
        private void Mutate(Random rand, ref String baby)
        {
            String mutatedGenome = String.Empty;
            int ct = 0;
            while (ct < baby.Length)
            {
                if (rand.NextDouble() <= MUTATION_RATE)
                {
                    if (baby[ct] == '1')
                    {
                        mutatedGenome += '0';
                    }
                    else
                    {
                        mutatedGenome += '1';
                    }
                }
                else
                {
                    mutatedGenome += baby[ct];
                }
                ++ct;
            }
            baby = mutatedGenome;
        }

        /// <summary>
        /// Gets the genetic codes of all elite rats.
        /// </summary>
        /// <param name="numbOfElites">The total number of elites.</param>
        /// <returns></returns>
        private String[] GetElites(int numbOfElites)
        {
            Rat[] elites = new Rat[numbOfElites];
            String[] elite_codes = new String[numbOfElites];
            int current_elite = 0, ct, current_index;
            double current_elite_score;

            while (current_elite < numbOfElites)
            {
                current_elite_score = population[0].score;
                elites[current_elite] = population[0];
                elite_codes[current_elite] = population[0].code;
                current_index = 0;
                ct = 1;
                while (ct < POPULATION_SIZE - current_elite)
                {
                    if (population[ct].score > current_elite_score)
                    {
                        current_elite_score = population[ct].score;
                        elites[current_elite] = population[ct];
                        elite_codes[current_elite] = population[ct].code;
                        current_index = ct;
                    }
                    ++ct;
                }
                ++current_elite;
                population.RemoveAt(current_index);
            }

            ct = 0;
            while (ct < numbOfElites)
            {
                population.Add(elites[ct]);
                Debug.Assert(elites[ct].code.Length > 0);
                ++ct;
            }
            Debug.Assert(elite_codes.Length == numbOfElites);
            return elite_codes;
        }

        /// <summary>
        /// Breed two parent rats to produce offspring.
        /// </summary>
        /// <param name="rand">The random object.</param>
        /// <param name="newPopulation">The new population.</param>
        /// <param name="parent1">The genes of parent1.</param>
        /// <param name="parent2">The genes of parent2.</param>
        private void Breed(Random rand, List<Rat> newPopulation, String parent1, String parent2)
        {
            String baby1 = string.Empty, baby2 = string.Empty;
            Rat babyRat1, babyRat2;
            //apply cross over to the offspring
            CrossOver(rand, parent1, parent2, ref baby1, ref baby2);
            //mutate the bits for the offspring
            Mutate(rand, ref baby1);
            Mutate(rand, ref baby2);

            //create the rat objects
            babyRat1 = new Rat(MainForm.START_POSITION, 1, 1, 1);
            babyRat1.code = baby1;
            babyRat1.decoded = Decode(baby1);
            babyRat2 = new Rat(MainForm.START_POSITION, 1, 1, 1);
            babyRat2.code = baby2;
            babyRat2.decoded = Decode(baby2);
            newPopulation.Add(babyRat1);
            newPopulation.Add(babyRat2);
        }

        /// <summary>
        /// Breeds the elites. This automatically preserves their genetic makeup to the next generation.
        /// </summary>
        /// <param name="rand">The random object.</param>
        /// <param name="newPopulation">The new population.</param>
        /// <param name="codes">The genetic codes.</param>
        private void BreedElites(Random rand, List<Rat> newPopulation, String[] codes)
        {
            //Automatically add the top rat into the next population
            Rat rat1 = new Rat(MainForm.START_POSITION, 1, 1, 1);
            rat1.code = codes[0];
            rat1.decoded = Decode(codes[0]);
            newPopulation.Add(rat1);

            //Automatically add the second top rat into the next population

            Rat rat2 = new Rat(MainForm.START_POSITION, 1, 1, 1);
            rat2.code = codes[1];
            rat2.decoded = Decode(codes[1]);
            newPopulation.Add(rat2);
            int ct = 2;
            //Breed the top two rats
            if (numOfElites >= 4)
            {
                Breed(rand, newPopulation, codes[0], codes[1]);
                ct += 2;
            }

            //Randomly select the rest of the elites in pairs
            while (ct < codes.Length)
            {
                Breed(rand, newPopulation, codes[rand.Next(codes.Length)], codes[rand.Next(codes.Length)]);
                ct += 2;
            }
        }

        /// <summary>
        /// Sets the current population to the population of the previous generation.
        /// </summary>
        public void RetroPopulation()
        {
            population = backupPopulation;
        }
    }
}