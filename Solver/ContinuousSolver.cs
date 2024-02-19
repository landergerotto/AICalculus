using System;

namespace AIContinuous;

public class Root
{
    public static double Bissction
    (   Func<double, double> function, double a, double b,
        double tol = 1e-4, int maxIter = 1000
    )
    {
        var dt0 = DateTime.Now;
        double c = 0;

        for (int i = 0; i < maxIter; i++)
        {
            c = (a + b) / 2.0;
            var fc = function(c);

            // absolute tolerance
            if (Math.Abs(fc) < tol)
                break;

            // relative tolerance
            if ((b - a) < tol * 2.0)
                break;

            if (fc * function(a) > 0)
                a = c;
            else
                b = c;
            
        }
        var dt1 = DateTime.Now;
        var diff = dt1 - dt0;

        Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");

        return c;
    }

    public static double FalsePosition
    (
        Func<double, double> function, double a, double b,
        double tol = 1e-4, int maxIter = 1000
    )
    {
        var dt0 = DateTime.Now;
        double c = 0;

        for (int i = 0; i < maxIter; i++)
        {
            var fa = function(a);
            var fb = function(b);
            c = a - fa*(b - a)/(fb - fa);
            
            var fc = function(c);

            // absolute tolerance
            if (Math.Abs(fc) < tol)
                break;

            if (b - a < tol * 2.0)
                break;

            if (fc * fa > 0)
                a = c;
            else
                b = c;
        }
        var dt1 = DateTime.Now;
        var diff = dt1 - dt0;

        Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");

        return c;
    }
}
