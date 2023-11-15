using System;
using System.Collections.Generic;

using UnityEngine;

using SolidUtilities.UnityEngineInternals;

using GameFramework.System;

namespace GameFramework.Abilities
{
    public partial class AbilityManager : MonoBehaviour
    {
        [SerializeField] 
        private List<AbilityDefinition> StartupAbilities = new();
        protected List<Ability> GivenAbilities = new();

        protected Dictionary<State, int> StateCountArray = new();
        protected List<State> ActiveStates = new();
        protected Dictionary<State, StateDelegate> StateEventArray = new();

        public delegate void StateDelegate(State State, int NewCount);

        private partial void Awake();

        #region Abilities

        public partial void GiveAbility(AbilityDefinition AbilityData);
        public partial bool TryActivateAbility(Type AbilityClass);

        #endregion

        #region States

        public State[] GetActiveStates() => ActiveStates.ToArray();
        public partial void UpdateStates(State State, int CountDelta);
        public partial void RegisterStateEvent(State InState, StateDelegate InDelegate);
        
        #endregion
    }

    public partial class AbilityManager : MonoBehaviour
    {
        private partial void Awake()
        {
            for (int i = 0; i < StartupAbilities.Count; i++)
            {
                GiveAbility(StartupAbilities[i]);
            }
        }

        #region Abilities

        public partial void GiveAbility(AbilityDefinition AbilityData)
        {
            if (AbilityData == null) { Debug.LogError("AbilityData is not vaild"); return; }

            Ability NewAbility = AbilityData.GetAbilityInstance().ShallowCopy();
            GivenAbilities.Add(NewAbility);
            NewAbility.OnGiveAbility(this);
        }

        public partial bool TryActivateAbility(Type AbilityClass)
        {
            if (AbilityClass == null) { Debug.LogError("AbilityClass is not valid"); return false; }

            for (int i = 0; i < GivenAbilities.Count; i++)
            {
                if (GivenAbilities[i].AbilityData == null)
                {
                    Debug.LogError("AbilityData is not valid. " + GivenAbilities[i].ToString());
                    continue;
                }

                if (GivenAbilities[i].AbilityData.AbilityClass.Type == AbilityClass)
                {
                    if (!GivenAbilities[i].CanActivateAbility()) return false;

                    GivenAbilities[i].ActivateAbility();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region States

        public partial void RegisterStateEvent(State InState, StateDelegate InDelegate)
        {
            if(!StateEventArray.ContainsKey(InState))
            {
                StateEventArray.Add(InState, InDelegate);
                return;
            }

            StateEventArray[InState] += InDelegate;
        }

        public partial void UpdateStates(State State, int CountDelta)
        {
            if (CountDelta != 0)
            {
                if(StateCountArray.ContainsKey(State))
                {
                    StateCountArray[State] = Math.Max(StateCountArray[State] + CountDelta, 0);
                }
                else
                {
                    StateCountArray.Add(State, Math.Max(CountDelta, 0));
                }

                if(ActiveStates.Contains(State))
                {
                    if (StateCountArray[State] == 0)
                    {
                        ActiveStates.Remove(State);
                    }
                }
                else
                {
                    if (StateCountArray[State] != 0)
                    {
                        ActiveStates.Add(State);
                    }
                }

                if (StateEventArray.ContainsKey(State))
                {
                    List<StateDelegate> InvalidDelegates = null;
                    Delegate[] Delegates = StateEventArray[State].GetInvocationList();
                    for (int i = 0; i < Delegates.Length; i++)
                    {
                        if (Delegates[i] is not StateDelegate StateDelegate) continue;
                        
                        if (StateDelegate.Target is MonoBehaviour Behaviour)
                        {
                            if (Behaviour == null)
                            {
                                InvalidDelegates ??= new List<StateDelegate>();
                                InvalidDelegates.Add(StateDelegate);
                                continue;
                            }
                        }

                        StateDelegate.Invoke(State, StateCountArray[State]);
                    }

                    if (InvalidDelegates == null) return;

                    for (int i = 0; i < InvalidDelegates.Count; i++)
                    {
                        StateEventArray[State] -= InvalidDelegates[i];
                    }
                }
            }
        }

        #endregion
    }
}