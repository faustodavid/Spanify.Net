using System;
using System.Runtime.CompilerServices;

namespace Spanify.Net
{
    public ref struct CommaSeparatedSpanCollection
    {
        private readonly ReadOnlySpan<char> _source;
        private readonly char _separator;
        private int _count;

        public CommaSeparatedSpanCollection(ReadOnlySpan<char> source, char separator)
        {
            _source = source;
            _separator = separator;
            _count = -1;
        }

        public int Count()
        {
            if (_count > -1)
            {
                return _count;
            }

            var enumerator = new Enumerator(_source, _separator);
            var count = 0;

            while (enumerator.MoveNext())
            {
                count++;
            }

            _count = count;
            return count;
        }

        public readonly ReadOnlySpan<char> this[int index]
        {
            get
            {
                if (index < 0 || (_count != -1 && index >= _count))
                    throw new IndexOutOfRangeException(nameof(index));

                var enumerator = new Enumerator(_source, _separator);
                var currentIndex = -1;
                while (currentIndex != index)
                {
                    if (!enumerator.MoveNext())
                        throw new IndexOutOfRangeException(nameof(index));

                    currentIndex++;
                }

                return enumerator.Current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(_source, _separator);

        public ref struct Enumerator
        {
            private ReadOnlySpan<char> _source;
            private ReadOnlySpan<char> _current;
            private readonly char _separator;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(ReadOnlySpan<char> source, char separator)
            {
                _source = source;
                _separator = separator;
                _current = ReadOnlySpan<char>.Empty;
            }

            public readonly ReadOnlySpan<char> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _current;
            }

            public bool MoveNext()
            {
                var source = _source;
                var index = source.IndexOf(_separator);
                if (index == -1)
                {
                    if (source.Length == 0) return false;
                    _current = source;
                    _source = ReadOnlySpan<char>.Empty;
                    return true;
                }

                _current = source.Slice(0, index);
                _source = source.Slice(index + 1);

                return true;
            }
        }
    }
}