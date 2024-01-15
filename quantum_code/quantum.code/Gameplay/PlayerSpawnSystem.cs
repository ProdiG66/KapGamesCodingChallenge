using System;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Player;

namespace Quantum.Gameplay;

internal unsafe class PlayerSpawnSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerDataSet {
    public void OnPlayerDataSet(Frame frame, PlayerRef player) {
        RuntimePlayer data = frame.GetPlayerData(player);
        EntityPrototype prototype = frame.Assets.Prototype("Resources/DB/PlayerCharacter|EntityPrototype");
        if (prototype == null) {
            Log.Error("Prototype Not found");
            return;
        }


        List<EntityRef> entities = new();
        frame.GetAllEntityRefs(entities);
        List<FP> usedSpawnPoints = new();
        foreach (EntityRef e in entities)
            if (frame.Unsafe.TryGetPointer<PlayerLink>(e, out PlayerLink* playerLink)) {
                Log.Info("Has");
                usedSpawnPoints.Add(playerLink->spawnPoint);
            }

        EntityRef entity = frame.Create(prototype);

        if (frame.Unsafe.TryGetPointer<PlayerLink>(entity, out PlayerLink* link)) {
            link->deflectCount = 0;
            link->Player = player;
        }

        FP range = FP._1 * 5;
        FP rand = frame.Global->RngSession.NextInclusive(-range, range);
        while (usedSpawnPoints.Contains(rand)) rand = frame.Global->RngSession.NextInclusive(-range, range);
        usedSpawnPoints.Add(rand);
        if (frame.Unsafe.TryGetPointer<Transform3D>(entity, out Transform3D* transform)) transform->Position.X = rand;

        BallSystem.SpawnBall(frame);
    }

    public override void Update(Frame f, ref MovementSystem.Filter filter) { }
}