using NovaNet.wfe;
using System.Data;

namespace ImageHeaven.Logic
{
    public abstract class BaseLogic
    {
        public abstract DataTable GetImagesItems(eITEMS item, eSTATES[] state, string projKey, string bundleKey, string fileName);
        public abstract int GetFileCount(string projkey, string bundleKey);
    }
}
