using System;
using System.Threading;

namespace AIContinuous;

public static class Optimize
{
    public static double Newton
    (
        Func<double, double> func, 
        double x0,
        double h = 1e-2,
        double tol = 1e-4,
        int maxIter = 10_000
    )
    {
        Func<double, double> diffFunction = x 
            => Diff.Differentiate3P(func, x, h);

        Func<double, double> diffSecondFunction = x 
            => Diff.Differentiate3P(diffFunction, x, h);

        return Root.Newton(diffFunction, diffSecondFunction, x0, tol, maxIter);
    }

    public static double DescendentGradient
    (
        Func<double, double> func, 
        double x0,
        double learningRate = 1e-2, // LR lr Lr lR
        double tol = 1e-4
    )
    {
        double xp = x0;
        double diff = Diff.Differentiate3P(func, xp);

        while (Math.Abs(diff) > tol)
        {
            diff = Diff.Differentiate3P(func, xp);
            xp -= learningRate * diff;
        }

        return xp;
    }

    public static double[] DescendentGradient
    (
        Func<double[], double> func, 
        double[] x0,
        double learningRate = 1e-2, // LR lr Lr lR
        double tol = 1e-4
    )
    {
        var dim = x0.Length;
        var xp = (double [])x0.Clone();

        double diffNorm;

        var diff = Diff.Gradient(func, xp);

        do
        {   
            diffNorm = 0.0;

            diff = Diff.Gradient(func, xp);

            for (int i = 0; i < dim; i++)
            {
                xp[i]    -= learningRate * diff[i];
                diffNorm += Math.Abs(diff[i]);
            }

            
        } while (diffNorm > tol * dim);

        return xp;
    }

    public static double Maximum
    (
        Func<double, double> func, 
        double h = 1e-2,
        double tol = 1e-4,
        int maxIter = 10_000
    )
    {
        Func<double, double> diffFunction = x 
            => Diff.Differentiate3P(func, x, h);

        Func<double, double> diffSecondFunction = x 
            => Diff.Differentiate3P(diffFunction, x, h);

        double[] allValues = new double[10_000];

        double max = Double.MinValue;

        for (int i = 0; i < 10_000; i++)
        {
            double guess = Random.Shared.NextInt64(-1_000_000, 1_000_000);
            double value = Root.Newton(diffFunction, diffSecondFunction, guess, tol, maxIter);

            if (value > max)
                max = value;
        }

        return max;
    }
    public static double Minimum
    (
        Func<double, double> func, 
        double h = 1e-2,
        double tol = 1e-4,
        int maxIter = 10_000
    )
    {
        Func<double, double> diffFunction = x 
            => Diff.Differentiate3P(func, x, h);

        Func<double, double> diffSecondFunction = x 
            => Diff.Differentiate3P(diffFunction, x, h);

        double[] allValues = new double[10_000];

        double min = Double.MaxValue;

        for (int i = 0; i < 10_000; i++)
        {
            double guess = Random.Shared.NextInt64(-1_000_000, 1_000_000);
            double value = Root.Newton(diffFunction, diffSecondFunction, guess, tol, maxIter);

            if (value < min)
                min = value;
        }

        return min;
    }
}
