using rozvrh.Localization;

namespace rozvrh
{
    public class LocalizedStrings
    {
        private readonly AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources
        {
            get
            {
                return _localizedResources;
            }
        }
    }
}
