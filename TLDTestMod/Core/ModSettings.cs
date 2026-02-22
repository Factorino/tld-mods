using Il2Cpp;
using ModSettings;
using TLDTestMod.Components;
using TLDTestMod.Services;
using UnityEngine;

namespace TLDTestMod
{
    /// <summary>
    /// Глобальный доступ к настройкам мода.
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        /// Экземпляр настроек. Инициализируется в OnLoad().
        /// </summary>
        public static Options options;

        /// <summary>
        /// Вызывается при инициализации мода для регистрации настроек в меню ModSettings.
        /// </summary>
        public static void OnLoad()
        {
            options = new Options();
            options.AddToModSettings("Pastime Reading");
        }
    }

    /// <summary>
    /// Настройки мода PastimeReading (MVP-версия).
    /// Наследуется от JsonModSettings для автоматической сериализации и интеграции с ModSettings.
    /// </summary>
    internal class Options : JsonModSettings
    {
        #region === General ===

        [Section("General")]

        [Name("Open Key")]
        [Description("Key to open/close the book. Press once to take out, again to open.")]
        public KeyCode OpenKey = KeyCode.B;

        [Name("Font Size")]
        [Description("Size of the text on book pages.")]
        [Slider(12f, 24f, 1)]
        public float FontSize = 14f;

        [Name("Time Slowdown")]
        [Description("Slow down game time while reading for better immersion. 1.0 = normal speed.")]
        [Slider(0.25f, 1f, 4)]
        public float TimeScale = 1f;

        [Name("Enable Book Tilt")]
        [Description("Tilt the book based on camera angle for a more natural feel.")]
        public bool EnableTilt = true;

        #endregion

        #region === Internal State ===

        /// <summary>
        /// Флаг: изменились ли настройки с момента последнего применения.
        /// Используется для триггера перезагрузки визуальных параметров.
        /// </summary>
        internal bool SettingsChanged { get; set; }

        #endregion

        #region === Apply Changes ===

        /// <summary>
        /// Вызывается при нажатии CONFIRM в меню настроек.
        /// Применяет изменения и уведомляет систему о необходимости обновления.
        /// </summary>
        protected override void OnConfirm()
        {
            base.OnConfirm();

            // Помечаем, что настройки изменены — внешняя система обработает применение
            SettingsChanged = true;

            // Если игра ещё не запущена или книга не инициализирована — откладываем применение
            if (!BookController.IsReady) return;

            ApplyVisualChanges();
            ApplyTimeScale();

            // Сбрасываем флаг после успешного применения
            SettingsChanged = false;
        }

        /// <summary>
        /// Применяет изменения, требующие пересчёта визуальных компонентов.
        /// </summary>
        private void ApplyVisualChanges()
        {
            // Пересоздать страницы при изменении размера шрифта
            if (BookVisualService.CurrentFontSize != FontSize)
            {
                BookVisualService.UpdateFontSize(FontSize);
                BookVisualService.RebuildPages();
            }
        }

        /// <summary>
        /// Применяет изменение времени сразу, если книга открыта.
        /// </summary>
        private void ApplyTimeScale()
        {
            if (BookController.IsBookOpen)
            {
                Time.timeScale = GameManager.m_GlobalTimeScale = TimeScale;

                // Если время изменено — аниматор должен работать в unscaled time
                if (BookController.HandsAnimator != null)
                {
                    BookController.HandsAnimator.updateMode =
                        Mathf.Approximately(TimeScale, 1f)
                            ? AnimatorUpdateMode.Normal
                            : AnimatorUpdateMode.UnscaledTime;
                }
            }
        }

        #endregion
    }
}