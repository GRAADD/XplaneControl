using System.Collections.Generic;
using System.Threading.Tasks;

namespace XplaneControl
{
    public static class Fails
    {
        public static List<Fail> FailsList = new List<Fail>();
        public static List<FailPage> Pages = new List<FailPage>();
        private static int _readyCount = 0;
        public static bool Ready = false;
        private static bool sorted = false;

        public static bool PageExists(int num)
        {
            foreach (var page in Pages)
                if (page.Num == num)
                    return true;
            return false;
        }
        public static bool FailExists(int num)
        {
            foreach (var fail in FailsList)
                if (fail.Num == num)
                    return true;
            return false;
        }
        public static FailPage Get(int num)
        {
            foreach (var page in Pages)
            {
                if (page.Num == num)
                    return page;
            }
            return new FailPage();
        }
        public static Fail GetFail(int num)
        {
            foreach (var fail in FailsList)
            {
                if (fail.Num == num)
                    return fail;
            }
            return new Fail();
        }

        public static void Sort()
        {
            if (sorted)
                return;
            List<Fail> temp = new List<Fail>();
            for (int i = 0; i < FailsList.Count; i++)
                temp.Add(GetFail(i));
            FailsList = temp;
        }

        public static Task Add(BytesPackage message)
        {
            string[] splitted = message.MessageString.Split(' ');
            if (splitted.Length > 6)
            {
                if (splitted[0] == "-1")
                {
                    AddPage(splitted);
                }
                else
                {
                    AddFail(splitted);
                }
            }
            Sort();
            return Task.CompletedTask;
        }

        private static void AddFail(string[] splitted)
        {
            int num = int.Parse(splitted[0]);
            string name = "";
            for (int i = 6; i < splitted.Length; i++)
            {
                name += splitted[i] + " ";
            }
            name = name.Substring(0, name.Length - 1);
            Fail fail = new Fail()
            {
                Name = name,
                Num = num,
                Page = int.Parse(splitted[1]),
                SubPage = int.Parse(splitted[2]),
                Position = int.Parse(splitted[3]),
                SubPosition = int.Parse(splitted[4]),
                Visibility = int.Parse(splitted[5]) == 0
            };
            if (!FailExists(num))
            {
                FailsList.Add(fail);
                _readyCount = 0;
            }
            else
            {
                if (_readyCount > 10)
                    Ready = true;
                else
                    _readyCount++;
                //Fail temp = Fails.GetFail(num);
                //temp = fail;
            }

        }

        private static void AddPage(string[] splitted)
        {
            int num = int.Parse(splitted[0]);
            string name = "";
            for (int i = 6; i < splitted.Length; i++)
            {
                name += splitted[i] + " ";
            }
            name = name.Substring(0, name.Length - 1);

            int pageNum = int.Parse(splitted[1]);
            int subPageNum = int.Parse(splitted[2]);

            if (splitted[2] == "-1")
            {
                if (!PageExists(pageNum))
                {
                    FailSubPage failSubPage = new FailSubPage();
                    List<FailSubPage> subPages = new List<FailSubPage>();
                    if (name == "World" || name == "Wings" || name.ToLower() == "multi rotors")
                    {
                        failSubPage.Name = name;
                        failSubPage.Num = -1;
                        subPages.Add(failSubPage);
                    }

                    Pages.Add(new FailPage()
                    {
                        Name = name,
                        Num = pageNum,
                        FailSubPages = subPages
                    });
                }
            }
            else
            {
                FailPage failPage = Get(pageNum);
                if (!failPage.Exists(subPageNum))
                {
                    failPage.FailSubPages.Add(new FailSubPage()
                    {
                        Name = name,
                        Num = subPageNum
                    });
                }
            }

        }
    }
}