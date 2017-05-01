namespace UWPApp.Store
{
    public class RecordStore
    {
        private static RecordStore instance = null;
        private static readonly object padlock = new object();

        private RecordStore () { }

        public static RecordStore getInstance ()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new RecordStore();
                }
                return instance;
            }
        }
    }
}
