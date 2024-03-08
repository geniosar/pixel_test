namespace PixelTest.Core.Exceptions;

public class NullValueException : Exception
{
	public NullValueException(string key)
		: base("Value " + key + " couldn't be null")
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
	}

	public NullValueException(string key, string because)
		: base("Value " + key + " couldn't be null. " + because)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}

		if (because == null)
		{
			throw new ArgumentNullException("because");
		}
	}

	public static void ThrowIfNull(object? obj, string key)
	{
		if (obj == null)
		{
			throw new NullValueException(key);
		}
	}

	public static void ThrowIfNull(object? obj, string key, string because)
	{
		if (obj == null)
		{
			throw new NullValueException(key, because);
		}
	}
}
