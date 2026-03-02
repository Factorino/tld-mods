using Il2Cpp;

namespace TLDTestMod.Core.Enums
{
    public enum CharacterType
    {
        Unknown,
        Will,
        Astrid,
    }

    public static class CharacterTypeExtension
    {
        public static CharacterType ToCharacterType(this VoicePersona persona) => persona switch
        {
            VoicePersona.Male => CharacterType.Will,
            VoicePersona.Female => CharacterType.Astrid,
            _ => CharacterType.Unknown,
        };

        public static bool MatchCurrentPlayer(this CharacterType type) => type switch
        {
            CharacterType.Will => PlayerManager.m_VoicePersona == VoicePersona.Male,
            CharacterType.Astrid => PlayerManager.m_VoicePersona == VoicePersona.Female,
            _ => false,
        };
    }
}
