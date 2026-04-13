using Moq;
using Microsoft.Extensions.Options;
using coleta_residuos.Settings;

namespace coleta_residuos.Tests.Fixtures
{
    /// <summary>
    /// Factory auxiliar para criar mocks comuns nos testes
    /// </summary>
    public static class MockFactory
    {
        public static Mock<IOptions<JwtSettings>> CreateJwtSettingsMock(string secret = null)
        {
            var mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            
            var secretValue = secret ?? "f+ujXAKHk00L5jlMXo2XhAWawsOoihNP1OiAM25lLSO57+X7uBMQgwPju6yzyePi";
            
            mockJwtSettings.Setup(x => x.Value).Returns(new JwtSettings
            {
                Secret = secretValue
            });

            return mockJwtSettings;
        }

        public static Mock<IOptions<JwtSettings>> CreateJwtSettingsMock(JwtSettings settings)
        {
            var mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            mockJwtSettings.Setup(x => x.Value).Returns(settings);
            return mockJwtSettings;
        }
    }
}
