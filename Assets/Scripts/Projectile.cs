using GameSystems;
using UnityEngine;
using Utils;

public class Projectile : MonoBehaviour, IEventListener
{
    
    public delegate void HitDelegate();
    public static HitDelegate OnHit;
    
    private void Start()
    {
        DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<OnRoomChanged>(this, _changed => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        GetComponent<Animator>().SetTrigger("Destroy");
        if(TryGetComponent(out Rigidbody2D _rigidbody)) _rigidbody.velocity = Vector2.zero;
        OnHit?.Invoke();
    }

    private void OnDestroy()
    {
        if(DungeonRoomSystem.Instance != null) DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<OnRoomChanged>(this);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
