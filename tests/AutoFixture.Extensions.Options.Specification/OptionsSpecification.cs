using AutoFixture.Extensions.Options.Helpers;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.Extensions.Options.Specification;

public class OptionsSpecification
{
    [Fact]
    public void Can_register_option_value_with_options_monitor()
    {
        var options = new TestOptions
        {
            ReferenceNumber = 123,
            Name = "Test"
        };

        var sut = new Fixture()
            .WithOptionsMonitor(options);

        var service = sut.Freeze<TestService>();

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.Equal(options.ReferenceNumber, service.GetReferenceNumber),
            () => Assert.Equal(options.Name, service.GetName)
        );
    }

    [Fact]
    public void Can_update_option_value_in_options_monitor_and_propagate_change()
    {
        var options = new TestOptions
        {
            ReferenceNumber = 123,
            Name = "Test"
        };

        var sut = new Fixture().WithOptionsMonitor(options);

        var service = sut.Freeze<TestService>();

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.Equal(options.ReferenceNumber, service.GetReferenceNumber),
            () => Assert.Equal(options.Name, service.GetName)
        );

        var updatedOptions = new TestOptions
        {
            ReferenceNumber = 456,
            Name = "Updated"
        };

        var optionsMonitor = sut.Freeze<TestOptionsMonitor<TestOptions>>();
        optionsMonitor.UpdateOptions(updatedOptions);

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.Equal(updatedOptions.ReferenceNumber, service.GetReferenceNumber),
            () => Assert.Equal(updatedOptions.Name, service.GetName)
        );
    }
}

public class TestService
{
    private readonly IOptionsMonitor<TestOptions> _options;
    public TestService(IOptionsMonitor<TestOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public int GetReferenceNumber => 
        _options.CurrentValue.ReferenceNumber;

    public string GetName => 
        _options.CurrentValue.Name ?? string.Empty;
}

public class TestOptions
{
    public int ReferenceNumber { get; set; }

    [NotNull]
    public string? Name { get; set; }
}
