using System;

namespace AIContinuous;

public class Root
{
    public static double Bissction
    (   Func<double, double> function,
         double a, double b,
        double tol = 1e-4, int maxIter = 1000
    )
    {
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

        return c;
    }

    public static double FalsePosition
    (
        Func<double, double> function, 
        double a, double b,
        double tol = 1e-4, int maxIter = 1000
    )
    {
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

            // relative tolerance
            if ((b - a) < tol * 2.0)
                break;

            if (fc * fa > 0)
                a = c;
            else
                b = c;
        }

        return c;
    }

    public static double Newton
    (
        Func<double, double> function, 
        Func<double, double> derivate, 
        double x0,
        double tol = 1e-4, 
        int maxIter = 10000
    )
    {
        double x = x0;

        for (int i = 0; i < maxIter; i++)
        {   
            var f0 = function(x);
            var fd = derivate(x);
            x -= f0 / fd;

            // absolute tolerance
            if (Math.Abs(f0) < tol)
                break;
        }

        return x;
    }
}
