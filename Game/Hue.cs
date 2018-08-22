﻿using System;
using System.Globalization;

namespace ClassicUO.Game
{
    public struct Hue : IComparable, IComparable<ushort>
    {
        public const ushort Invariant = ushort.MaxValue;
        public static Hue SystemCol = new Hue(0x3B2);
        public static Hue Good = new Hue(68);
        public static Hue Error = new Hue(37);
        public static Hue Warning = new Hue(1174);

        private readonly ushort _value;

        public Hue(in ushort hue)
        {
            _value = hue;
        }

        public bool IsInvariant => _value == Invariant;

        public static implicit operator Hue(in ushort value)
        {
            return new Hue(value);
        }

        public static implicit operator ushort(in Hue color)
        {
            return color._value;
        }

        public static bool operator ==(in Hue h1, in Hue h2)
        {
            return h1.IsInvariant || h2.IsInvariant || h1._value == h2._value;
        }

        public static bool operator !=(in Hue h1, in Hue h2)
        {
            return !h1.IsInvariant && !h2.IsInvariant && h1._value != h2._value;
        }

        public static bool operator <(in Hue h1, in Hue h2)
        {
            return h1._value < h2._value;
        }

        public static bool operator >(in Hue h1, in Hue h2)
        {
            return h1._value > h2._value;
        }

        public int CompareTo(object obj)
        {
            return _value.CompareTo(obj);
        }

        public int CompareTo(ushort other)
        {
            return _value.CompareTo(other);
        }

        public override string ToString()
        {
            return $"0x{_value:X4}";
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Hue)
            {
                return this == (Hue)obj;
            }

            if (obj is ushort)
            {
                return _value == (ushort)obj;
            }

            return false;
        }

        public static Hue Parse(in string str)
        {
            if (str.StartsWith("0x"))
            {
                return ushort.Parse(str.Remove(0, 2), NumberStyles.HexNumber);
            }

            return ushort.Parse(str);
        }
    }
}