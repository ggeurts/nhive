namespace NHive.Core.Events
{
    using System;
    using System.Collections.Generic;

    internal struct HiveEventPublisher<T, TSize, TIterator>
        where TIterator: struct, IInputIterator<T, TSize, TIterator>
        where TSize: struct, IConvertible
    {
        private HiveEvents _supportedEvents;
        private HiveEvents _activeEvents;
        private IList<IHiveEventSubscriber<T, TSize, TIterator>> _addEventSubscribers;
        private IList<IHiveEventSubscriber<T, TSize, TIterator>> _removingEventSubscribers;

        public HiveEventPublisher(HiveEvents supportedEvents)
        {
            _supportedEvents = supportedEvents;
            _activeEvents = HiveEvents.None;
            _addEventSubscribers = null;
            _removingEventSubscribers = null;
        }

        public bool HasSubscriptionForAll(HiveEvents hiveEvents)
        {
            return (_activeEvents & hiveEvents) == hiveEvents;
        }

        public bool HasSubscriptionForAny(HiveEvents hiveEvents)
        {
            return (_activeEvents & hiveEvents) != HiveEvents.None;
        }

        public bool HasAddEventSubscribers
        {
            get { return _addEventSubscribers != null && _addEventSubscribers.Count > 0; }
        }

        public bool HasRemovingEventSubscribers
        {
            get { return _removingEventSubscribers != null && _removingEventSubscribers.Count > 0; }
        }

        public void Subscribe(IHiveEventSubscriber<T, TSize, TIterator> subscriber, HiveEvents toEvents)
        {
            if ((toEvents & HiveEvents.Added) == HiveEvents.Added)
            {
                Subscribe(subscriber, HiveEvents.Added, ref _addEventSubscribers);
            }
            if ((toEvents & HiveEvents.Removing) == HiveEvents.Removing)
            {
                Subscribe(subscriber, HiveEvents.Removing, ref _removingEventSubscribers);
            }
        }

        private void Subscribe(IHiveEventSubscriber<T, TSize, TIterator> subscriber, HiveEvents toEvent,
            ref IList<IHiveEventSubscriber<T, TSize, TIterator>> subscriberList)
        {
            if ((_supportedEvents & toEvent) == 0)
            {
                throw new NotSupportedException(
                    string.Format("The '{0}' event is not supported.", toEvent));
            }
            
            if (subscriberList == null)
            {
                subscriberList = new List<IHiveEventSubscriber<T, TSize, TIterator>>();
            }
            subscriberList.Add(subscriber);
        }

        public void Unsubscribe(IHiveEventSubscriber<T, TSize, TIterator> subscriber)
        {
            Unsubscribe(subscriber, _supportedEvents);
        }

        public void Unsubscribe(IHiveEventSubscriber<T, TSize, TIterator> subscriber, HiveEvents fromEvents)
        {
            if ((fromEvents & HiveEvents.Added) == HiveEvents.Added && _addEventSubscribers != null)
            {
                _addEventSubscribers.Remove(subscriber);
            }
            if ((fromEvents & HiveEvents.Removing) == HiveEvents.Removing && _removingEventSubscribers != null)
            {
                _removingEventSubscribers.Remove(subscriber);
            }
        }

        public void PublishAddedEvent(TIterator at)
        {
            if (_addEventSubscribers == null) return;
            foreach (IHiveEventSubscriber<T, TSize, TIterator> subscriber in _addEventSubscribers)
            {
                subscriber.Added(at);
            }
        }

        public void PublishAddedEvent(Range<T, TSize, TIterator> addedRange)
        {
            if (_addEventSubscribers == null) return;
            foreach (IHiveEventSubscriber<T, TSize, TIterator> subscriber in _addEventSubscribers)
            {
                subscriber.Added(addedRange);
            }
        }

        public void PublishRemovingEvent(TIterator at)
        {
            if (_removingEventSubscribers == null) return;
            foreach (IHiveEventSubscriber<T, TSize, TIterator> subscriber in _removingEventSubscribers)
            {
                subscriber.Removing(at);
            }
        }

        public void PublishRemovingEvent(Range<T, TSize, TIterator> removingRange)
        {
            if (_removingEventSubscribers == null) return;
            foreach (IHiveEventSubscriber<T, TSize, TIterator> subscriber in _removingEventSubscribers)
            {
                subscriber.Removing(removingRange);
            }
        }
    }
}