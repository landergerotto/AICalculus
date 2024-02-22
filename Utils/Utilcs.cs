namespace AIContinuous;

public static class Utils
{
    public static double Rescale(double x, double min, double max)
        => (max - min) * x + min;
}