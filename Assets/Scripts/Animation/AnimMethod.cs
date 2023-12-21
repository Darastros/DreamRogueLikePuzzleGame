using UnityEngine;

public class AnimMethod : MonoBehaviour
{
    public void Shake(ShakeData _data)
    {
        GameManager.Instance.effects.ScreenShake(_data.intensity, _data.sustain, _data.release);
    }

    public void Freeze(float _duration)
    {
        GameManager.Instance.effects.FreezeTime(_duration);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
