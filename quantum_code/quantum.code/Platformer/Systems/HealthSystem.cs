using System;

namespace Quantum.Platformer.Systems
{
    public unsafe class HealthSystem : SystemSignalsOnly, ISignalOnHealthChanged
    {
        public void OnHealthChanged(Frame frame, EntityRef entity, int healthChange)
        {
            ChangeHealth(frame, entity, healthChange);
        }

        private void ChangeHealth(Frame frame, EntityRef entity, int healthChange)
        {
            frame.Unsafe.TryGetPointer<Health>(entity, out Health* health);
            health->CurrentHealth = Math.Max(health->CurrentHealth - healthChange, 0);
            frame.Events.HealthChanged(entity, health->CurrentHealth);

            if (health->CurrentHealth == 0)
            {
                if (frame.Unsafe.TryGetPointer(entity, out PlayerLink* playerLink))
                {
                    Quantum.Log.Debug($"Player death event");
                    frame.Events.Death(entity, playerLink->Player);
                }
            }
        }
    }
}
