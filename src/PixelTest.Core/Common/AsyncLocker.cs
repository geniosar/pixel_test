namespace PixelTest.Core.Common;

public sealed class AsyncLocker : IDisposable
{
	private readonly SemaphoreSlim _semaphore = new(1, 1);
	private readonly Task<IDisposable> _releaser;

	public AsyncLocker()
	{
		_releaser = Task.FromResult((IDisposable)new Releaser(this));
	}

	public void Dispose()
	{
		_releaser.Dispose();
	}

	public Task<IDisposable> LockAsync()
	{
		var waiter = _semaphore.WaitAsync();
		return waiter.IsCompleted ?
					_releaser :
					waiter.ContinueWith((_, state) =>
						(IDisposable)state,
						_releaser.Result,
						CancellationToken.None,
						TaskContinuationOptions.ExecuteSynchronously,
						TaskScheduler.Default);
	}

	private sealed class Releaser : IDisposable
	{
		private readonly AsyncLocker _toRelease;
		internal Releaser(AsyncLocker toRelease)
		{
			_toRelease = toRelease;
		}
		public void Dispose()
		{
			_toRelease._semaphore.Release();
		}
	}
}

