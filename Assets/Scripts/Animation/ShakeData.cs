using UnityEngine;

[CreateAssetMenu(menuName = "JuiceAnimation/ShakeData")]
public class ShakeData : ScriptableObject
{
    public float intensity;
    public float sustain;
    public float release;
}