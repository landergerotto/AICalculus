using System;
using System.Collections.Generic;

namespace AIContinuous;

public class DiffEvoltuion
{
    protected int Npop                      { get; set; }
    protected int Dimension                 { get; set; }
    protected double Mutation               { get; set; }
    protected double Recombination          { get; set; }
    protected Func<double[], double> Fitness{ get; set; }
    protected List<double[]> Individuals    { get; set; }
    protected List<double[]> Bounds         { get; set; }
    protected int bestIndividualIndex       { get; set; }
    protected double bestIndividualFitness  { get; set; } = double.MaxValue;

    public DiffEvoltuion
    (
        Func<double[], double> fitness, 
        int npop, 
        List<double[]> bounds,
        double mutation = 0.7,
        double recombination = 0.8
    )
    {
        this.Fitness = fitness;
        this.Npop = npop;
        this.Dimension = bounds.Count;
        this.Individuals = new List<double[]>(Npop);
        this.Bounds = bounds;
        this.Mutation = mutation;
        this.Recombination = recombination;
    }

    private void GeneratePopulation()
    {
        var dim = this.Dimension;
        for (int i = 0; i < Npop; i++)
        {
            Individuals.Add( new double[dim] );
            for (int j = 0; j < dim; j++)
            {
                this.Individuals[i][j] = Utils.Rescale (
                    Random.Shared.NextDouble(), 
                    this.Bounds[j][0], 
                    this.Bounds[j][1]
                );
            }
        }  
    }

    private void FindBestIndividual()
    {
        var fitnessBest = bestIndividualFitness;
        for (int i = 0; i < Npop; i++)
        {
            var fitnessCurrent = Fitness(Individuals[i]);

            if (fitnessCurrent < fitnessBest)
            {
                bestIndividualIndex = i;
                fitnessBest = fitnessCurrent;
            }
        }
        bestIndividualFitness = fitnessBest;
    }

    private double[] Mutate(double[] individual)
    {
        var newIndiviual = new double[this.Dimension];
        newIndiviual = (double[])Individuals[bestIndividualIndex].Clone();

        var randomIndividual1 = Random.Shared.Next(Npop);
        int randomIndividual2;

        do
        {
            randomIndividual2 = Random.Shared.Next(Npop);
        } while (randomIndividual1 == randomIndividual2);
            

        for (int i = 0; i < this.Dimension; i++)
        {
            newIndiviual[i] += this.Mutation * (Individuals[randomIndividual1][i] 
                             - Individuals[randomIndividual2][i]);
        }

        return newIndiviual;
    }

    protected double[] Crossover(int index)
    {
        var trial2 = (double[])Individuals[index].Clone();
        var trial1 = Mutate(Individuals[index]);

        for (int i = 0; i < this.Dimension; i++)
        {
            if (!((Random.Shared.NextDouble() < this.Recombination) || (i == Random.Shared.Next(this.Dimension))))
                    trial2[i] = trial1[i];
        
        }

        return trial2;
    }

    protected void Iterate()
    {
        for (int i = 0; i < Npop; i++)
        {
            var trial = Crossover(i);

            if (Fitness(trial) < Fitness(Individuals[i]))
                Individuals[i] = trial; 
        }

        FindBestIndividual();
    }

    public double[] Optimize(int n)
    {
        GeneratePopulation();
        FindBestIndividual();

        for (int i = 0; i < n; i++)
            Iterate();

        return Individuals[bestIndividualIndex];
    }

}
