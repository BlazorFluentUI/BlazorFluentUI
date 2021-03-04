using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class LayerHostService
    {
        Dictionary<string, BFULayerHost> hosts = new Dictionary<string, BFULayerHost>();
        Dictionary<string, BehaviorSubject<BFULayerHost>> hostSubjects = new Dictionary<string, BehaviorSubject<BFULayerHost>>();

        BFULayerHost? rootHost;

        

        public void RegisterHost(BFULayerHost host)
        {
            if (host.Id == null)
            {
                if (rootHost != null)
                {
                    throw new Exception("You must specify an Id for your host.");
                }
                rootHost = host;
            }
            else
            {
                hosts.Add(host.Id, host);
                if (hostSubjects.ContainsKey(host.Id))
                {
                    var subject = hostSubjects[host.Id];
                    subject.OnNext(host);
                }
            }
        }

        public BFULayerHost GetHost(string id)
        {
            BFULayerHost host = null;
            if (hosts.ContainsKey(id))
                host = hosts[id];

            return host;
        }

        public IObservable<BFULayerHost> GetHostObs(string id)
        {
            BehaviorSubject<BFULayerHost> subject = null;
            if (hostSubjects.ContainsKey(id))
                subject = hostSubjects[id];
            else
            {
                BFULayerHost host = null;
                if (hosts.ContainsKey(id))
                    host = hosts[id];
                subject = new BehaviorSubject<BFULayerHost>(host);
                hostSubjects.Add(id, subject);
            }
            return subject.AsObservable();
            
        }

        public void RemoveHost(BFULayerHost host)
        {
            if (host.Id != null)
            {
                if (hostSubjects.ContainsKey(host.Id))
                {
                    var subject = hostSubjects[host.Id];
                    subject.OnCompleted();
                    hostSubjects.Remove(host.Id);
                }
                if (hosts.ContainsKey(host.Id))
                {
                    hosts.Remove(host.Id);
                }
            }
        }

        public BFULayerHost GetDefaultHost()
        {
            //if (rootHost == null)
                //throw new Exception("You need to add a BFULayerHost somewhere after your Router root component.");
            return rootHost;
        }
    }
}
