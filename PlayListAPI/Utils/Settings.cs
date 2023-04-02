using dotenv.net;
namespace PlayListAPI.Utils;
public static class Settings
{

  public static string Secret()
  {
    DotEnv.Load();
    var builder = new ConfigurationBuilder();
    builder.AddEnvironmentVariables();
    var configuration = builder.Build();

    var secret = configuration.GetValue<string>("SECRET").Trim();
    return secret;
  }
}