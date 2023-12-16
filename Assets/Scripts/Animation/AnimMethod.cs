using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMethod : MonoBehaviour
{
    public void Shake(ShakeData _data)
    {
        StartCoroutine(GameManager.Instance.effects.ScreenShake(_data.intensity, _data.sustain, _data.release));
    }

    public void Freeze(float _duration)
    {
        StartCoroutine(GameManager.Instance.effects.FreezeTime(_duration));
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
