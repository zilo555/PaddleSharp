namespace Sdcb.PaddleInference.Tests;

public class PaddleConfigAdvancedApiTests
{
    [Fact]
    public void GLogMinLevelCanRoundTrip()
    {
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        int originalLevel = PaddleConfig.GLogMinLevel;
        int targetLevel = originalLevel == 3 ? 2 : 3;

        try
        {
            PaddleConfig.GLogMinLevel = targetLevel;
            Assert.Equal(targetLevel, PaddleConfig.GLogMinLevel);
        }
        finally
        {
            PaddleConfig.GLogMinLevel = originalLevel;
        }
    }

    [Fact]
    public void GLogHandlerCanRoundTripAndClear()
    {
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        PaddleGLogCallback handler = StaticGlogCallback;
        PaddleConfig.GLogHandler = handler;
        Assert.Same(handler, PaddleConfig.GLogHandler);

        PaddleConfig.GLogHandler = null;
        Assert.Null(PaddleConfig.GLogHandler);
    }

    [Fact]
    public void InvalidGLogMinLevelThrowsManagedException()
    {
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => PaddleConfig.GLogMinLevel = 4);
        Assert.False(string.IsNullOrWhiteSpace(exception.Message));
    }

    private static void StaticGlogCallback(int severity, string file, int line, string message)
    {
        Assert.NotNull(file);
        Assert.NotNull(message);
    }
}