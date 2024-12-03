using System.Collections.Generic;
using UnityEngine;

namespace MyPet.AI
{
    /// <summary>
    /// <T>의 상태를 관리하는 클래스
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine;         // 현 State가 등록되어있는 머신
        protected T context;                            // stateMachine을 가지고 있는 주체

        public State() {}                               // 생성자

        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;

            OnInitialized();
        }

        public virtual void OnInitialized() {}          // 생성후 1회 실행, 초기값 설정
        public virtual void OnEnter() {}                // 상태 전환 시 상태로 들어올 때 1회 실행
        public abstract void Update(float deltaTime);   // 상태 실행중 
        public virtual void OnExit() {}                 // 상태 전환시 나갈 때 1회 실행

    }

    /// <summary>
    /// <T>의 State들을 관리하는 클래스
    /// </summary>
    public class StateMachine<T>
    {
        private T context;                              // StateMachine을 가지고 있는 주체
        private State<T> currentState;                  // 현재 상태
        public State<T> CurrentState => currentState;   // 읽기 전용 프로퍼티

        private State<T> previousState;                 // 이전 상태
        public State<T> PreviousState => previousState; // 읽기 전용 프로퍼티

        private float elapsedTimeInState = 0.0f;        // 현재 상태 지속 시간       
        public float ElapsedTimeInState => elapsedTimeInState;  // 읽기 전용 프로퍼티

        // 등록된 상태를 상태의 타입을 키 값으로 저장
        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        // 생성자
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            
            AddState(initialState);
            
            currentState = initialState;
            currentState.OnEnter();
        }

        // StateMachine에 State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine에서 State의 업데이트 실행
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            currentState?.Update(deltaTime);
        }

        // currentState의 상태 바꾸기
        public R ChangeState<R>() where R : State<T>
        {
            // 현 상태와 새로운 상태 비교
            var newType = typeof(R);
            if(currentState.GetType() == newType) 
            {
                return currentState as R;
            }

            // 상태 변경이전
            if(currentState != null)
            {
                currentState.OnExit();
            }
            previousState = currentState;

            // 상태 변경 
            currentState = states[newType];
            currentState.OnEnter();
            elapsedTimeInState = 0.0f;

            return currentState as R;            
        }
    }
}
