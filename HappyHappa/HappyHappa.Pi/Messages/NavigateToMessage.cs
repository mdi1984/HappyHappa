using System;

namespace HappyHappa.Pi.Messages
{
  public class NavigateToMessage
  {
    public NavigateToMessage(Type type)
    {
      this.Type = type;
    }

    public Type Type { get; private set; }
  }
}