using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskApp1
{
    class PC<T> : IDisposable
    {
        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public int Length { get => _queue.Count; }

        public Task Produce(T element, Action<T> onCompleted)
        {
            Task t = Task.Factory.StartNew(
                () =>
                {
                    Add(element, onCompleted);
                }
                );

            return t;
        }

        public Task Consume(Action<T> onCompleted)
        {
            Task t = Task.Factory.StartNew(
                () =>
                {
                    Remove(onCompleted);
                }
                );

            return t;
        }

        private void Add(T element, Action<T> onCompleted)
        {
            _queue.Enqueue(element);
            if (onCompleted != null) onCompleted(element);
        }

        private void Remove(Action<T> onCompleted)
        {
            _queue.TryDequeue(out T el);

            if (el == null)
                Trace.WriteLine($"Consume is waiting...");
            else
                if (onCompleted != null) onCompleted(el);
        }

        public void Dispose()
        {
            _queue.Clear();
        }
    }
}
