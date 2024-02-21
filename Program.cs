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

double NewtonDer(double x)
{
    // return 2 * x;
    return (1/(2*Math.Sqrt(Math.Abs(x)))) * x + (Math.Sqrt(Math.Abs(x)) - 5);
}

var date = DateTime.Now;

date = DateTime.Now;
double sola = Root.Bissction(NewtonFunction, 0, 4);
var diff0 = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff0.TotalMilliseconds + " Miliseconds");

date = DateTime.Now;
double solb = Root.FalsePosition(NewtonFunction, 0, 10);
var diff1 = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff1.TotalMilliseconds + " Miliseconds");

date = DateTime.Now;
double solc = Root.Newton(NewtonFunction, NewtonDer, 10);
var diff2 = DateTime.Now - date;

date = DateTime.Now;
double sold = Root.Newton(NewtonFunction, (double x) => Diff.Differentiate3P(NewtonFunction, x), 10);
var diff = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
Console.WriteLine(sold);
