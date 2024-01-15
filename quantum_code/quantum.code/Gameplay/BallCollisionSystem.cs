using System.Collections.Generic;
using Quantum.Player;

namespace Quantum.Gameplay;

public unsafe class BallCollisionSystem : SystemSignalsOnly, ISignalOnCollisionEnter3D {
    public void OnCollisionEnter3D(Frame f, CollisionInfo3D info) {
        if (f.Unsafe.TryGetPointer<PlayerLink>(info.Other, out PlayerLink* link))
            f.lastPlayerTouched = link;

        if (f.lastPlayerTouched != null)
            if (info.StaticData.ColliderIndex == 5) {
                MovementSystem.ResetDeflect(f);

                Log.Info("GOAL! by Player " + f.lastPlayerTouched->Player._index);
                f.lastPlayerTouched->score++;
                BallSystem.ResetBall(f);
            }
    }
}