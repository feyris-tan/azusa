using System;

namespace moe.yo3explorer.azusa.DexcomHistory.Control
{
    class ListBoxDateWrapper
    {
        public ListBoxDateWrapper(DateTime dt)
        {
            Value = dt;
        }

        public DateTime Value { get; private set; }

        public override string ToString()
        {
            return Value.ToLongDateString();
        }
    }
}
