using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;

namespace RegExHelper.Data
{
    public class IsoStore
    {
        const string isoStoreHistoryFileName = "history.txt";
        IsolatedStorageFile isoStore;

        public IsoStore()
        {
            isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        }

        public void CreateIsoStoreFiles()
        {
            if (!isoStore.FileExists(isoStoreHistoryFileName))
                isoStore.CreateFile(isoStoreHistoryFileName);
        }

        public void StoreHistory(string text)
        {
            if (isoStore.FileExists(isoStoreHistoryFileName))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(isoStoreHistoryFileName, FileMode.Append, isoStore))
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(text);
                }
            }
        }
        internal List<String> GetHistory(bool reversed)
        {
            var history = new List<string>();

            if (isoStore.FileExists(isoStoreHistoryFileName))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(isoStoreHistoryFileName, FileMode.Open, isoStore))
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    while (!reader.EndOfStream)
                        history.Add(reader.ReadLine());
                }
            }

            if (reversed)
                history.Reverse();

            return history;
        }

        public void UpdateHistory(System.Windows.Controls.ComboBox control)
        {
            control.Items.Clear();

            foreach (string s in GetHistory(true))
            {
                control.Items.Add(s);
            }
        }

        public void ClearHistory(System.Windows.Controls.ComboBox control)
        {
            control.Items.Clear();
            if (isoStore.FileExists(isoStoreHistoryFileName))
            {
                File.Delete(GetIsoStorePath());
                CreateIsoStoreFiles();
            }
        }

        public void RemoveItem(System.Windows.Controls.ComboBox control, object item)
        {
            var history = GetHistory(false);
            history.Remove(item.ToString());

            File.WriteAllLines(GetIsoStorePath(), history);
        }

        private string GetIsoStorePath()
        {
            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(isoStoreHistoryFileName, FileMode.Open, isoStore);
            string absolutePath = typeof(IsolatedStorageFileStream).GetField("m_FullPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(isoStream).ToString();
            isoStream.Close();
            isoStream = null;

            return absolutePath;
        }
    }
}
