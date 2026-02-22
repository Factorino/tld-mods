using Il2Cpp;
using TLDTestMod.Core.Enums;
using UnityEngine;

namespace TLDTestMod.Services
{
    public static class CharacterService
    {
        private static CharacterVariant _current;

        public static void UpdateHands(GameObject handsF, GameObject handsM)
        {
            var persona = PlayerManager.m_VoicePersona;

            if (persona == VoicePersona.Female && _current != CharacterVariant.Astrid)
            {
                handsM?.SetActive(false);
                handsF?.SetActive(true);
                _current = CharacterVariant.Astrid;
            }
            else if (persona == VoicePersona.Male && _current != CharacterVariant.Will)
            {
                handsF?.SetActive(false);
                handsM?.SetActive(true);
                _current = CharacterVariant.Will;
            }
        }

        public static void ApplyTexture(GameObject mesh, Texture2D tex, Shader shader)
        {
            if (mesh?.GetComponent<SkinnedMeshRenderer>() is var renderer && renderer != null)
            {
                renderer.material.shader = shader;
                renderer.material.mainTexture = tex;
            }
        }
    }
}