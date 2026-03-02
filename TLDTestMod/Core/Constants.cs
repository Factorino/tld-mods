using System.IO;

namespace TLDTestMod.Core
{
    public static class Constants
    {
        public static string? ModsPath => Path.GetDirectoryName(typeof(Main).Assembly.Location);

        public const string MainAssetBundle = "assets.ass";
        public const string HandsAssetBundle = "handtex";

        public const string BookTextureSimpleRed = "texture/simpleRed.png";
        public const string BookTextureSimpleBlue = "texture/simpleBlue.png";
        public const string BookTextureSimpleYellow = "texture/simpleYellow.png";
        public const string BookTextureSimpleWhite = "texture/simpleWhite.png";
        public const string BookTextureDetailYellow = "texture/detailYellow.png";
        public const string BookTextureDetailGray = "texture/detailGray.png";
        public const string BookTextureDetailBlack = "texture/detailBlack.png";
    }
}
