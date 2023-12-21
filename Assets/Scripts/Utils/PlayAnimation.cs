using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Animation>().Play("Resource");
    }
}
