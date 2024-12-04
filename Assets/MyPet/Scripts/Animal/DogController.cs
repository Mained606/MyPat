using UnityEngine;

namespace MyPet.AI
{
    public class DogController : AnimalController
    {
        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new DrinkState());
            stateMachine.AddState(new SitState());
            stateMachine.AddState(new DogIdleState());

            ChangeState<DogIdleState>();
        }

        public void Idle()
        {
            ChangeState<DogIdleState>();
        }

        public void Sit()
        {
            ChangeState<SitState>();
        }

        public void Drink()
        {
            ChangeState<DrinkState>();
        }
    }    
}
