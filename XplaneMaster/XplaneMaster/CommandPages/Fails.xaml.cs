using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XplaneControl;

namespace XplaneMaster.CommandPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fails : TabbedPage
    {
        public Fails()
        {
            InitializeComponent();
            Run();
            //Thread failDrawerThread = new Thread(Drawer);
            //failDrawerThread.Start();
        }

        private void Run()
        {
            return;//todo remove
            Thread failsDrawerThread = new Thread(() =>
            {
                while (Thread.CurrentThread.IsAlive)
                    DrawFailsButton();
            });
            failsDrawerThread.Start();
        }

        private void Drawer()
        {
            return;

            while (!XplaneControl.Fails.Ready)
            {
                Task.Delay(1000).Wait();
            }

            try
            {
                Application.Current.Properties["Fails"] = XplaneControl.Fails.Pages;
                foreach (FailPage page in XplaneControl.Fails.Pages)
                {
                    TabbedPage tabPage = new TabbedPage
                    {
                        Title = page.Name
                    };
                    foreach (FailSubPage pageFailSubPage in page.FailSubPages)
                    {
                        ContentPage subPage = new ContentPage
                        {
                            Title = pageFailSubPage.Name
                        };
                        Grid failsGrid = new Grid
                        {
                            RowDefinitions =
                                {
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                },
                            ColumnDefinitions =
                                {
                                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                                },
                            ColumnSpacing = 2
                        };
                        foreach (var i in pageFailSubPage.Fails)
                        {
                            Fail newFail = XplaneControl.Fails.GetFail(i);
                            Button btn = new MyButton()
                            {
                                Tag = newFail.Num.ToString(),
                                Text = newFail.Name,
                                IsEnabled = newFail.Visibility
                            };
                            btn.Clicked += ((object sender, EventArgs e) =>
                            {
                                MyButton btnButton = (MyButton)sender;
                                //Connection.SendMessage($"FAIL {btnButton.Tag}");
                            });
                            failsGrid.Children.Add(btn);
                        }

                        subPage.Content = failsGrid;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            tabPage.Children.Add(subPage);
                        });
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Children.Add(tabPage);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void DrawFailsButton()
        {
            Application.Current.Properties["Fails"] = XplaneControl.Fails.Pages;

            foreach (FailPage page in XplaneControl.Fails.Pages)
            {
                //основные страницы
                if (page.Num == Children.Count)//добавлены ли предыдущие страницы
                {
                    bool failsPageCreated = false;
                    TabbedPage FailsPage = new TabbedPage
                    {
                        Title = page.Name
                    };
                    foreach (TabbedPage child in Children)
                        if (child.Title == page.Name)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                FailsPage = child;//если такая страница уже существует, то использовать именно её
                            });
                            failsPageCreated = true;
                        }

                    foreach (FailSubPage FailsSubPage in page.FailSubPages)
                    {
                        //подстраницы
                        if (FailsSubPage.Num == FailsPage.Children.Count - 1 || FailsSubPage.Num == FailsPage.Children.Count)//добавлены ли предыдущие страницы
                        {
                            bool failsSubPageCreated = false;
                            ContentPage subPage = new ContentPage
                            {
                                Title = FailsSubPage.Name
                            };
                            Grid failsGrid = new Grid
                            {
                                RowDefinitions =
                                {
                                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                },
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                                },
                                ColumnSpacing = 2
                            };

                            int rows = 0;
                            foreach (ContentPage child in FailsPage.Children)
                                if (child.Title == FailsSubPage.Name)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        subPage = child;//если такая страница уже существует, то использовать именно её
                                    });
                                    failsSubPageCreated = true;
                                    failsGrid = (Grid)child.Content;
                                    rows = failsGrid.RowDefinitions.Count - 1;//считаем, сколько строк уже создано
                                }

                            foreach (int failNum in FailsSubPage.Fails)
                            {
                                Fail newFail = XplaneControl.Fails.GetFail(failNum);
                                foreach (MyButton button in failsGrid.Children)
                                    if (button.Tag != newFail.Num.ToString())
                                    {
                                        Button btn = new MyButton()
                                        {
                                            Tag = newFail.Num.ToString(),
                                            Text = newFail.Name,
                                            IsEnabled = newFail.Visibility
                                        };
                                        btn.Clicked += ((object sender, EventArgs e) =>
                                        {
                                            MyButton btnButton = (MyButton)sender;
                                            //sss.SendMessage($"FAIL {btnButton.Tag}"); //todo fix
                                        });
                                        failsGrid.Children.Add(btn);
                                    }
                            }

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (!failsSubPageCreated)
                                {
                                    subPage.Content = failsGrid;
                                    FailsPage.Children.Add(subPage);
                                }
                            });
                        }

                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (!failsPageCreated)
                            Children.Add(FailsPage);
                    });
                }
            }

            Thread.Sleep(5000);
        }
    }
}