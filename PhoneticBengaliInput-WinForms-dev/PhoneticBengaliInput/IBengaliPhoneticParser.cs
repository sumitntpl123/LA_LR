using System.Threading.Tasks;
using System;

namespace PhoneticBengaliInput
{
    public interface IBengaliPhoneticParser
    {
        Task<string> TranslateToBengali(string english);
    }

    public class BengaliPhoneticParser
    {
        private static IBengaliPhoneticParser Parser;

        public delegate Task<IBengaliPhoneticParser> InitInstance();

        private static InitInstance InstanceInitializer;

        public static void RegisterInstanceInitializer(InitInstance init)
        {
            InstanceInitializer = init;
        }
        public static async Task<IBengaliPhoneticParser> GetInstance()
        {
            if (Parser == null)
            {
                if (InstanceInitializer == null)
                {
                    throw new Exception("No Bengali phonetic parser is registered");
                }
                Parser = await InstanceInitializer();
            }
            return Parser;
        }
    }
}
