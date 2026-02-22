using Il2CppTMPro;
using PastimeReading.Core;
using TLDTestMod.Core.Enums;
using TLDTestMod.Services;
using UnityEngine;

namespace TLDTestMod.Components
{
    public static class BookController
    {
        // Публичные ссылки для доступа из других сервисов
        public static GameObject Hands { get; private set; }
        public static Animator HandsAnimator { get; private set; }
        public static Camera WeaponCamera { get; private set; }

        private static BookState _state = BookState.Pocket;
        private static bool _isOpen;
        private static bool _closing;
        private static bool _interrupted;
        private static bool _ready;

        public static bool IsReady => _ready;
        public static bool IsBookOpen => _isOpen && _state == BookState.Open;

        public static void Initialize(string modsPath)
        {
            AssetService.Load(modsPath);
            if (AssetService.MainBundle == null) return;

            InstantiateAssets();
            SetupHierarchy();
            SetupVisuals();
            InitializeServices();

            _ready = true;
        }

        private static void InstantiateAssets()
        {
            // Загрузка префабов
            var handsPrefab = AssetService.LoadAsset<GameObject>(ModConstants.AssetHandsPrefab);
            Hands = UnityEngine.Object.Instantiate(handsPrefab);

            var pCamPrefab = AssetService.LoadAsset<GameObject>(ModConstants.AssetPageCamera);
            var hCamPrefab = AssetService.LoadAsset<GameObject>(ModConstants.AssetTurnCamera);
            var cCamPrefab = AssetService.LoadAsset<GameObject>(ModConstants.AssetCoverCamera);

            var pCam = UnityEngine.Object.Instantiate(pCamPrefab);
            var hCam = UnityEngine.Object.Instantiate(hCamPrefab);
            var cCam = UnityEngine.Object.Instantiate(cCamPrefab);

            // Поиск ванильной камеры
            WeaponCamera = GameObject.Find(ModConstants.PathWeaponCamera)?.GetComponent<Camera>();
            if (WeaponCamera != null)
                WeaponCamera.fieldOfView = ModConstants.DefaultFOV;

            // Сохраняем ссылки для других сервисов (в упрощённой версии — статика)
            // В полной версии можно использовать DI-контейнер
        }

        private static void SetupHierarchy()
        {
            // Родительские связи
            Hands.transform.SetParent(WeaponCamera?.transform);

            foreach (Transform child in Hands.GetComponentsInChildren<Transform>())
                child.gameObject.layer = LayerMask.NameToLayer(ModConstants.LayerWeapon);
        }

        private static void SetupVisuals()
        {
            // Поиск компонентов
            HandsAnimator = Hands.GetComponent<Animator>();

            var pCam = Hands.transform.Find(ModConstants.AssetPageCamera)?.gameObject;
            var cCam = Hands.transform.Find(ModConstants.AssetCoverCamera)?.gameObject;

            if (pCam == null || cCam == null) return;

            // TMP-компоненты
            var titleText = cCam.transform.GetChild(ModConstants.IdxTitle).GetComponent<TMP_Text>();
            var authorText = cCam.transform.GetChild(ModConstants.IdxAuthor).GetComponent<TMP_Text>();
            var p1Text = pCam.transform.GetChild(ModConstants.IdxTitle).GetComponent<TMP_Text>();
            var p2Text = pCam.transform.GetChild(ModConstants.IdxAuthor).GetComponent<TMP_Text>();
            var p1Num = pCam.transform.GetChild(ModConstants.IdxText).GetComponent<TMP_Text>();
            var p2Num = pCam.transform.GetChild(ModConstants.IdxNumber).GetComponent<TMP_Text>();

            // Настройка шрифта
            foreach (var txt in new[] { p1Text, p2Text })
            {
                txt.fontSize = Settings.options.FontSize;
                txt.alignment = TextAlignmentOptions.TopJustified; // упрощение
            }

            // Инициализация контента книги
            BookVisualService.Initialize(ModConstants.PlaceholderContent, Settings.options.FontSize);
            BookVisualService.RenderPages(p1Text, p2Text, p1Num, p2Num);

            titleText.text = "Placeholder Book";
            authorText.text = "PastimeReading Mod";

            // Настройка текстур рук
            var handsF = Hands.transform.Find(ModConstants.ObjHandsF)?.gameObject;
            var handsM = Hands.transform.Find(ModConstants.ObjHandsM)?.gameObject;
            var shader = Shader.Find(ModConstants.ShaderSkinned);

            var texAstrid = AssetService.LoadAsset<Texture2D>(ModConstants.TextureAstrid, true);
            var texWill = AssetService.LoadAsset<Texture2D>(ModConstants.TextureWill, true);

            CharacterService.ApplyTexture(handsF, texAstrid, shader);
            CharacterService.ApplyTexture(handsM, texWill, shader);
        }

        private static void InitializeServices()
        {
            AnimationService.Initialize(Hands, WeaponCamera);
        }

        public static void Update()
        {
            if (!_ready) return;

            // === НОВОЕ: Применение изменённых настроек ===
            if (Settings.options.SettingsChanged)
            {
                // Настройки применяются в Settings.OnConfirm(), 
                // но здесь можно добавить реактивную логику при необходимости
            }

            // Обновление персонажа
            var handsF = Hands?.transform.Find(ModConstants.ObjHandsF)?.gameObject;
            var handsM = Hands?.transform.Find(ModConstants.ObjHandsM)?.gameObject;
            CharacterService.UpdateHands(handsF, handsM);

            // Открытие/закрытие книги
            if (InputService.IsOpenBookPressed(Settings.options.OpenKey) && !_closing)
            {
                if (_state == BookState.Title)
                {
                    AnimationService.SetState(BookState.Open);
                    _state = BookState.Open;
                    _isOpen = true;
                    _interrupted = false;
                }
                else if (_state == BookState.Pocket)
                {
                    AnimationService.SetState(BookState.Title);
                    _state = BookState.Title;
                }
            }

            // Логика при активной книге
            if (_state is BookState.Open or BookState.Title)
            {
                AnimationService.UpdateTilt(Settings.options.EnableTilt, _isOpen);

                if (_isOpen && AnimationService.IsIdle)
                    AnimationService.UpdateIdle();

                // Перелистывание
                if (_isOpen)
                {
                    if (InputService.IsTurnNext() && BookVisualService.CanTurn(PageDirection.Next))
                    {
                        BookVisualService.Turn(PageDirection.Next);
                        AnimationService.TriggerPageTurn(PageDirection.Next);
                        // TODO: Обновить текст после анимации
                    }
                    if (InputService.IsTurnPrev() && BookVisualService.CanTurn(PageDirection.Prev))
                    {
                        BookVisualService.Turn(PageDirection.Prev);
                        AnimationService.TriggerPageTurn(PageDirection.Prev);
                    }
                }

                // Закрытие/прерывание
                if (InputService.HasItemInHands() || InputService.IsInStruggle() || InputService.IsClosePressed())
                {
                    AnimationService.SetState(BookState.Pocket);

                    if (_state == BookState.Open)
                    {
                        _isOpen = false;
                        _interrupted = true;
                    }
                    _state = BookState.Pocket;
                }
            }

            // Управление временем
        }

        public static void OnSceneUnload()
        {
            _state = BookState.Pocket;
            _isOpen = false;
            _closing = false;
            _ready = false;
            TimeScaleService.ResetComplete();
        }

        public static void Cleanup()
        {
            AssetService.UnloadAll();
            if (Hands != null) UnityEngine.Object.Destroy(Hands);
        }
    }
}