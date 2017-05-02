using System.Collections.ObjectModel;

namespace UWPApp.Store
{
    public class RecordStore
    {
        private static RecordStore instance = null;
        private static readonly object padlock = new object();

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

        // 记录集合
        private ObservableCollection<Model.Record> AllRecords = new ObservableCollection<Model.Record>();
        public ObservableCollection<Model.Record> RECORDS { get { return AllRecords; } }

        private RecordStore()
        {
            for (int i = 0; i < 10; ++i)
            {
                AllRecords.Add(new Model.Record());
            }
        }
    }
}
