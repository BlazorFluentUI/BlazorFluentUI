using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace BlazorFabric
{
    public static class ObservableExtensions
    {
        public static IObservable<T> SampleFirst<T>(
            this IObservable<T> source,
            TimeSpan sampleDuration,
            IScheduler scheduler = null)
        {
            scheduler = scheduler ?? Scheduler.Default;
            return source.Publish(ps =>
                ps.Window(() => ps.Delay(sampleDuration, scheduler))
                  .SelectMany(x => x.Take(1)));
        }
    }
}
