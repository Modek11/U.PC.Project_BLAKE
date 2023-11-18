using System;
using System.Collections;

using UnityEngine;

using GameFramework.System;

namespace GameFramework.Abilities
{
    [Serializable]
    public partial class Ability : object
    {
        [SerializeField, HideInInspector]
        public AbilityDefinition AbilityDefinition;

        public AbilityManager OwningAbilityManager { get; private set; }

        private bool isActive = false;

        public delegate void AbilityEnded(bool wasCanceled);
        public event AbilityEnded OnAbilityEnded;

        public partial void StartCoroutine(IEnumerator routine);

        public virtual partial void OnGiveAbility(AbilityManager abilityManager);
        public virtual partial bool CanActivateAbility();
        public virtual partial void ActivateAbility();
        public virtual partial void EndAbility(bool wasCanceled);
    }

    public partial class Ability : object
    {
        public partial void StartCoroutine(IEnumerator routine)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            OwningAbilityManager.StartCoroutine(routine);
        }

        public virtual partial void OnGiveAbility(AbilityManager abilityManager)
        {
            OwningAbilityManager = abilityManager;
        }

        public virtual partial bool CanActivateAbility()
        {
            if (isActive) return false;
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return false; }

            if (AbilityDefinition.BlockedStates.Length > 0 || AbilityDefinition.RequiredStates.Length > 0)
            {
                State[] abilityManagerStates = OwningAbilityManager.ActiveStates;

                if (State.HasAny(abilityManagerStates, AbilityDefinition.BlockedStates)) return false;
                if (!State.HasAll(abilityManagerStates, AbilityDefinition.RequiredStates)) return false;
            }

            return true;
        }

        public virtual partial void ActivateAbility()
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            isActive = true;

            for (int i = 0; i < AbilityDefinition.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityDefinition.StatesToAdd[i], 1);
            }
        }

        public virtual partial void EndAbility(bool wasCanceled)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            isActive = false;

            for (int i = 0; i < AbilityDefinition.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityDefinition.StatesToAdd[i], -1);
            }
            OnAbilityEnded?.Invoke(wasCanceled);
        }
    }
}