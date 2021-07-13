using System.Collections.Generic;

namespace XplaneControl
{
    public class FailPage
    {
        private bool sorted = false;
        public int Num = -1;
        public string Name = "";
        public List<FailSubPage> FailSubPages = new List<FailSubPage>();
        public bool Exists(int num)
        {
            foreach (var page in FailSubPages)
                if (page.Num == num)
                    return true;
            return false;
        }

        public FailSubPage Get(int num)
        {
            foreach (var subPage in FailSubPages)
            {
                if (subPage.Num == num)
                    return subPage;
            }
            return new FailSubPage();
        }
        public void Sort()
        {
            if (sorted)
                return;
            List<FailSubPage> temp = new List<FailSubPage>();
            for (int i = 0; i < FailSubPages.Count; i++)
                temp.Add(Get(i));
            FailSubPages = temp;
        }
    }
}