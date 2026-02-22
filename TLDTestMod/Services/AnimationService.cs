using PastimeReading.Core;
using TLDTestMod.Core.Enums;
using UnityEngine;

namespace TLDTestMod.Services
{
    public static class AnimationService
    {
        private static Animator _animator;
        private static Transform _handsTransform;
        private static Camera _weaponCam;

        // Параметры idle-анимации
        private static float _idleValue = 0.5f;
        private static float _idlePhase;
        private static int _idleDir;
        private static float _idleTimer;

        public static void Initialize(GameObject hands, Camera weaponCam)
        {
            _animator = hands.GetComponent<Animator>();
            _handsTransform = hands.transform;
            _weaponCam = weaponCam;
        }

        public static void SetState(BookState state)
        {
            if (_animator == null) return;

            var trigger = state switch
            {
                BookState.Title => ModConstants.AnimBring,
                BookState.Open => ModConstants.AnimOpen,
                BookState.Pocket => ModConstants.AnimRemove,
                _ => null
            };

            if (!string.IsNullOrEmpty(trigger))
                _animator.SetTrigger(trigger);
        }

        public static void TriggerPageTurn(PageDirection dir)
        {
            if (_animator == null) return;
            var trigger = dir == PageDirection.Next ? ModConstants.AnimNext : ModConstants.AnimPrev;
            _animator.SetTrigger(trigger);
        }

        /// <returns>True, если нужно заблокировать ввод</returns>
        public static bool UpdateTilt(bool enableTilt, bool isBookOpen)
        {
            if (_handsTransform == null || _weaponCam == null) return false;

            float camX = _weaponCam.transform.rotation.eulerAngles.x;
            if (camX > 90f) camX = 0f;

            // Расчёт наклона (две формулы из оригинала)
            float tiltX = enableTilt
                ? 50f / Mathf.Pow(1f + (camX - 8f) / 50f, 1.2f) - 50f
                : 40f / Mathf.Pow(1f + camX / 45f, 2f) - 40f;

            float posY = (10f / Mathf.Pow(1f + camX / 55f, 1.7f) - 23f) / 100f;

            _handsTransform.localRotation = Quaternion.Euler(tiltX, 0f, 0f);
            _handsTransform.localPosition = new Vector3(0f, posY, 0.05f);

            // Блокировка ввода при взгляде вниз
            return camX >= ModConstants.AngleLockInput && isBookOpen;
        }

        public static void UpdateIdle()
        {
            if (_animator == null) return;

            _idleTimer += Time.deltaTime;
            if (_idleTimer < ModConstants.IdleUpdateInterval) return;
            _idleTimer = 0f;

            if (_idlePhase < ModConstants.IdlePhaseMax)
            {
                _idleValue += (_idleDir == 0) ? ModConstants.IdleStep : -ModConstants.IdleStep;
                _idleValue = Mathf.Clamp01(_idleValue);

                if (_idleValue >= 0.9f) _idleDir = 1;
                if (_idleValue <= 0.1f) _idleDir = 0;

                _animator.SetFloat(ModConstants.ParamIdleRandom, _idleValue);
                _idlePhase += 0.1f;
            }
            else
            {
                _idleDir = UnityEngine.Random.Range(0, 2);
                _idlePhase = 0f;

                if (UnityEngine.Random.value <= ModConstants.ScratchChance)
                    _animator.SetTrigger(ModConstants.TriggerScratch);
            }
        }

        public static bool IsClosing => _animator?.GetCurrentAnimatorStateInfo(0).IsName(ModConstants.AnimClose) == true ||
                                        _animator?.GetCurrentAnimatorStateInfo(0).IsName(ModConstants.AnimRemove) == true;

        public static bool IsIdle => _animator?.GetCurrentAnimatorStateInfo(0).IsName(ModConstants.AnimIdleOpen) == true ||
                                     _animator?.GetCurrentAnimatorStateInfo(0).IsName(ModConstants.AnimIdleTitle) == true;
    }
}