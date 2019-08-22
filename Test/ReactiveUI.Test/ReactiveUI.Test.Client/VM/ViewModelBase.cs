using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUI.Test.Client.VM
{
    public class ViewModelBase : ReactiveUI.ReactiveObject
    {
        private string testProp = "";
        public string TestProp { get => testProp; set => this.RaiseAndSetIfChanged(ref testProp, value); }

        private string testProp2 = "";
        public string TestProp2 { get => testProp2; set => this.RaiseAndSetIfChanged(ref testProp2, value); }

        public int othercount = 100;

        public ViewModelBase()
        {
            this.WhenAnyValue(x => x.TestProp).ObserveOn(RxApp.MainThreadScheduler).Subscribe(change =>
            {
                othercount++;
                TestProp2 = othercount.ToString();
                
            });  
        }
    }
}
