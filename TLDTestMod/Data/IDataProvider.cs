using TLDTestMod.Data.Entities;

namespace TLDTestMod.Data
{
    public interface IDataProvider
    {
        public SessionData Load();
    }
}
