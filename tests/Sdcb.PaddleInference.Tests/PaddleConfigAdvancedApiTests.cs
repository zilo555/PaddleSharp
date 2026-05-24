namespace Sdcb.PaddleInference.Tests;

public class PaddleConfigAdvancedApiTests
{
    [Fact]
    public void GLogMinLevelCanRoundTrip()
    {
        using PaddleConfig config = new();
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        int originalLevel = config.GLogMinLevel;
        int targetLevel = originalLevel == 3 ? 2 : 3;

        try
        {
            config.GLogMinLevel = targetLevel;
            Assert.Equal(targetLevel, config.GLogMinLevel);
        }
        finally
        {
            config.GLogMinLevel = originalLevel;
        }
    }

    [Fact]
    public void GLogRedirectCallbackCanRegisterAndClear()
    {
        using PaddleConfig config = new();
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        config.SetGLogRedirectCallback(StaticGlogCallback);
        config.ClearGLogRedirectCallback();
    }

    [Fact]
    public void InvalidGLogMinLevelThrowsManagedException()
    {
        using PaddleConfig config = new();
        if (!PaddleAdvancedNativeApi.IsV2CapiAvailable)
        {
            return;
        }

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => config.GLogMinLevel = 4);
        Assert.False(string.IsNullOrWhiteSpace(exception.Message));
    }

    private static void StaticGlogCallback(PaddleGLogSeverity severity, string file, int line, string message)
    {
        Assert.True(Enum.IsDefined(typeof(PaddleGLogSeverity), severity));
        Assert.NotNull(file);
        Assert.NotNull(message);
    }
}