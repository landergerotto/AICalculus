using System;
using System.Collections.Generic;

namespace AIContinuous;

public class DiffEvoltuion
{
    protected int Npop                          { get; set; }
    protected int Dimension                     { get; set; }
    protected double[] Mutation                 { get; set; } = new double[2];
    protected double Recombination              { get; set; }
    protected Func<double[], double> Fitness    { get; set; }
    protected Func<double[], double> Restriction{ get; set; }
    protected List<double[]> Individuals        { get; set; }
    protected List<double[]> Bounds             { get; set; }
    protected int bestIndividualIndex           { get; set; }

    private double[] IndividualsRestrictions    { get; set; }
    private double[] IndividualsFitness         { get; set; }

    public DiffEvoltuion
    (
        Func<double[], double> fitness, 
        List<double[]> bounds,
        int npop, 
        Func<double[], double> restriction,
        double mutationMin = 0.5,
        double mutationMax = 0.9,
        double recombination = 0.8
    )
    {
        this.Fitness = fitness;
        this.Bounds = bounds;
        this.Npop = npop;
        this.Restriction = restriction;
        this.Dimension = bounds.Count;
        this.Individuals = new List<double[]>(Npop);
        this.Mutation[0] = mutationMin;
        this.Mutation[1] = mutationMax;
        this.Recombination = recombination;
        this.IndividualsRestrictions = new double[Npop];
        this.IndividualsFitness = new double[Npop];

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

            IndividualsRestrictions[i] = Restriction(Individuals[i]);
        

            IndividualsFitness[i] = IndividualsRestrictions[i] <= 0.0 
                                    ? Fitness(Individuals[i]) 
                                    : double.MaxValue;

        }  
    }

    private void FindBestIndividual()
    {
        var fitnessBest = this.IndividualsFitness[bestIndividualIndex];

        for (int i = 0; i < Npop; i++)
        {
            var fitnessCurrent = Fitness(Individuals[i]);

            if (fitnessCurrent < fitnessBest)
            {
                bestIndividualIndex = i;
                fitnessBest = fitnessCurrent;
            }
        }
        this.IndividualsFitness[bestIndividualIndex] = fitnessBest;
    }

    private double[] Mutate(int index)
    {
        int randomIndividual1;
        int randomIndividual2;

        do randomIndividual1 = Random.Shared.Next(Npop);
        while (randomIndividual1 == index); 

        do randomIndividual2 = Random.Shared.Next(Npop);
        while (randomIndividual1 == randomIndividual2); 

        var newIndiviual = (double[])Individuals[bestIndividualIndex].Clone();
        for (int i = 0; i < this.Dimension; i++)
        {
            newIndiviual[i] += Utils.Rescale(Random.Shared.NextDouble(), Mutation[0], Mutation[1]) * 
                                (Individuals[randomIndividual1][i] - Individuals[randomIndividual2][i]);
        }

        return newIndiviual;
    }

    protected double[] Crossover(int index)
    {
        var trial1 = Mutate(index);
        var trial2 = (double[]) Individuals[index].Clone();

        for (int i = 0; i < this.Dimension; i++)
        {
            if (!(
                Random.Shared.NextDouble() < this.Recombination || 
                i == Random.Shared.Next(this.Dimension)
               ))
                    trial2[i] = trial1[i];
        }

        return trial2;
    }

    protected void Iterate()
    {
        for (int i = 0; i < Npop; i++)
        {
            var trial = Crossover(i);

            var restTrial = Restriction(trial);
            // var fitnessTrial = Fitness(trial);
            double fitnessTrial = restTrial <= 0.0 ? Fitness(trial) : double.MaxValue;

            var restIndividual = IndividualsRestrictions[i];

            if (
                ((restIndividual > 0.0) && 
                 (restTrial < restIndividual))

                || ((restTrial <= 0.0) &&
                   (restIndividual > 0.0)) 

                || ((restTrial <= 0.0) &&
                   (fitnessTrial < IndividualsFitness[i]))
               )
               {
                Individuals[i] = trial;

                IndividualsRestrictions[i] = restTrial;
               }
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
