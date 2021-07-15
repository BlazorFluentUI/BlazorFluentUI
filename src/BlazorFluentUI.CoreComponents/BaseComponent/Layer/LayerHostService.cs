using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BlazorFluentUI
{
    public class LayerHostService
    {
        Dictionary<string, LayerHost> hosts = new();
        Dictionary<string, BehaviorSubject<LayerHost>> hostSubjects = new();

        LayerHost? rootHost;



        public void RegisterHost(LayerHost host)
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
                    BehaviorSubject<LayerHost>? subject = hostSubjects[host.Id];
                    subject.OnNext(host);
                }
            }
        }

        public LayerHost? GetHost(string id)
        {
            LayerHost? host = null;
            if (hosts.ContainsKey(id))
                host = hosts[id];

            return host;
        }

        public IObservable<LayerHost> GetHostObs(string id)
        {
            BehaviorSubject<LayerHost>? subject;
            if (hostSubjects.ContainsKey(id))
                subject = hostSubjects[id];
            else
            {
                LayerHost? host = null;
                if (hosts.ContainsKey(id))
                    host = hosts[id];

                subject = new BehaviorSubject<LayerHost>(host!);
                hostSubjects.Add(id, subject);
            }
            return subject.AsObservable();

        }

        public void RemoveHost(LayerHost host)
        {
            if (host.Id != null)
            {
                if (hostSubjects.ContainsKey(host.Id))
                {
                    BehaviorSubject<LayerHost>? subject = hostSubjects[host.Id];
                    subject.OnCompleted();
                    hostSubjects.Remove(host.Id);
                }
                if (hosts.ContainsKey(host.Id))
                {
                    hosts.Remove(host.Id);
                }
            }
        }

        public LayerHost? GetDefaultHost()
        {
            //if (rootHost == null)
            //throw new Exception("You need to add a LayerHost somewhere after your Router root component.");
            return rootHost;
        }
    }
}
