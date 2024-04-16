using System;
using System.Collections.Generic;
using System.Text;


public class Dice
{
    private static Random RandomGenerator = new Random(10);
    public static int Seed { set { RandomGenerator = new Random(value);} }

    public static float PercentageChance()
    {
        return (float) RandomGenerator.NextDouble();
    }

    public static int RangeRoll(int min, int max)
    {
        return RandomGenerator.Next(min, max);
    }


}

