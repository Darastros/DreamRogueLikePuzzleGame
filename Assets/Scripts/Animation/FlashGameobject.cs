using DG.Tweening;
using UnityEngine;

namespace AnimationNamespace
{
    public class FlashGameobject : MonoBehaviour
    {
        private Sequence m_sequence = null;

        [SerializeField] private float scaleDown = 0;
        [SerializeField]private float timeFullSize = 0.2f;
        
        [SerializeField] private Ease easeSizeDown = Ease.Linear;
        [SerializeField] private Ease easeSizeUp = Ease.Linear;
        [SerializeField] private float easeSizeDownDuration = 0.5f;
        [SerializeField] private float easeSizeUpDuration = 0.5f;

        
        
        private void Start()
        {
            m_sequence = DOTween.Sequence(transform)
                .Append(transform.DOScale(Vector3.one * scaleDown, easeSizeDownDuration).SetEase(easeSizeDown))
                .AppendInterval(timeFullSize)
                .Append(transform.DOScale(Vector3.one, easeSizeUpDuration).SetEase(easeSizeUp)).SetLoops(-1);
        }

        private void OnDestroy()
        {
            m_sequence.Kill();
        }
    }
}