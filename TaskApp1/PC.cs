using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskApp1
{
    class PC<T> : IDisposable
    {
        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public int Length => _queue?.Count ?? 0;

        public Task Produce(T element, Action<T> onCompleted)
        {
            return Task.Factory.StartNew(
                () => Add(element, onCompleted)
                );
        }

        public Task Consume(Action<T> onCompleted)
        {
            return Task.Factory.StartNew(
                () => Remove(onCompleted)
                );
        }

        private void Add(T element, Action<T> onCompleted)
        {
            _queue.Enqueue(element);
            onCompleted?.Invoke(element);
        }

        private void Remove(Action<T> onCompleted)
        {
            _queue.TryDequeue(out T el);

            var f = (el == null ? 
                delegate { Trace.WriteLine($"Consume is idle..."); } : onCompleted);
            f?.Invoke(el);
        }

        public void Dispose() => _queue.Clear();
    }
}
