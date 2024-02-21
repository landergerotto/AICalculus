using System;

using AIContinuous;

double myFuction(double x)
{
    // return x + 1;
    // return Math.Sqrt(x) * x - 5*x + 10;
    return Math.Sqrt(x) - Math.Cos(x);
}

double NewtonFunction(double x)
{
    return x * x;
}

double NewtonDer(double x)
{
    return 2 * x;
}

var date = DateTime.Now;

date = DateTime.Now;

double sol = Root.Newton(NewtonFunction, NewtonDer, 1);

var diff = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
// See https://aka.ms/new-console-template for more information
Console.WriteLine(sol);
