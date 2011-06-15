// Type: EventStore.Wireup
// Assembly: EventStore, Version=2.0.0.0, Culture=neutral, PublicKeyToken=7735eb81c0bd9948
// Assembly location: C:\Users\Daniel\Documents\Visual Studio 2010\Projects\MueVue1\Events\bin\Debug\EventStore.dll

using EventStore.Dispatcher;
using EventStore.Persistence;
using EventStore.Persistence.InMemoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EventStore
	{
	[ComVisible(true)]
	public class Wireup
		{
		private readonly Wireup inner;
		private readonly NanoContainer container;

		protected NanoContainer Container
			{
			get
				{
				return this.container ?? this.inner.Container;
				}
			}

		protected Wireup(NanoContainer container)
			{
			this.container = container;
			}

		protected Wireup(Wireup inner)
			{
			this.inner = inner;
			}

		[ComVisible(true)]
		public static Wireup Init()
			{
			NanoContainer container = new NanoContainer();
			container.Register<IPersistStreams>((IPersistStreams)new InMemoryPersistenceEngine());
			container.Register<IStoreEvents>(new Func<NanoContainer, IStoreEvents>(Wireup.BuildEventStore));
			return new Wireup(container);
			}

		[ComVisible(true)]
		public virtual Wireup With<T>(T instance) where T : class
			{
			this.Container.Register<T>(instance);
			return this;
			}

		[ComVisible(true)]
		public virtual Wireup HookIntoPipelineUsing(IEnumerable<IPipelineHook> hooks)
			{
			return this.HookIntoPipelineUsing(Enumerable.ToArray<IPipelineHook>(hooks ?? (IEnumerable<IPipelineHook>)new IPipelineHook[0]));
			}

		[ComVisible(true)]
		public virtual Wireup HookIntoPipelineUsing(params IPipelineHook[] hooks)
			{
			this.Container.Register<ICollection<IPipelineHook>>((ICollection<IPipelineHook>)Enumerable.ToArray<IPipelineHook>(Enumerable.Where<IPipelineHook>((IEnumerable<IPipelineHook>)(hooks ?? new IPipelineHook[0]), (Func<IPipelineHook, bool>)(x => x != null))));
			return this;
			}

		[ComVisible(true)]
		public virtual IStoreEvents Build()
			{
			if (this.inner != null)
				return this.inner.Build();
			else
				return this.Container.Resolve<IStoreEvents>();
			}

		private static IStoreEvents BuildEventStore(NanoContainer context)
			{
			ICollection<IPipelineHook> collection = (ICollection<IPipelineHook>)Enumerable.ToArray<IPipelineHook>(Enumerable.Concat<IPipelineHook>((IEnumerable<IPipelineHook>)new IPipelineHook[2]
      {
        (IPipelineHook) new OptimisticPipelineHook(),
        (IPipelineHook) new DispatchPipelineHook(context.Resolve<IDispatchCommits>())
      }, (IEnumerable<IPipelineHook>)(context.Resolve<ICollection<IPipelineHook>>() ?? (ICollection<IPipelineHook>)new IPipelineHook[0])));
			return (IStoreEvents)new OptimisticEventStore(context.Resolve<IPersistStreams>(), (IEnumerable<IPipelineHook>)collection);
			}
		}
	}
