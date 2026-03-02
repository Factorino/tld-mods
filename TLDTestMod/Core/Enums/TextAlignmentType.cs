using Il2CppTMPro;

namespace TLDTestMod.Core.Enums
{
    public enum TextAlignmentType
    {
        TopJustified,
        TopLeft,
        TopRight,
        TopCenter
    }

    public static class TextAlignmentTypeExtensions
    {
        public static TextAlignmentOptions ToTMPAlignment(this TextAlignmentType alignment) => alignment switch
        {
            TextAlignmentType.TopJustified => TextAlignmentOptions.TopJustified,
            TextAlignmentType.TopLeft => TextAlignmentOptions.TopLeft,
            TextAlignmentType.TopRight => TextAlignmentOptions.TopRight,
            TextAlignmentType.TopCenter => TextAlignmentOptions.Top,
            _ => TextAlignmentOptions.TopLeft
        };
    }
}