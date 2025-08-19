using System.Numerics;

namespace Photography_Tools.Helpers;

public static class NumberTypeHelper<T> where T : INumber<T>
{
    public static readonly bool IsInteger = IntegerChecker<T>.Value;

    private static class IntegerChecker<TT> where TT : INumber<TT>
    {
        public static readonly bool Value =
            typeof(TT) == typeof(sbyte) ||
            typeof(TT) == typeof(byte) ||
            typeof(TT) == typeof(short) ||
            typeof(TT) == typeof(ushort) ||
            typeof(TT) == typeof(int) ||
            typeof(TT) == typeof(uint) ||
            typeof(TT) == typeof(long) ||
            typeof(TT) == typeof(ulong) ||
            typeof(TT) == typeof(Int128) ||
            typeof(TT) == typeof(UInt128) ||
            typeof(TT) == typeof(nint) ||
            typeof(TT) == typeof(nuint) ||
            typeof(TT) == typeof(BigInteger);
    }
}