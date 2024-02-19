using System;
using AIContinuous;

double myFuction(double x)
{
    // return x + 1;
    // return Math.Sqrt(x) * x - 5*x + 10;
    return Math.Sqrt(x) - Math.Cos(x);
}

double sol = Root.Bissction(myFuction, 0, 4);

// See https://aka.ms/new-console-template for more information
Console.WriteLine(sol);
