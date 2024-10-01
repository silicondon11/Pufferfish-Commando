using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string aName;
    public string description;
    public int coinsReward;
    public int xpReward;
    public float progress;
}
