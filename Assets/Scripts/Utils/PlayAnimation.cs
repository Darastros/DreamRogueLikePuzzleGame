using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Animation>().Play("Resource");
    }
}
