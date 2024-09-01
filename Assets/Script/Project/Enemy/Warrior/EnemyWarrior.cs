namespace RiverCrab
{
    public class EnemyWarrior : EnemyNormal
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();            
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void Dead()
        {
            base.Dead();
        }
        public override void TakeDamage(float damage, float hitforce)
        {
            base.TakeDamage(damage, hitforce);
        }
    }
}
