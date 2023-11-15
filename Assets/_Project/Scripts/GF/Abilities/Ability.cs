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
        public AbilityDefinition AbilityData;

        public AbilityManager OwningAbilityManager { get; private set; }

        private bool IsActive = false;

        public delegate void AbilityEnded(bool WasCanceled);
        public event AbilityEnded OnAbilityEnded;

        public partial void StartCoroutine(IEnumerator Routine);

        public virtual partial void OnGiveAbility(AbilityManager InAbilityManager);
        public virtual partial bool CanActivateAbility();
        public virtual partial void ActivateAbility();
        public virtual partial void EndAbility(bool WasCanceled);
    }

    public partial class Ability : object
    {
        public partial void StartCoroutine(IEnumerator Routine)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            OwningAbilityManager.StartCoroutine(Routine);
        }

        public virtual partial void OnGiveAbility(AbilityManager InAbilityManager)
        {
            OwningAbilityManager = InAbilityManager;
        }

        public virtual partial bool CanActivateAbility()
        {
            if (IsActive) return false;
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return false; }

            if (AbilityData.BlockedStates.Length > 0 || AbilityData.RequiredStates.Length > 0)
            {
                State[] AbilityManagerStates = OwningAbilityManager.GetActiveStates();

                if (State.HasAny(AbilityManagerStates, AbilityData.BlockedStates)) return false;
                if (!State.HasAll(AbilityManagerStates, AbilityData.RequiredStates)) return false;
            }

            return true;
        }

        public virtual partial void ActivateAbility()
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            IsActive = true;

            for (int i = 0; i < AbilityData.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityData.StatesToAdd[i], 1);
            }
        }

        public virtual partial void EndAbility(bool WasCanceled)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            IsActive = false;

            for (int i = 0; i < AbilityData.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityData.StatesToAdd[i], -1);
            }
            OnAbilityEnded?.Invoke(WasCanceled);
        }
    }
}