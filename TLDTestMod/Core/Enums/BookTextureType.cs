namespace TLDTestMod.Core.Enums
{
    public enum BookTextureType
    {
        SimpleRed,
        SimpleBlue,
        SimpleYellow,
        SimpleWhite,
        DetailYellow,
        DetailGray,
        DetailBlack,
    }

    public static class BookTextureTypeExtensions
    {
        public static string GetTexturePath(this BookTextureType type) => type switch
        {
            BookTextureType.SimpleRed => Constants.BookTextureSimpleRed,
            BookTextureType.SimpleBlue => Constants.BookTextureSimpleBlue,
            BookTextureType.SimpleYellow => Constants.BookTextureSimpleYellow,
            BookTextureType.SimpleWhite => Constants.BookTextureSimpleWhite,
            BookTextureType.DetailYellow => Constants.BookTextureDetailYellow,
            BookTextureType.DetailGray => Constants.BookTextureDetailGray,
            BookTextureType.DetailBlack => Constants.BookTextureDetailBlack,
            _ => Constants.BookTextureSimpleRed,
        };
    }
}