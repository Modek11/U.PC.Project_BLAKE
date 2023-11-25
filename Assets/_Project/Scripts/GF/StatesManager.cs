using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.System
{
    public static class StatesManager
    {
        private static string[] statesNames;
        public static string[] StatesNames { get => statesNames; }

        private static State[] states = new State[0];
        private static Tuple<State, State[]>[] statesWithSubStates = new Tuple<State, State[]>[0];

#if UNITY_EDITOR
        static StatesManager()
        {
            statesNames = File.ReadAllLines("Assets/Resources/BlakeStates.txt");

            for (int i = 0; i < statesNames.Length; i++)
            {
                statesNames[i] = statesNames[i].Split(',')[0];
            }
            InitializeStates();
        }
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadAsset()
        {
            UnityEngine.TextAsset statesAsset = UnityEngine.Resources.Load<UnityEngine.TextAsset>("BlakeStates");
            if (statesAsset == null) return;

            statesNames = statesAsset.text.Split('\n');

            for (int i = 0; i < statesNames.Length; i++)
            {
                statesNames[i] = statesNames[i].Split(',')[0];
            }
            InitializeStates();
        }
#endif

        private static void InitializeStates()
        {
            states = new State[statesNames.Length];
            statesWithSubStates = new Tuple<State, State[]>[statesNames.Length];

            for (int i = 0; i < statesNames.Length; i++)
            {
                List<State> parentStates = new();
                {
                    string state = statesNames[i];
                    for (int Index = state.LastIndexOf('.'); Index != -1; Index = state.LastIndexOf('.'))
                    {
                        state = state[..Index];
                        parentStates.Add(GetState(state));
                    }
                }

                State[] parentStatesArray = new State[parentStates.Count];
                {
                    int n = 0;
                    for (int j = parentStates.Count - 1; j >= 0; j--)
                    {
                        parentStatesArray[n++] = parentStates[j];
                    }
                }

                states[i] = new State(statesNames[i], i);
                statesWithSubStates[i] = new Tuple<State, State[]>(states[i], parentStatesArray);
            }
        }

        public static State GetState(string stateName)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].StateName == stateName)
                {
                    return states[i];
                }
            }
            return null;
        }

        public static State GetState(int stateId)
        {
            if (states.Length > stateId)
            {
                return states[stateId];
            }
            return null;
        }

        public static State[] States { get => states; }
        public static int StatesCount { get => states.Length; }

        public static State[] GetSeparatedState(State state)
        {
            for (int i = 0; i < statesWithSubStates.Length; i++)
            {
                if (statesWithSubStates[i].Item1.StateName == state.StateName)
                {
                    return statesWithSubStates[i].Item2;
                }
            }
            return new State[0];
        }
    }
}
