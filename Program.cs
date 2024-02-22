using System;

using AIContinuous;

double myFuction(double x)
{
    // return x + 1;
    // return Math.Sqrt(x) * x - 5*x + 10;
    return Math.Sqrt(x) - Math.Cos(x);
    // return (x - 1) * (x - 1) - Math.Sin(x * x * x);
}


double[] dimensions = {1, 10};
double NewtonFunction(double x)
{
    // return x * x;
    // return (Math.Sqrt(Math.Abs(x)) - 5) * x + 10;
    return (x - 1) * (x - 1) + Math.Sin(x * x * x);
}

double NDimFunction (double[] dims)
{
    // return dimensions[0] * dimensions[0] 
    //      + dimensions[1] * dimensions[1];

    return (dims[0] + 2 * dims[1] -7) * (dims[0] + 2 * dims[1] -7)
         + (2 * dims[0] + dims[1] -5) * (2 * dims[0] + dims[1] -5);
}


var date = DateTime.Now;

date = DateTime.Now;
double[] sold = Optimize.DescendentGradient(NDimFunction, new double[]{1, 10});
var diff = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
Console.WriteLine(sold[0]);
Console.WriteLine(sold[1]);
