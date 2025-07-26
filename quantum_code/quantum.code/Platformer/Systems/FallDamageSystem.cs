using Photon.Deterministic;

namespace Quantum.Platformer.Systems
{
    public unsafe class FallDamageSystem : SystemMainThreadFilter<FallDamageSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Health* Health;
            public Fall* Fall;
            public Transform3D* Transform;
            public CharacterController3D* CharacterController;
        }

        public override void Update(Frame frame, ref Filter f)
        {
            FP currentY = f.Transform->Position.Y;

            if (!f.Fall->IsFalling && currentY > f.Fall->StartFallY + FP._0_01)
            {
                f.Fall->IsFalling = true;
                f.Fall->StartFallY = currentY;
            }
            if (f.Fall->IsFalling && f.CharacterController->Grounded)
            {
                FP fallDistance = f.Fall->StartFallY - currentY;

                if (fallDistance > f.Fall->FallThreshold)
                {
                    int damage = (int)((fallDistance - f.Fall->FallThreshold) * f.Fall->DamagePerMeter);
                    frame.Signals.OnHealthChanged(f.Entity, damage);
                    Quantum.Log.Debug($"FallDistance {fallDistance} Damage {damage}");
                }
                f.Fall->IsFalling = false;
            }
        }
    }
}