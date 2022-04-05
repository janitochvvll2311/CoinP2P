namespace CoinP2P.Helpers;

public static class BinaryExtensions
{

    public static byte[] GetHardBytes<T>(this T data)
        where T : unmanaged
    {
        unsafe
        {
            var cache = new byte[sizeof(T)];
            fixed (byte* pin = &cache[0])
            {
                var pointer = (T*)pin;
                *pointer = data;
            }
            return cache;
        }
    }

    public static U HardCast<T, U>(this T data)
        where T : unmanaged
        where U : unmanaged
    {
        unsafe
        {
            var pointer = &data;
            return *(U*)pointer;
        }
    }

}