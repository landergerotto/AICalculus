using System;
using System.Collections.Generic;
using AIContinuous;
using AIContinuous.Nuenv;
using RocketSim;

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

double Restriction(double[] dims)
{

    return -1.0;
    // return dims[0] * dims[0] + dims[1] * dims[1] - 2.0;
    // return (dims[0] - 1) * (dims[0] - 1) * (dims[0] - 1) - dims[1] + 1.0 - 2.0;

}

// double RocketFunction(double[] mes)
// {
//     var Rocket = new Rocket();

//     Rocket.FlyALittleBit(mes);

//     return -Rocket.Height;
// }

List<double[]> bounds = new() { 
    new double[]{-10, 10},
    new double[]{-10, 10},
};

List<double[]> RocketBounds = new() { 
    new double[]{0.0, 3500},
    new double[]{0.0, 3500},
    new double[]{0.0, 3500},
    new double[]{0.0, 3500},
    new double[]{0.0, 3500},
};

var timeData = Space.Linear(0.0, 200.0, 11);
var massFlowData = Space.Uniform(17.5, 11);


var Rocket = new Rocket(timeData, massFlowData);


var date = DateTime.Now;

// var evol = new DiffEvolution(RocketFunction, RocketBounds, 1000, Restriction);

// date = DateTime.Now;
// // double[] sold = Optimize.DescendentGradient(RosenBrookFunction, new double[]{10, 10}, 1e-6, 1e-9);
// var Evol = evol.Optimize(1000);
// var diff = DateTime.Now - date;

// Console.WriteLine("Executed in: " + diff.TotalMilliseconds + " Miliseconds");
// Console.WriteLine(Evol[0]);
Console.WriteLine(Rocket.LaunchUntilMax());

