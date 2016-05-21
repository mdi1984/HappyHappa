using System;
using System.Collections.Generic;
using System.Text;

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

    public static string ToCharactersOnlyString(this string str)
    {
      var vowels = "äöü";
      StringBuilder sb = new StringBuilder();
      foreach (char c in str)
      {
        if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || vowels.Contains(c.ToString()))
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }

    public static string RemoveSpecialCharacters(this string str)
    {
      StringBuilder sb = new StringBuilder();
      foreach (char c in str)
      {
        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }

  }
}