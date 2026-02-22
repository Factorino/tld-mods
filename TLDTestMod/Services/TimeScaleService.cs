using Il2Cpp;
using PastimeReading.Core;
using UnityEngine;

namespace TLDTestMod.Services
{
    public static class TimeScaleService
    {
        private static float _lerp;
        private static bool _resetPending;

        public static void Update(bool reading, float targetScale, bool interrupted)
        {
            if (reading)
            {
                if (!Mathf.Approximately(targetScale, Time.timeScale))
                {
                    if (Mathf.Approximately(1f, Time.timeScale)) _lerp = 0f;

                    Time.timeScale = GameManager.m_GlobalTimeScale =
                        Mathf.Lerp(1f, targetScale, _lerp += Time.unscaledDeltaTime / ModConstants.TiltLerpSpeed);
                }
            }
            else
            {
                if (_resetPending) return;

                if (!Mathf.Approximately(1f, Time.timeScale))
                {
                    if (Mathf.Approximately(Time.timeScale, targetScale)) _lerp = 0f;

                    float speed = interrupted ? ModConstants.InterruptLerpSpeed : ModConstants.TiltLerpSpeed;
                    Time.timeScale = GameManager.m_GlobalTimeScale =
                        Mathf.Lerp(targetScale, 1f, _lerp += Time.unscaledDeltaTime / speed);
                }
                else
                {
                    _resetPending = true;
                }
            }
        }

        public static void ResetComplete() => _resetPending = false;
    }
}