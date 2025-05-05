using LitMotion;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game

{
    internal class Stone : MonoBehaviour
    {
        [SerializeField] private StoneType _stoneType;

        internal int X { get; set; }
        internal int Y { get; set; }
        internal StoneType StoneType => _stoneType;

        internal Action<Stone> GemDestroyComplete;

        private RectTransform _rectTransform;
        private CompositeMotionHandle _сompositeMotionHandle;

        internal RectTransform RectTransform => _rectTransform;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _сompositeMotionHandle = new CompositeMotionHandle();
        }

        internal void Initialize(Int32 x, Int32 y)
        {
            X = x;
            Y = y;

            _rectTransform.localScale = Vector3.zero;
        }


        internal void DestroyGem()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _сompositeMotionHandle.Cancel();
        }
    }
}