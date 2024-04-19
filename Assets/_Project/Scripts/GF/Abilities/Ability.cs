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

        public bool IsActive { get; private set; }
        public bool IsInputPressed { get; set; }
        public object SourceObject { get; private set; }

        public delegate void AbilityEnded(bool wasCanceled);
        public event AbilityEnded OnAbilityEnded;

        public partial void StartCoroutine(IEnumerator routine);
        public partial void SetupAbility(AbilityManager abilityManager, object sourceObject = null);

        public virtual void OnGiveAbility() { }
        public virtual partial bool CanActivateAbility();
        public virtual partial void ActivateAbility();
        public virtual partial void EndAbility(bool wasCanceled = false);

        public virtual void InputPressed() { }
        public virtual void InputReleased() { }
    }

    public partial class Ability : object
    {
        public partial void StartCoroutine(IEnumerator routine)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            OwningAbilityManager.StartCoroutine(routine);
        }

        public partial void SetupAbility(AbilityManager abilityManager, object sourceObject)
        {
            OwningAbilityManager = abilityManager;
            SourceObject = sourceObject;
        }

        public virtual partial bool CanActivateAbility()
        {
            if (IsActive) return false;
            if (AbilityDefinition == null) { Debug.LogError("AbilityDefinition is not valid"); return false; }
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

            IsActive = true;

            for (int i = 0; i < AbilityDefinition.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityDefinition.StatesToAdd[i], 1);
            }
        }

        public virtual partial void EndAbility(bool wasCanceled)
        {
            if (OwningAbilityManager == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            IsActive = false;

            for (int i = 0; i < AbilityDefinition.StatesToAdd.Length; i++)
            {
                OwningAbilityManager.UpdateStates(AbilityDefinition.StatesToAdd[i], -1);
            }
            OnAbilityEnded?.Invoke(wasCanceled);
        }
    }
}