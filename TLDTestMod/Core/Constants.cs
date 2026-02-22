namespace PastimeReading.Core
{
    public static class ModConstants
    {
        // === AssetBundle и ассеты
        public const string BundleMain = "assets.ass";
        public const string BundleHands = "handtex";

        public const string AssetHandsPrefab = "hands_with_book";
        public const string AssetPageCamera = "p1-2Cam";
        public const string AssetTurnCamera = "h1-2Cam";
        public const string AssetCoverCamera = "coverCam";

        public const string TextureAstrid = "Assets/HMF_FP_Hands_Astrid.png";
        public const string TextureWill = "Assets/HMM_FP_Hands_Will.png";

        // === Имена объектов в иерархии префаба ===
        public const string ObjHandsF = "readingArmsF";
        public const string ObjHandsM = "readingArmsM";
        public const string ObjBook = "readingBook";
        public const string ObjTurnPage = "readingBook_turnpage";
        public const string ObjPage1 = "readingBook_textField_p1";
        public const string ObjPage2 = "readingBook_textField_p2";
        public const string ObjPageH1 = "readingBook_textField_h1";
        public const string ObjPageH2 = "readingBook_textField_h2";

        // === Пути в сцене ===
        public const string PathWeaponCamera = "/CHARACTER_FPSPlayer/WeaponView/WeaponCamera";
        public const string LayerWeapon = "Weapon";

        // === Шейдеры ===
        public const string ShaderSkinned = "Shader Forge/TLD_StandardSkinned";

        // === Анимации и параметры Animator ===
        public const string AnimOpen = "open_book";
        public const string AnimBring = "bring_book";
        public const string AnimClose = "close_book";
        public const string AnimRemove = "remove_book";
        public const string AnimNext = "next_page";
        public const string AnimPrev = "prev_page";
        public const string AnimIdleOpen = "book_open_idle";
        public const string AnimIdleTitle = "book_title_idle";

        public const string ParamIdleRandom = "idle_random";
        public const string TriggerScratch = "scratch_ear";

        // === TMP: индексы детей для GetChild() ===
        public const int IdxTitle = 0;
        public const int IdxAuthor = 1;
        public const int IdxText = 2;
        public const int IdxNumber = 3;

        // === Магические числа (вынесены) ===
        public const float AngleLockInput = 27f;
        public const float DefaultFOV = 37.5f;
        public const float IdleUpdateInterval = 0.1f;
        public const float IdleStep = 0.03f;
        public const float IdlePhaseMax = 1f;
        public const float ScratchChance = 0.001f;
        public const float TiltLerpSpeed = 2f;
        public const float InterruptLerpSpeed = 0.3f;

        // В ModConstants.cs добавьте:
        public const float DefaultFontSize = 14f;
        public const float DefaultTimeScale = 1f;
        public const bool DefaultEnableTilt = true;

        // === Placeholder-текст книги ===
        public const string PlaceholderContent =
            "Welcome to Pastime Reading mod for The Long Dark.\n\n" +
            "This is a simplified version with placeholder text.\n\n" +
            "Use your configured key to open the book.\n\n" +
            "Rotate view to turn pages — enjoy reading in the wild!\n\n" +
            "Stay warm, stay alive, and happy reading.";
    }
}