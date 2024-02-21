using System;

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

        Root.Newton(diffFunction, diffSecondFunction, x0, tol, maxIter);

        return 0.0;
    }
}