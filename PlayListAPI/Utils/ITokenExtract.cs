using Microsoft.Extensions.Primitives;
namespace PlayListAPI.Utils;

public interface ITokenExtract
{
  string ExtractID(StringValues stringValues);
}