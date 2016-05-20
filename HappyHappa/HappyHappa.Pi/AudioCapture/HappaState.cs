using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyHappa.Pi.AudioCapture
{
  public enum HappaState
  {
    Initializing,
    WaitingForHeyHappa,
    WaitingForCommand,
    WaitingForExpirationDate,
    WaitingForItemCreation,
    WaitingForItemDeletion
  };
}
