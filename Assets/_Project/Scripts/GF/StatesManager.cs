using System;
using System.IO;
using System.Collections.Generic;

namespace GameFramework.System
{
    public static class StatesManager
    {
        private static string[] StatesNames;
        public static string[] GetStatesNames() => StatesNames;

        private static State[] States = new State[0];
        private static Tuple<State, State[]>[] StatesWithSubStates = new Tuple<State, State[]>[0];

#if UNITY_EDITOR
        static StatesManager()
        {
            StatesNames = File.ReadAllLines("Assets/Resources/BlakeStates.txt");

            for (int i = 0; i < StatesNames.Length; i++)
            {
                StatesNames[i] = StatesNames[i].Split(',')[0];
            }
            InitializeStates();
        }
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadAsset()
        {
            UnityEngine.TextAsset StatesAsset = UnityEngine.Resources.Load<UnityEngine.TextAsset>("BlakeStates");
            if (StatesAsset == null) return;

            StatesNames = StatesAsset.text.Split('\n');

            for (int i = 0; i < StatesNames.Length; i++)
            {
                StatesNames[i] = StatesNames[i].Split(',')[0];
            }
            InitializeStates();
        }
#endif

        private static void InitializeStates()
        {
            States = new State[StatesNames.Length];
            StatesWithSubStates = new Tuple<State, State[]>[StatesNames.Length];

            for (int i = 0; i < StatesNames.Length; i++)
            {
                List<State> ParentStates = new();
                {
                    string State = StatesNames[i];
                    for (int Index = State.LastIndexOf('.'); Index != -1; Index = State.LastIndexOf('.'))
                    {
                        State = State[..Index];
                        ParentStates.Add(GetState(State));
                    }
                }

                State[] ParentStatesArray = new State[ParentStates.Count];
                {
                    int n = 0;
                    for (int j = ParentStates.Count - 1; j >= 0; j--)
                    {
                        ParentStatesArray[n++] = ParentStates[j];
                    }
                }

                States[i] = new State(StatesNames[i], i);
                StatesWithSubStates[i] = new Tuple<State, State[]>(States[i], ParentStatesArray);
            }
        }

        public static State GetState(string InStateName)
        {
            for (int i = 0; i < States.Length; i++)
            {
                if (States[i].GetStateName() == InStateName)
                {
                    return States[i];
                }
            }
            return null;
        }

        public static State GetState(int InId)
        {
            if (States.Length > InId)
            {
                return States[InId];
            }
            return null;
        }

        public static State[] GetStates() => States;
        public static int GetStatesCount() => States.Length;

        public static State[] GetSeparatedState(State InState)
        {
            for (int i = 0; i < StatesWithSubStates.Length; i++)
            {
                if (StatesWithSubStates[i].Item1.GetStateName() == InState.GetStateName())
                {
                    return StatesWithSubStates[i].Item2;
                }
            }
            return new State[0];
        }
    }
}
