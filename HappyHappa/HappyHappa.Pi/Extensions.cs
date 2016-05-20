﻿using System;
using System.Collections.Generic;

namespace HappyHappa.Pi
{
  public static class Extensions
  {
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
      return source.IndexOf(toCheck, comp) >= 0;
    }

    public static IEnumerable<T> ToEnumerable<T>(this T input)
    {
      yield return input;
    }

    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
      T[] result = new T[length];
      Array.Copy(data, index, result, 0, length);
      return result;
    }
  }
}