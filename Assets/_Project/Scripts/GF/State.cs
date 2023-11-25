using System;

using UnityEngine;

namespace GameFramework.System
{
    [Serializable]
    public class State : object, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string stateName;
        public string StateName { get => stateName; }

        [SerializeField, HideInInspector]
        private int stateId = -1;
        public int StateId { get => stateId; }

        /// <summary>
        /// Use States.GetState instead.
        /// </summary>
        public State(string stateName, int stateId)
        {
            this.stateName = stateName;
            this.stateId = stateId;
        }

        /// <summary>
        /// "A.B".MatchesState("A") = True
        /// </summary>
        public bool MatchesState(State state)
        {
            State[] separatedState = StatesManager.GetSeparatedState(this);
            State[] separatedStateToCompare = StatesManager.GetSeparatedState(state);

            if (separatedState.Length == separatedStateToCompare.Length) return this == state;
            if (separatedStateToCompare.Length > separatedState.Length) return false;

            for (int i = 0; i < separatedState.Length && i < separatedStateToCompare.Length; i++)
            {
                if (separatedState[i] != separatedStateToCompare[i]) return false;
            }

            if (separatedStateToCompare.Length == 0 && separatedState.Length > 0) return separatedState[0] == state;
            
            return true;
        }

        public static bool HasAny(State[] statesA, State[] statesB)
        {
            if (statesB.Length < 1) return false;

            for (int i = 0; i < statesB.Length; i++)
            {
                for (int j = 0; j < statesA.Length; j++)
                {
                    if (statesA[j].MatchesState(statesB[i])) return true;
                }
            }
            return false;
        }

        public static bool HasAll(State[] statesA, State[] statesB)
        {
            if(statesB.Length < 1) return true;

            for (int i = 0; i < statesB.Length; i++)
            {
                for (int j = 0; j < statesA.Length; j++)
                {
                    if (!statesA[j].MatchesState(statesB[i])) return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            State other = obj as State;
            if (other == null) return base.Equals(obj);

            return stateId == other.stateId;
        }

        public static bool Equals(State x, State y)
        {
            if ((object)x == (object)y) return true;
            if (x is null || y is null) return false;
            return x.Equals(y);
        }

        public static bool operator ==(State x, State y) => Equals(x, y);
        public static bool operator !=(State x, State y) => !Equals(x, y);

        public override int GetHashCode() => stateId;
        public override string ToString() => stateName;

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            if(stateName == null || stateName == "") return;

            State state;           
            if (stateId != -1)
            {
                state = StatesManager.GetState(stateId);
                if(state != null)
                {
                    if (state.stateName == stateName) return;
                }
            }
            
            state = StatesManager.GetState(stateName);
            if (state == null) return;
            
            stateId = state.stateId;
        }
    }
}