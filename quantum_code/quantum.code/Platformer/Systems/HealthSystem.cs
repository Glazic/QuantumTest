namespace Quantum.Platformer.Systems
{
    public unsafe class HealthSystem : SystemSignalsOnly, ISignalOnHealthChanged
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Health* Health;
        }

        public void OnHealthChanged(Frame frame, EntityRef entity, int healthChange)
        {
            ChangeHealth(frame, entity, healthChange);
        }

        private void ChangeHealth(Frame frame, EntityRef entity, int healthChange)
        {
            frame.Unsafe.TryGetPointer<Health>(entity, out Health* health);
            health->CurrentHealth = health->CurrentHealth - healthChange;
            frame.Events.HealthChanged(entity, health->CurrentHealth);
            Quantum.Log.Debug($"Damage {healthChange}");
        }
    }
}
