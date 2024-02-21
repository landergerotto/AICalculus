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
    // return x * x;
    return (Math.Sqrt(Math.Abs(x)) - 5) * x + 10;
}

var date = DateTime.Now;

date = DateTime.Now;
double sold = Optimize.Newton(NewtonFunction, 1.0);
var diff = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
Console.WriteLine(sold);
