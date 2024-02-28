using System;
using System.Reflection.Metadata;
using AIContinuous.Nuenv;
using AIContinuous.Rocket;

namespace RocketSim;
public class Rocket
{
    public double EmptyMass         { get; set; } = 750;  // kg
    public double EnergyMass        { get; set; } = 3500; // kg
    public double Diameter          { get; set; } = 0.6;  // m
    public double CrossSectionArea  { get; set; } = 0.6;  // m
    public double ExhaustionSpeed   { get; set;} = 1916;  // m/s
    public double DragCoefficient   { get; set;} = 0.8;

    public double UpForce           { get; set; }
    public double DragForce         { get; set; }
    public double GravityForce      { get; set;}

    public double Acceleration      { get; set; }
    public double Speed             { get; set; }
    public double Height            { get; set; }
    public double Mass              { get; set; }
    public double Time              { get; set; } = 0.0;
    public double[] TimeData        { get; set; }
    public double[] MassFlowData    { get; set; }

    public Rocket(double[] timeData, double[] massFlowData) 
    { 
        this.TimeData = (double[])timeData.Clone();
        this.MassFlowData = (double[])massFlowData.Clone();
        this.CrossSectionArea = Math.PI * this.Diameter * this.Diameter / 4;

        this.Mass = this.EmptyMass + Integrate.Romberg(this.TimeData, this.MassFlowData);
    }

    public Rocket
    (
        double[] timeData, 
        double[] massFlowData,
        double mass, 
        double energyMass, 
        double diameter, 
        double exhaustionSpeed, 
        double dragCoefficient
    )
    {
        this.TimeData = (double[])timeData.Clone();
        this.MassFlowData = (double[])massFlowData.Clone();
        this.EmptyMass = mass;
        this.EnergyMass = energyMass;
        this.Diameter = diameter;
        this.ExhaustionSpeed = exhaustionSpeed;
        this.DragCoefficient = dragCoefficient;
        this.CrossSectionArea = Math.PI * this.Diameter * this.Diameter / 4;

        this.Mass = this.EmptyMass + Integrate.Romberg(this.TimeData, this.MassFlowData);
    }

    private void UpdateSpeed(double time, double dt)
    {
        GetAcceleration(time);
        double speed = this.Acceleration * dt;

        this.Speed += speed;
    }

    private void UpdateHeight(double dt)
        => this.Height += this.Speed * dt;
    
    private void UpdateMass(double time, double dt)
        => this.Mass -= dt *( ( GetMassFlow(time) + GetMassFlow(time + dt) ) * 0.5 );
    
    private double GetDragForce(double speed, double height)
    {
        var airDensity = Atmosphere.Density(height);
        var airTemperature = Atmosphere.Temperature(height);
        var cd = Drag.Coefficient(speed, airTemperature, this.DragCoefficient);
        var area = this.CrossSectionArea;

        double drag = - 0.5 * cd * airDensity * area * speed * speed * Math.Sign(speed);

        this.DragForce = drag;

        return drag;
    }

    private double GetDragForce(double speed)
        => GetDragForce(speed, this.Height);

    private double GetGravityForce(double height)
    {
        var grav = Gravity.GetGravity(height);
        var force = - (this.EmptyMass + this.EnergyMass) * grav;

        this.GravityForce = force;

        return force;
    } 

    private double GetGravityForce()
        => GetGravityForce(this.Height);

    private double GetMassFlow(double time)
        => Interp1D.Linear(this.TimeData, this.MassFlowData, time, true);

    private double GetUpForce(double time)
    {
        var up = this.GetMassFlow(time) * this.ExhaustionSpeed;
        
        this.UpForce = up;

        return up;
    }
    private double GetAcceleration(double time)
    {
        this.GetUpForce(time);
        this.GetDragForce(this.Speed);
        this.GetGravityForce();

        double TotalMass = this.EmptyMass + this.EnergyMass;

        double accel = (UpForce + DragForce + GravityForce) / TotalMass;

        this.Acceleration = accel;

        return accel;
    }
    public void FlyALittleBit(double dt)
    {
        // Preparacao Inicial, pegar aceleracao inicial, alt e vel

        // fazer calculos ao longo do tempo para vermos o nosso fracasso

        UpdateSpeed(this.Time, dt);
        UpdateHeight(dt);
        UpdateMass(this.Time, dt);

    }

    public double Launch(double time, double dt = 1e-1)
    {
        for (double t = 0.0; t < time; t += dt)
            FlyALittleBit(dt);
        
        return this.Height;
    }

    public double LaunchUntilMax(double dt = 1e-1)
    {
        do FlyALittleBit(dt); 
        while (this.Speed > 0);
        
        return this.Height;
    }

    public double LaunchUntilGround(double dt = 1e-1)
    {
        do FlyALittleBit(dt); 
        while (this.Height > 0);
        
        return this.Height;
    }
}
