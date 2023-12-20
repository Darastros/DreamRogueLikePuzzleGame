using System;
using DG.Tweening;
using GameSystems;
using UnityEngine;
using Utils;

namespace UI
{
    public class ShowMap : MonoBehaviour, IEventListener
    {
        [SerializeField]
        RectTransform minimap;
        
        [SerializeField]
        RectTransform map;

        public float scaleUpTime = 0.3f;
        public Ease easeScaleUp = Ease.Linear;

        public float scaleDownTime = 0.3f;
        public Ease easeScaleDown = Ease.Linear;


        private Vector2 startAnchorPos;
        private Vector2 startAnchorMax;
        private Vector2 startAnchorMin;
        private Vector3 startLocalPos;
        private Vector2 startPivot;
        private Vector2 startSizeDelta;

        private Sequence tweenSequence;
        private void Awake()
        {
            startAnchorPos = minimap.anchoredPosition;
            startAnchorMax = minimap.anchorMax;
            startAnchorMin = minimap.anchorMin;
            startLocalPos = minimap.position;
            startPivot = minimap.pivot;
            startSizeDelta = minimap.sizeDelta;
        }

        private void OnEnable()
        {
            DungeonRoomSystem.EventDispatcher?.RegisterEvent<ShowBigMap>(this, OnShowBigMap);
            DungeonRoomSystem.EventDispatcher?.RegisterEvent<HideBigMap>(this, OnHideBigMap);
        }

        private void OnDisable()
        {
            DungeonRoomSystem.EventDispatcher?.UnregisterAllEvents(this);
        }

        private void OnHideBigMap(HideBigMap _obj)
        {
            ScaleDown();
        }

        private void OnShowBigMap(ShowBigMap _obj)
        {
            ScaleUp();
        }

        public void ScaleUp()
        {
            DOTween.Kill(minimap);
            minimap.DOSizeDelta(map.sizeDelta, scaleUpTime).SetEase(easeScaleUp);
            minimap.DOAnchorPos(map.anchoredPosition, scaleUpTime).SetEase(easeScaleUp);
            minimap.DOAnchorMax(map.anchorMax, scaleUpTime).SetEase(easeScaleUp);
            minimap.DOAnchorMin(map.anchorMin, scaleUpTime).SetEase(easeScaleUp);
            minimap.DOPivot(map.pivot, scaleUpTime).SetEase(easeScaleUp);
            minimap.DOMove(map.position, scaleUpTime).SetEase(easeScaleUp);
        }
        
        public void ScaleDown()
        {
            DOTween.Kill(minimap);
            minimap.DOSizeDelta(startSizeDelta, scaleDownTime).SetEase(easeScaleDown);
            minimap.DOAnchorPos(startAnchorPos, scaleDownTime).SetEase(easeScaleDown);
            minimap.DOAnchorMax(startAnchorMax, scaleDownTime).SetEase(easeScaleDown);
            minimap.DOAnchorMin(startAnchorMin, scaleDownTime).SetEase(easeScaleDown);
            minimap.DOPivot(startPivot, scaleDownTime).SetEase(easeScaleDown);
            minimap.DOMove(startLocalPos, scaleDownTime).SetEase(easeScaleDown);
        }
    }
}