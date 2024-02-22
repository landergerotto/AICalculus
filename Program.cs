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

double RosenBrookFunction (double[] dims)
{   
    var n = dims.Length - 1;
    double hellyeah = 0;
    for (int i = 0; i < n; i++)
        hellyeah += 100 * (dims[i + 1] - dims[i] * dims[i]) * (dims[i + 1] - dims[i] * dims[i]) + (1 - dims[i]) * (1 - dims[i]);
    
    return hellyeah;
}

var date = DateTime.Now;

date = DateTime.Now;
double[] sold = Optimize.DescendentGradient(RosenBrookFunction, new double[]{10, 10}, 1e-6, 1e-9);
var diff = DateTime.Now - date;

Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
Console.WriteLine(sold[0]);
Console.WriteLine(sold[1]);

