using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace QueenGame.WPF.Utils.JSON
{
    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.EnsureDefaults(builder);
            return new WritableJsonConfigurationProvider(this);
        }
    }
}
