using Microsoft.Extensions.Internal;

namespace Jouska.AutoFixture.Extensions.HybridCache.Helpers;

public class TestClock: ISystemClock
{
    private readonly TimeProvider _timeProvider;

    public TestClock(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();
}
