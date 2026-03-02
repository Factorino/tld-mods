using Il2Cpp;
using MelonLoader;
using System.IO;
using UnityEngine;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        // === Состояние ===
        private bool _isInitialized;
        private bool _hasBook;
        private bool _bookOpen;

        // === Ассеты ===
        private AssetBundle _mainBundle;
        private AssetBundle _handsBundle;

        // === Объекты сцены ===
        private GameObject _handsObject;      // Префаб рук с книгой
        private Animator _handsAnimator;      // Аниматор рук
        private Camera _weaponCamera;         // Ванильная FP-камера
        private GameObject _handsF;           // Женские руки
        private GameObject _handsM;           // Мужские руки

        // === Константы (чтобы не искать по коду) ===
        private const string BundleMain = "assets.ass";
        private const string BundleHands = "handtex";
        private const string AssetHands = "hands_with_book";
        private const string PathWeaponCam = "/CHARACTER_FPSPlayer/WeaponView/WeaponCamera";
        private const string LayerWeapon = "Weapon";
        private const string ShaderName = "Shader Forge/TLD_StandardSkinned";
        private const string TexAstrid = "Assets/HMF_FP_Hands_Astrid.png";
        private const string TexWill = "Assets/HMM_FP_Hands_Will.png";

        // === Настройки ===
        private KeyCode _openKey = KeyCode.B;

        // === Точка входа ===
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("PastimeReading: initializing...");

            // 1. Определяем путь к папке Mods
            string? modsPath = Path.GetDirectoryName(typeof(Main).Assembly.Location);

            // 2. Загружаем AssetBundle'ы
            string mainPath = Path.Combine(modsPath!, "assets", BundleMain);
            string handsPath = Path.Combine(modsPath!, "assets", BundleHands);

            _mainBundle = AssetBundle.LoadFromFile(mainPath);
            _handsBundle = AssetBundle.LoadFromFile(handsPath);

            if (_mainBundle == null)
            {
                MelonLogger.Error($"Failed to load bundle: {mainPath}");
                return;
            }
            if (handsPath == null)
            {
                MelonLogger.Error($"Failed to load bundle: {handsPath}");
                return;
            }

            MelonLogger.Msg("PastimeReading: bundles loaded");
            _isInitialized = true;
        }

        // === Вызывается при загрузке каждой сцены ===
        public override void OnSceneWasInitialized(int level, string name)
        {
            // Работаем только в игровых сценах и только если ещё не инициализированы
            if (!_isInitialized || !IsPlayableScene(name) || _handsObject != null)
                return;

            SetupBookInScene();
        }

        // === Основной цикл обновления ===
        public override void OnUpdate()
        {
            // Если книга ещё не добавлена в сцену — ничего не делаем
            if (_handsObject == null) return;

            // === ОТКРЫТИЕ КНИГИ ПО НАЖАТИЮ КЛАВИШИ ===
            if (Input.GetKeyDown(_openKey) && !_bookOpen && !IsMenuOpen())
            {
                // Если книга в кармане — достаём её
                if (!_hasBook)
                {
                    TakeOutBook();
                }
                // Если книга в руках, но закрыта — открываем
                else if (!_bookOpen)
                {
                    OpenBook();
                }
            }

            // === ЗАКРЫТИЕ КНИГИ ===
            if (Input.GetKeyDown(KeyCode.H) && _bookOpen && !IsMenuOpen())
            {
                CloseBook();
            }

            // === ПЕРЕКЛЮЧЕНИЕ РУК ПОД ПЕРСОНАЖА ===
            UpdateHandsForCharacter();
        }

        // === Очистка при выходе ===
        public override void OnApplicationQuit()
        {
            Cleanup();
        }

        // =====================================================================
        // ▼▼▼ ОСНОВНАЯ ЛОГИКА ▼▼▼
        // =====================================================================

        /// <summary>
        /// Создаёт и настраивает объект книги в сцене.
        /// Вызывается один раз при загрузке игровой сцены.
        /// </summary>
        private void SetupBookInScene()
        {
            MelonLogger.Msg("PastimeReading: setting up book...");

            // 1. Находим ванильную камеру оружия
            _weaponCamera = GameObject.Find(PathWeaponCam)?.GetComponent<Camera>();
            if (_weaponCamera == null)
            {
                MelonLogger.Error("Weapon camera not found!");
                return;
            }

            // 2. Загружаем и инстанцируем префаб рук с книгой
            var handsPrefab = _mainBundle.LoadAsset<GameObject>(AssetHands);
            if (handsPrefab == null)
            {
                MelonLogger.Error($"Prefab '{AssetHands}' not found in bundle!");
                return;
            }

            _handsObject = GameObject.Instantiate(handsPrefab);
            _handsObject.name = "PastimeReading_Hands";

            // 3. Получаем аниматор
            _handsAnimator = _handsObject.GetComponent<Animator>();
            if (_handsAnimator == null)
            {
                MelonLogger.Error("Animator not found on hands prefab!");
                return;
            }

            // 4. Находим модели рук для переключения под персонажа
            _handsF = _handsObject.transform.Find("readingArmsF")?.gameObject;
            _handsM = _handsObject.transform.Find("readingArmsM")?.gameObject;

            // 5. Применяем ванильные текстуры к рукам (чтобы не отличались от игры)
            ApplyHandTextures();

            // 6. Настраиваем слой рендеринга (критично для отображения в FP-режиме)
            foreach (Transform child in _handsObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer(LayerWeapon);
            }

            // 7. Привязываем руки к камере игрока (чтобы двигались вместе с видом)
            _handsObject.transform.SetParent(_weaponCamera.transform, worldPositionStays: false);
            _handsObject.transform.localPosition = Vector3.zero;
            _handsObject.transform.localRotation = Quaternion.identity;

            // 8. Скрываем объект по умолчанию (книга в "кармане")
            _handsObject.SetActive(false);

            MelonLogger.Msg("PastimeReading: book setup complete");
        }

        /// <summary>
        /// Применяет ванильные текстуры к моделям рук.
        /// </summary>
        private void ApplyHandTextures()
        {
            var shader = Shader.Find(ShaderName);
            if (shader == null)
            {
                MelonLogger.Warning($"Shader '{ShaderName}' not found, using default");
                return;
            }

            var texA = _handsBundle?.LoadAsset<Texture2D>(TexAstrid);
            var texW = _handsBundle?.LoadAsset<Texture2D>(TexWill);

            ApplyTexture(_handsF, texA, shader);
            ApplyTexture(_handsM, texW, shader);
        }

        /// <summary>
        /// Вспомогательный метод: применяет текстуру и шейдер к мешу рук.
        /// </summary>
        private void ApplyTexture(GameObject handsMesh, Texture2D texture, Shader shader)
        {
            if (handsMesh == null || texture == null) return;

            var renderer = handsMesh.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null) return;

            renderer.material.shader = shader;
            renderer.material.mainTexture = texture;
        }

        /// <summary>
        /// === ГЛАВНЫЙ МЕТОД: достаёт книгу из "кармана" ===
        /// Проигрывает анимацию bring_book и показывает объект.
        /// </summary>
        private void TakeOutBook()
        {
            if (_handsObject == null || _handsAnimator == null) return;

            // Показываем объект
            _handsObject.SetActive(true);

            // Запускаем анимацию "достать книгу"
            _handsAnimator.SetTrigger("bring_book");

            // Обновляем состояние
            _hasBook = true;
            _bookOpen = false;

            // Корректируем FOV камеры для лучшего обзора книги (как в оригинале)
            _weaponCamera.fieldOfView = 37.5f;

            MelonLogger.Msg("PastimeReading: book taken out");
        }

        /// <summary>
        /// Открывает книгу (показывает обложку/текст).
        /// </summary>
        private void OpenBook()
        {
            if (_handsAnimator == null) return;

            _handsAnimator.SetTrigger("open_book");
            _bookOpen = true;

            MelonLogger.Msg("PastimeReading: book opened");
        }

        /// <summary>
        /// Закрывает книгу и убирает её.
        /// </summary>
        private void CloseBook()
        {
            if (_handsAnimator == null) return;

            // Если книга открыта — сначала закрываем, потом убираем
            if (_bookOpen)
            {
                _handsAnimator.SetTrigger("close_book");
                _bookOpen = false;
                // Книга останется в руках, но закрытой
            }
            else if (_hasBook)
            {
                // Если уже закрыта — убираем в карман
                _handsAnimator.SetTrigger("remove_book");
                _hasBook = false;

                // Возвращаем FOV камеры
                if (_weaponCamera != null)
                    _weaponCamera.fieldOfView = 60f; // стандартный FOV в TLD
            }

            MelonLogger.Msg("PastimeReading: book closed/removed");
        }

        /// <summary>
        /// Переключает видимую модель рук в зависимости от персонажа.
        /// </summary>
        private void UpdateHandsForCharacter()
        {
            if (_handsF == null || _handsM == null) return;

            // Проверяем текущий голос персонажа (Astrid = женский, Will = мужской)
            if (PlayerManager.m_VoicePersona == VoicePersona.Female)
            {
                if (!_handsF.activeSelf)
                {
                    _handsF.SetActive(true);
                    _handsM.SetActive(false);
                }
            }
            else if (PlayerManager.m_VoicePersona == VoicePersona.Male)
            {
                if (!_handsM.activeSelf)
                {
                    _handsM.SetActive(true);
                    _handsF.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Проверяет, не открыто ли меню/инвентарь (чтобы не открывать книгу поверх UI).
        /// </summary>
        private bool IsMenuOpen()
        {
            return InterfaceManager.IsOverlayActiveCached();
        }

        /// <summary>
        /// Проверяет, является ли сцена игровой (а не меню или загрузкой).
        /// </summary>
        private bool IsPlayableScene(string sceneName)
        {
            // Список не-игровых сцен в TLD
            if (string.IsNullOrEmpty(sceneName)) return false;
            if (sceneName.Contains("Menu") || sceneName.Contains("Loading")) return false;
            return true;
        }

        /// <summary>
        /// Очищает ресурсы при выходе.
        /// </summary>
        private void Cleanup()
        {
            if (_handsObject != null) GameObject.Destroy(_handsObject);
            _mainBundle?.Unload(true);
            _handsBundle?.Unload(true);

            _handsObject = null;
            _mainBundle = null;
            _handsBundle = null;

            MelonLogger.Msg("PastimeReading: cleaned up");
        }
    }
}