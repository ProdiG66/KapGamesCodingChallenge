using Quantum.Gameplay;

namespace Quantum.Player;

public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter> {
    public struct Filter {
        public EntityRef Entity;
        public CharacterController3D* CharacterController;
        public PlayerLink* Link;
    }

    public override void Update(Frame f, ref Filter filter) {
        f.players = filter;
        Input input = default;
        if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink)) input = *f.GetPlayerInput(playerLink->Player);

        if (input.Deflect.WasPressed && filter.Link->deflectCount < 1) {
            BallSystem.Deflect(f);
            filter.Link->deflectCount++;
        }

        filter.CharacterController->Move(f, filter.Entity, input.Direction.XOY);
    }

    public static void ResetDeflect(Frame frame) {
        frame.players.Link->deflectCount = 0;
    }
}