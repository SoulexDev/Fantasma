using Fantasma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
public class StateMachine<T> : FantasmaObject where T : StateMachine<T>
{
    public Dictionary<Enum, State<T>> m_stateDictionary = new Dictionary<Enum, State<T>>();
    public State<T> m_currentState;
    private bool m_switchingState;

    protected delegate void StateSwitched();
    protected event StateSwitched OnStateSwitched;
    public Enum m_previousState;
    public Enum m_currentStateEnum;
    public Enum m_nextState;

    public override void Update()
    {
        if (m_switchingState || m_currentState == null)
            return;

        m_currentState.UpdateState((T)this);
        Console.WriteLine(m_currentState);
    }
    private void SwitchState(State<T> state)
    {
        if (m_currentState == state && !state.canEnterSelf || m_switchingState)
        {
            return;
        }

        m_switchingState = true;

        m_currentState?.ExitState((T)this);

        m_currentState = state;

        m_currentState.EnterState((T)this);

        m_switchingState = false;
    }
    public void SwitchState(Enum state)
    {
        m_nextState = state;
        m_previousState = m_stateDictionary.FirstOrDefault(k => k.Value == m_currentState).Key;
        SwitchState(m_stateDictionary[state]);
        m_currentStateEnum = state;

        OnStateSwitched?.Invoke();
    }
}