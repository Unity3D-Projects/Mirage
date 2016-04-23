using UnityEngine;
using System.Collections.Generic;
using Klak.Motion;
using Kvant;

namespace Mirage
{
    public class TouchController : MonoBehaviour
    {
        #region External references

        [SerializeField] SmoothFollow[] _followers;
        [SerializeField] Transform _frontScreen;
        [SerializeField] Camera _primaryCamera;

        #endregion

        #region Public methods

        public void CaptureFollowers()
        {
            foreach (var h in _handlers) h.ChangeTarget(transform);
        }

        public void ReleaseFollowers()
        {
            foreach (var h in _handlers) h.UseOriginalTarget();
        }

        #endregion

        #region Handler objects

        class FollowerHandler
        {
            SmoothFollow _follower;
            Transform _originalTarget;

            public FollowerHandler(SmoothFollow follower)
            {
                _follower = follower;
                _originalTarget = follower.target;
            }

            public void UseOriginalTarget()
            {
                _follower.target = _originalTarget;
            }

            public void ChangeTarget(Transform target)
            {
                _follower.target = target;
            }
        }

        List<FollowerHandler> _handlers;

        #endregion

        #region MonoBehaviour functions

        void Start()
        {
            _handlers = new List<FollowerHandler>(_followers.Length);

            foreach (var follower in _followers)
                _handlers.Add(new FollowerHandler(follower));
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mp = Input.mousePosition;

                var view = _primaryCamera.rect;
                var px =  mp.x / Screen.width;
                var py = (mp.y / Screen.height - view.yMin) / view.height;

                var scale = _frontScreen.localScale;
                px = (px - 0.5f) * scale.x;
                py *= scale.y;

                transform.localPosition = new Vector3(px, py, 0);
            }
        }

        #endregion
    }
}
