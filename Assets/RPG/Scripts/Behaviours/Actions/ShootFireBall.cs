using BehaviorDesigner.Runtime.Tasks;
using RPG.Scripts.Enemies;

namespace RPG.Scripts.Behaviours.Actions
{
    public class ShootFireBall : Action
    {
        private FireSkull _fireSkull;
        
        public override void OnAwake()
        {
            _fireSkull = GetComponent<FireSkull>();
        }
        
        public override TaskStatus OnUpdate()
        {
            _fireSkull.ShootFireBall();
            return TaskStatus.Success;
        }
    }
}