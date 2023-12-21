using GameSystems;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public Transform m_wallLeft;
    public Transform m_wallRight;
    public Transform m_wallBottom;
    public Transform m_wallTop;

    public void ActivateWall(RoomEntrance _entrances)
    {
        Debug.Log("Activate walls " + _entrances);
        m_wallLeft.gameObject.SetActive(true);
        m_wallRight.gameObject.SetActive(true);
        m_wallBottom.gameObject.SetActive(true);
        m_wallTop.gameObject.SetActive(true);
        
        if (_entrances.HasFlag(RoomEntrance.South))
        {
            m_wallBottom.gameObject.SetActive(false);
        }
            
        if (_entrances.HasFlag(RoomEntrance.North))
        {
            m_wallTop.gameObject.SetActive(false);
        }
            
        if (_entrances.HasFlag(RoomEntrance.East))
        {
            m_wallRight.gameObject.SetActive(false);
        }
            
        if (_entrances.HasFlag(RoomEntrance.West))
        {
            m_wallLeft.gameObject.SetActive(false);
        }
    }
}
