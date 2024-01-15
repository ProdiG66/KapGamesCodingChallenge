using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Deterministic;
using Quantum.Gameplay;
using Quantum.Player;

namespace Quantum;

unsafe partial class Frame {
    public PlayerLink* lastPlayerTouched;
    public MovementSystem.Filter players;
}