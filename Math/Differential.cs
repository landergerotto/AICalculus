using System;

namespace AIContinuous;

public static class Diff
{
    // x1 = x + h
    // x0 = x
    //erro e/h + h/M 
    public static double Differentiate3P(Func<double, double> function, double x, double h = 1e-2)
        => (
              function(x + h) 
            - function(x - h)
           ) / (2.0 * h);
    
    public static double Differentiate5P(Func<double, double> function, double x, double h = 1e-2)
        => (
            function(x - 2.0 * h) 
            - 8.0 * function(x - h) 
            + 8.0 * function(x + h) 
            - function(x + 2.0 * h)
            ) / (12.0 * h);
}