using UnityEngine;

using TypeReferences;

using GameFramework.System;
using GameFramework.System.Attributes;

namespace GameFramework.Abilities
{
    [CreateAssetMenu(menuName = "GameFramework/Abilities/Modifier")]
    public class Modifier : ScriptableObject
    {
        public DurationPolicy DurationPolicy = DurationPolicy.Instant;

        [Tooltip("Period in seconds. 0 for non-periodic effects.")]
        public float Period = 0f;

        [Tooltip("States that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        [State] public State[] GrantedStatesAdded;

        [Tooltip("States that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        [State] public State[] GrantedStatesRemoved;

        [Tooltip("Once applied, these States determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing State Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.")]
        [State] public State[] OngoingStateRequirementsRequired;

        [Tooltip("Once applied, these States determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing State Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.")]
        [State] public State[] OngoingStateRequirementsIgnored;

        [Tooltip("States on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.")]
        [State] public State[] ApplicationStateRequirementsRequired;

        [Tooltip("States on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.")]
        [State] public State[] ApplicationStateRequirementsIgnored;

        [Tooltip("Once applied, these States determine whether the GameplayEffect should be removed. Also prevents effect application.")]
        [State] public State[] RemovalStateRequirementsRequired;

        [Tooltip("Once applied, these States determine whether the GameplayEffect should be removed. Also prevents effect application.")]
        [State] public State[] RemovalStateRequirementsIgnored;

        [Tooltip("GameplayEffects on the Target that have any of these States in their Asset States or Granted States will be removed from the Target when this GameplayEffect is successfully applied.")]
        [State] public State[] RemoveGameplayEffectsWithStates;

        [Tooltip("States that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        [Inherits(typeof(Ability))]
        public TypeReference[] GrantedAbilities;
    }

    public enum DurationPolicy
    {
        Instant,
        Infinite,
        HasDuration
    }
}