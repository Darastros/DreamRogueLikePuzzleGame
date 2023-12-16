using UnityEngine;

namespace GameSystems.RoomScript
{
    public class StartRoom : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.AddStartGameToStack();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.RemoveStartGameFromStack();
            }
        }
    }
}