using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Photon.Deterministic;

namespace Quantum.Gameplay;

public unsafe class BallSystem : SystemSignalsOnly {
    public static void Deflect(Frame frame) {
        List<EntityRef> entities = new();
        frame.GetAllEntityRefs(entities);
        EntityRef ballEntity = entities.Find(entity => frame.Has<BallLink>(entity));
        if (ballEntity.IsValid) {
            frame.Unsafe.GetPointer<PhysicsBody3D>(ballEntity)->AddForce(new FPVector3(0, 0, -40));
            Log.Warn("Deflect");
        }
    }

    public static void ResetBall(Frame frame) {
        List<EntityRef> entities = new();
        frame.GetAllEntityRefs(entities);
        EntityRef ballEntity = entities.Find(entity => frame.Has<BallLink>(entity));
        if (ballEntity.IsValid) {
            FP range = FP._1_50;
            frame.Unsafe.GetPointer<Transform3D>(ballEntity)->Position = new FPVector3(frame.Global->RngSession.NextInclusive(-range, range), 4, frame.Global->RngSession.NextInclusive(-range, range));
            frame.Unsafe.GetPointer<PhysicsBody3D>(ballEntity)->Velocity = new FPVector3(0, -3, 0);
        }
    }

    public static void SpawnBall(Frame frame) {
        List<EntityRef> entities = new();
        frame.GetAllEntityRefs(entities);
        EntityRef ballEntity = entities.Find(entity => frame.Has<BallLink>(entity));
        int playerCount = 0;
        for (int i = 0; i < entities.Count; i++)
            if (frame.Has<PlayerLink>(entities[i]))
                playerCount++;
        if (!ballEntity.IsValid && playerCount >= 2) {
            string ballEntityPath = "Resources/DB/Ball|EntityPrototype";
            EntityPrototype ballPrototype = frame.FindAsset<EntityPrototype>(ballEntityPath);

            if (ballPrototype == null) {
                Log.Error("Ball Prototype Not found");
                return;
            }

            ballEntity = frame.Create(ballPrototype);
            Log.Info("Ball Spawned");
        }
    }
}