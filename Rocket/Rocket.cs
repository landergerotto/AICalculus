using System;
using System.Reflection.Metadata;

using AIContinuous.Rocket;

public class Rocket
{
    public double Mass              { get; set; } = 750;  // kg
    public double EnergyMass        { get; set; } = 3500; // kg
    public double Diameter          { get; set; } = 0.6;  // m
    public double ExhaustionSpeed   { get; set;} = 1916;  // m/s
    public double DragCoefficient   { get; set;} = 0.8;

    public double UpForce           { get; set; }
    public double DragForce         { get; set; }
    public double GravityForce      { get; set;}

    public double Acceleration      { get; set; }
    public double Speed             { get; set; }
    public double Height            { get; set; }


    public Rocket() { }

    public Rocket
    (
        double mass, 
        double energyMass, 
        double diameter, 
        double exhaustionSpeed, 
        double dragCoefficient
    )
    {
        this.Mass = mass;
        this.EnergyMass = energyMass;
        this.Diameter = diameter;
        this.ExhaustionSpeed = exhaustionSpeed;
        this.DragCoefficient = dragCoefficient;
    }

    private double GetAcceleration()
    {
        double TotalMass = this.Mass + this.EnergyMass;

        double accel = (UpForce - DragForce - GravityForce) / TotalMass;

        this.Acceleration = accel;

        return accel;
    }

    private double GetSpeed(double accel, double dt)
    {
        double speed = accel * dt;

        this.Speed = speed;

        return speed;
    }
    private double GetSpeed(double dt)
        => GetSpeed(this.Acceleration, dt);

    private double GetHeight(double speed, double dt)
    {
        double height = speed * dt;

        this.Height = height;

        return height;
    }
    private double GetHeight(double dt)
        => GetHeight(this.Height, dt);

    private double GetDragForce(double height)
    {
        var density = Atmosphere.Density(height);
        var area = Math.PI * this.Diameter / 4;

        double drag = 0.5 * this.DragCoefficient * density * area * this.Speed * this.Speed * 1;

        this.DragForce = drag;

        return drag;
    }

    private double GetDragForce()
        => GetDragForce(this.Height);

    private double GetGravityForce(double height)
    {
        var grav = Gravity.GetGravity(height);
        var force = this.Mass + this.EnergyMass * grav;

        this.GravityForce = force;

        return force;
    } 

    private double GetGravityForce()
        => GetGravityForce(this.Height);

    private double GetUpForce(double me)
    {
        var up = me * this.ExhaustionSpeed;
        
        this.UpForce = up;

        return up;
    }

    private void Calculateheight(double me, int increment)
    {
        if (this.EnergyMass < 0)
            me = 0;
        this.GetUpForce(me);
        this.GetDragForce();
        this.GetGravityForce();

        this.Acceleration = this.GetAcceleration();
        this.Speed = this.GetSpeed(increment);
        var h =  this.GetHeight(increment);
        if (h > this.Height)
            this.Height = h;

        this.EnergyMass -= me;
        if (this.EnergyMass < 0)
            this.EnergyMass = 0;    
    }
    public void Launch(double[] me)
    {
        // Preparacao Inicial, pegar aceleracao inicial, alt e vel

        // fazer calculos ao longo do tempo para vermos o nosso fracasso
        var i = 0;
        do
        {   
            if (i > me.Length)
                this.Calculateheight(0, i);
            else
                this.Calculateheight(me[i], i);

            i++;
        } while (this.Speed > 0);
    }

}
