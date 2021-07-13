using System.Collections.Generic;

namespace XplaneControl
{
    public class FailSubPage
    {
        private bool sorted = false;
        public int Num = -1;
        public string Name = "";
        public List<int> Fails = new List<int>();
        public bool Exists(int num)
        {
            foreach (var fail in Fails)
                if (fail == num)
                    return true;
            return false;
        }
        public Fail Get(int num)
        {
            foreach (var fail in XplaneControl.Fails.FailsList)
            {
                if (fail.Num == num)
                    return fail;
            }
            return new Fail();
        }
    }
}