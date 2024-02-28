using System;

public static class Integrate
{
    public static double Romberg(double[] x, double[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("x and y arrays must have the same length");

        var len = x.Length - 1;
        double sum = 0;

        for (int i = 0; i < len; i++)
            sum += 0.5 * (y[i] + y[i + 1]) * (x[i + 1] - x[i]);
        
        return 0;
    }
}