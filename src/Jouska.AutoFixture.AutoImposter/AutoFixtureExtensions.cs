using AutoFixture;
using Imposter.Abstractions;
using System.Reflection;

namespace Jouska.AutoFixture.AutoImposter;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        // TODO: add xml comments

        public IFixture WithAutoImposter(params Assembly[] assemblies)
        {
            fixture.Customize(new AutoImposterCustomization(assemblies));
            return fixture;
        }

        public TTargetInterface FreezeImposter<TTargetInterface, TImposter>()
            where TImposter : IHaveImposterInstance<TTargetInterface>
        {
            var target = fixture.Freeze<TTargetInterface>(x => x.FromFactory<TImposter>(imposter => imposter.Instance()));
            return target;
        }

        public TTargetInterface FreezeImposter<TTargetInterface, TImposter>(TImposter imposter)
            where TImposter : IHaveImposterInstance<TTargetInterface>
        {
            fixture.Inject(imposter);
            var target = fixture.Freeze<TTargetInterface>(x => x.FromFactory<TImposter>(imposter => imposter.Instance()));
            return target;
        }
    }
}