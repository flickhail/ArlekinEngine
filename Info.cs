using System;
using System.Linq;

namespace ArlekinEngine;

internal static class Info
{
    public static void PrintL(string msg)
    {
        Console.WriteLine(msg);
    }

    public static void Print(string msg)
    {
        Console.Write(msg);
    }
}

internal class Counter
{
    private double _averageValue;
    private double[] _values;
    private int _index;

    public Counter(int bufferSize) 
    {
        _values = new double[bufferSize];
        _index = 0;
    }

    public double Adjust(double value)
    {
        Set(value);
        return _averageValue;
    }

    private void Set(double value)
    {
        _values[_index++] = value;
        _averageValue = _values.Average();
        if (_index >= _values.Length) _index = 0;
    }
}
