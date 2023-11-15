using System;

using UnityEngine;

namespace GameFramework.System
{
    [Serializable]
    public class State : object, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string StateName;
        public string GetStateName() => StateName;

        [SerializeField, HideInInspector]
        private int StateId = -1;
        public int GetStateId() => StateId;

        /// <summary>
        /// Use States.GetState instead.
        /// </summary>
        public State(string InStateName, int InStateId)
        {
            StateName = InStateName;
            StateId = InStateId;
        }

        /// <summary>
        /// "A.B".MatchesState("A") = True
        /// </summary>
        public bool MatchesState(State StateToCheck)
        {
            State[] SeparatedState = StatesManager.GetSeparatedState(this);
            State[] SeparatedStateToCheck = StatesManager.GetSeparatedState(StateToCheck);

            if (SeparatedState.Length == SeparatedStateToCheck.Length) return this == StateToCheck;
            if (SeparatedStateToCheck.Length > SeparatedState.Length) return false;

            for (int i = 0; i < SeparatedState.Length && i < SeparatedStateToCheck.Length; i++)
            {
                if (SeparatedState[i] != SeparatedStateToCheck[i]) return false;
            }

            if (SeparatedStateToCheck.Length == 0 && SeparatedState.Length > 0) return SeparatedState[0] == StateToCheck;
            
            return true;
        }

        public static bool HasAny(State[] InStates1, State[] InStates2)
        {
            if (InStates2.Length < 1) return false;

            for (int i = 0; i < InStates2.Length; i++)
            {
                for (int j = 0; j < InStates1.Length; j++)
                {
                    if (InStates1[j].MatchesState(InStates2[i])) return true;
                }
            }
            return false;
        }

        public static bool HasAll(State[] InStates1, State[] InStates2)
        {
            if(InStates2.Length < 1) return true;

            for (int i = 0; i < InStates2.Length; i++)
            {
                for (int j = 0; j < InStates1.Length; j++)
                {
                    if (!InStates1[j].MatchesState(InStates2[i])) return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            State other = obj as State;
            if (other == null) return base.Equals(obj);

            return StateId == other.StateId;
        }

        public static bool Equals(State X, State Y)
        {
            if ((object)X == (object)Y) return true;
            if (X is null || Y is null) return false;
            return X.Equals(Y);
        }

        public static bool operator ==(State X, State Y) => Equals(X, Y);
        public static bool operator !=(State X, State Y) => !Equals(X, Y);

        public override int GetHashCode() => StateId;
        public override string ToString() => StateName;

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            if(StateName == null || StateName == "") return;

            State State;           
            if (StateId != -1)
            {
                State = StatesManager.GetState(StateId);
                if(State != null)
                {
                    if (State.StateName == StateName) return;
                }
            }
            
            State = StatesManager.GetState(StateName);
            if (State == null) return;
            
            StateId = State.StateId;
        }
    }
}