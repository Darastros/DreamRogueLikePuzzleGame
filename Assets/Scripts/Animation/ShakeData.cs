using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animation/ShakeData")]
public class ShakeData : ScriptableObject
{
    public float intensity;
    public float sustain;
    public float release;
}