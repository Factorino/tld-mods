using MelonLoader;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        public static MelonLogger.Instance logger = new MelonLogger.Instance(BuildInfo.Name);

        public override void OnInitializeMelon()
        {
            logger.Msg($"Version {Info.Version} loaded");
        }
    }
}
