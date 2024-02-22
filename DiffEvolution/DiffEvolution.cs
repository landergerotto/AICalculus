using System;
using System.Collections.Generic;

namespace AIContinuous;

public class DiffEvoltuion
{
    protected int Npop                      { get; set; }
    protected int Dimension                 { get; set; }
    protected Func<double[], double> fitness{ get; set; }
    protected List<double[]> individuals    { get; set; }
    protected List<double[]> Bounds         { get; set; }
    protected int bestIndividualIndex       { get; set; }
    protected double bestIndividualFitness  { get; set; } = double.MaxValue;

    public DiffEvoltuion(Func<double[], double> fitness, int npop, List<double[]> bounds)
    {
        this.Npop = npop;
        this.Dimension = bounds.Count;
        this.individuals = new List<double[]>(Npop);
        this.Bounds = bounds;
    }

    private void GeneratePopulation()
    {
        var dim = this.Dimension;
        for (int i = 0; i < Npop; i++)
        {
            individuals[i] = new double[dim];
            for (int j = 0; j < dim; j++)
            {
                this.individuals[i][j] = Utils.Rescale (
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
            var fitnessCurrent = fitness(individuals[i]);

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
        newIndiviual = individuals[bestIndividualIndex];
        for (int i = 0; i < this.Dimension; i++)
        {
            newIndiviual[i] += individuals[Random.Shared.Next(Npop)][i] 
                             - individuals[Random.Shared.Next(Npop)][i];
        }

        return newIndiviual;
    }

    public double[] Optimize()
    {
        GeneratePopulation();
        FindBestIndividual();

        return individuals[bestIndividualIndex];
    }

}
