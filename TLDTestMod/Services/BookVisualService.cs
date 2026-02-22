using Il2CppTMPro;
using PastimeReading.Core;
using System;
using TLDTestMod.Core.Enums;

namespace TLDTestMod.Services
{
    public static class BookVisualService
    {
        private static string[] _pages;
        private static int _currentPage = 1;

        public static float _currentFontSize = ModConstants.DefaultFontSize;

        private static string _cachedContent = ModConstants.PlaceholderContent;

        public static float CurrentFontSize => _currentFontSize;

        public static void Initialize(string content, float fontSize)
        {
            _cachedContent = content;
            _currentFontSize = fontSize;
            _pages = content.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            _currentPage = 1;
        }

        public static void UpdateFontSize(float newSize)
        {
            _currentFontSize = newSize;
            // Здесь можно кэшировать или пересчитать метрики текста при необходимости
        }

        public static void RebuildPages()
        {
            // Пересобираем страницы с новым размером шрифта
            // Упрощённая версия: просто сбрасываем и инициализируем заново
            Initialize(_cachedContent, _currentFontSize);
        }

        public static void RenderPages(TMP_Text left, TMP_Text right, TMP_Text leftNum, TMP_Text rightNum)
        {
            int leftIdx = _currentPage - 1;
            int rightIdx = _currentPage;

            left.text = leftIdx < _pages.Length ? _pages[leftIdx] : string.Empty;
            right.text = rightIdx < _pages.Length ? _pages[rightIdx] : string.Empty;

            leftNum.text = _currentPage.ToString();
            rightNum.text = (_currentPage + 1 <= _pages.Length) ? (_currentPage + 1).ToString() : string.Empty;
        }

        public static bool CanTurn(PageDirection dir) => dir switch
        {
            PageDirection.Next => _currentPage + 1 < _pages.Length,
            PageDirection.Prev => _currentPage > 1,
            _ => false
        };

        public static void Turn(PageDirection dir)
        {
            if (dir == PageDirection.Next && CanTurn(PageDirection.Next))
                _currentPage += 2;
            else if (dir == PageDirection.Prev && CanTurn(PageDirection.Prev))
                _currentPage -= 2;
        }

        public static int CurrentPage => _currentPage;
        public static int TotalPages => _pages?.Length ?? 0;
    }
}