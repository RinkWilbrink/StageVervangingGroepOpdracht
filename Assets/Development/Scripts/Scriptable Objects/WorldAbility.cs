using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WorldAbility", menuName = "WorldAbility")]
public class WorldAbility : ScriptableObject
{
    public Color abilityActiveColor;
    public float abilityCooldown;
    public int abilityManaCost;
}

