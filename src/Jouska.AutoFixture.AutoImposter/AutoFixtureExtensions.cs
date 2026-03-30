using AutoFixture;
using Imposter.Abstractions;
using System.Reflection;

namespace Jouska.AutoFixture.AutoImposter;

public static class AutoFixtureExtensions
{
    public static IFixture WithAutoImposter(this IFixture fixture, params Assembly[] assemblies)
    {
        fixture.Customize(new AutoImposterCustomization(assemblies));
        return fixture;
    }

    public static TTargetInterface FreezeImposter<TTargetInterface, TImposter>(this IFixture fixture)
        where TImposter : IHaveImposterInstance<TTargetInterface>
    {
        var target = fixture.Freeze<TTargetInterface>(x => x.FromFactory<TImposter>(imposter => imposter.Instance()));        
        return target;
    }

    public static TTargetInterface FreezeImposter<TTargetInterface, TImposter>(this IFixture fixture, TImposter imposter)
        where TImposter : IHaveImposterInstance<TTargetInterface>
    {
        fixture.Inject(imposter);
        var target = fixture.Freeze<TTargetInterface>(x => x.FromFactory<TImposter>(imposter => imposter.Instance()));
        return target;
    }
}