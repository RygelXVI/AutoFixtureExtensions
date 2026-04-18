using AutoFixture;
using Jouska.AutoFixture.Extensions.Options.Helpers;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Jouska.AutoFixture.Extensions.Options.Specification;

public class OptionsSpecification
{
    [Fact]
    public void Can_register_option_value_with_options_monitor()
    {
        var name = "Test";
        var referenceNumber = 1523;

        var sut = new Fixture()
            .WithOptionsMonitor<TestOptions>(options =>
            {
                options.Name = name;
                options.ReferenceNumber = referenceNumber;
            });

        var service = sut.Freeze<OptionsMonitorTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.Equal(referenceNumber, service.GetReferenceNumber),
            () => Assert.Equal(name, service.GetName)
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

        var service = sut.Freeze<OptionsMonitorTestSubject>();

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

    [Fact]
    public void Can_register_option_snapshot()
    {
        var name = "Test";
        var referenceNumber = 1523;
        var fixture = new Fixture()
            .WithOptionsSnapshot<TestOptions>(option =>
            {
                option.Name = name;
                option.ReferenceNumber = referenceNumber;
            });
        
        var service = fixture.Freeze<OptionsSnapshotTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.NotNull(service.Options),
            () => Assert.Equal(referenceNumber, service.Options.Value.ReferenceNumber),
            () => Assert.Equal(name, service.Options.Value.Name)
        );
    }


    [Fact]
    public void Can_register_option()
    {
        var name = "Test";
        var referenceNumber = 1523;

        var fixture = new Fixture()
            .WithOptions<TestOptions>(option => 
            {
                option.Name = name;
                option.ReferenceNumber = referenceNumber;
            });

        var service = fixture.Freeze<OptionsTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(service),
            () => Assert.NotNull(service.Options),
            () => Assert.Equal(referenceNumber, service.Options.Value.ReferenceNumber),
            () => Assert.Equal(name, service.Options.Value.Name)
        );
    }
}

public class OptionsMonitorTestSubject
{
    private readonly IOptionsMonitor<TestOptions> _options;
    public OptionsMonitorTestSubject(IOptionsMonitor<TestOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public int GetReferenceNumber => 
        _options.CurrentValue.ReferenceNumber;

    public string GetName => 
        _options.CurrentValue.Name ?? string.Empty;
}

public class OptionsTestSubject
{
    public OptionsTestSubject(IOptions<TestOptions> options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IOptions<TestOptions> Options { get; }
}

public class OptionsSnapshotTestSubject
{
    public OptionsSnapshotTestSubject(IOptionsSnapshot<TestOptions> options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IOptionsSnapshot<TestOptions> Options { get; }
}

public class TestOptions
{
    public int ReferenceNumber { get; set; }

    [NotNull]
    public string? Name { get; set; }
}
