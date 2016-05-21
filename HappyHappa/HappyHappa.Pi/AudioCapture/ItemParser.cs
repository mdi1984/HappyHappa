using System;
using HappyHappa.Pi.Model;

namespace HappyHappa.Pi.AudioCapture
{
  public static class ItemParser
  {
    public static bool TryParseItem(string input, out SimpleItem item)
    {
      // assume format: {amount} [{unit}] {name}
      var parts = input.Split(' ');

      var unit = string.Empty;
      if (parts.Length >= 3)
      {
        unit = parts[1];
      }

      int amount;
      var amountParseResult = int.TryParse(parts[0], out amount);
      if (!amountParseResult)
      {
        if (parts[0].Contains("ein", StringComparison.OrdinalIgnoreCase))
        {
          amount = 1;
        }
        else
        {
          item = null;
          return false;
        }
      }
      amount = string.IsNullOrEmpty(unit) ? amount : ModifyAmountBasedOnUnit(amount, unit);
      var nameStart = string.IsNullOrEmpty(unit) ? 1 : 2;

      var name = string.Join(" ", parts.SubArray(nameStart, parts.Length - nameStart));
      name = name.ToCharactersOnlyString();

      item = new SimpleItem
      {
        ItemName = name,
        Amount = Math.Abs(amount)
      };
      return true;
    }

    private static int ModifyAmountBasedOnUnit(int amount, string unit)
    {
      switch (unit)
      {
        // TODO: extend this...
        case "Stück":
          return amount;
        case "Kilo":
          return amount * 1000;
        case "kg":
          return amount * 1000;
        case "Liter":
          return amount * 1000;
        default:
          return amount;
      }
    }
  }
}