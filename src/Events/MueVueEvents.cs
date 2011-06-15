using System;
using EventStore;
using EventStore.Dispatcher;
// using Infrastructure.Config;

namespace Events
{
    public static class MueVueEvents
    {
    	private static readonly byte[] EncryptionKey = new byte[]
    	                                               	{
    	                                               		0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa,
    	                                               		0xb, 0xc, 0xd, 0xe, 0xf
    	                                               	};

        private static readonly IStoreEvents Store = Wireup.Init()
			.UsingInMemoryPersistence()
			//.UsingSqlPersistence(MueVueConfig.MueVueEventsName)
			//.InitializeStorageEngine()
			.UsingJsonSerialization()
			.Compress()
			.EncryptWith(EncryptionKey)
			.HookIntoPipelineUsing(new[] {new AuthorizationPipelineHook()})
            .UsingAsynchronousDispatcher()
            .PublishTo(new DelegateMessagePublisher(DispatchCommit))
            .Build();

        private static void DispatchCommit(Commit commit)
        {
            // this is where we'd hook into our messaging infrastructure, e.g. NServiceBus.
            // this can be a class as well--just implement IPublishMessages
            try
            {
                Console.WriteLine((string) Resources.MessagesPublished);
            }
            catch (Exception)
            {
                Console.WriteLine((string) Resources.UnableToPublish);
            }
        }

        public static void Add(EventBase e)
        {
            using (Store)
            {
                using (var stream = Store.OpenStream(e.StreamId, 0, int.MaxValue))
                {
                    e.StoredAt = DateTime.UtcNow;
                    stream.Add(new EventMessage {Body = e});
                    stream.CommitChanges(Guid.NewGuid());
                }
            }
        }
    }

//private static void Main()
        //{
        //    using (Store)
        //    {
        //        OpenOrCreateStream();
        //        AppendToStream();
        //        TakeSnapshot();
        //        LoadFromSnapshotForwardAndAppend();

        //        Console.WriteLine(Resources.PressAnyKey);
        //        Console.ReadLine();
        //    }
        //}
        



        //private static void OpenOrCreateStream()
        //{
        //    // we can call CreateStream(StreamId) if we know there isn't going to be any data.
        //    // or we can call OpenStream(StreamId, 0, int.MaxValue) to read all commits,
        //    // if no commits exist then it creates a new stream for us.
        //    using (var stream = Store.OpenStream(StreamId, 0, int.MaxValue))
        //    {
        //        var @event = new SomeDomainEvent { Value = "Initial event." };

        //        stream.Add(new EventMessage { Body = @event });
        //        stream.CommitChanges(Guid.NewGuid());
        //    }

        //private static void AddEvent(MueVueEvent mvEvent)
        //{
        //    using (var stream = Store.OpenStream)
        //}

        //private static void AppendToStream()
        //{
        //    using (var stream = Store.OpenStream(StreamId, int.MinValue, int.MaxValue))
        //    {
        //        var @event = new SomeDomainEvent { Value = "Second event." };

        //        stream.Add(new EventMessage { Body = @event });
        //        stream.CommitChanges(Guid.NewGuid());
        //    }
        //}
        //private static void TakeSnapshot()
        //{
        //    var memento = new AggregateMemento { Value = "snapshot" };
        //    Store.AddSnapshot(new Snapshot(StreamId, 2, memento));
        //}
        //private static void LoadFromSnapshotForwardAndAppend()
        //{
        //    var latestSnapshot = Store.GetSnapshot(StreamId, int.MaxValue);

        //    using (var stream = Store.OpenStream(latestSnapshot, int.MaxValue))
        //    {
        //        var @event = new SomeDomainEvent { Value = "Third event (first one after a snapshot)." };

        //        stream.Add(new EventMessage { Body = @event });
        //        stream.CommitChanges(Guid.NewGuid());
        //    }
        //}
    //}
}