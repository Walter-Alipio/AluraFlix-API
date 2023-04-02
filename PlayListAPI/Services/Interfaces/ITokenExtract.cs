using Microsoft.Extensions.Primitives;
namespace PlayListAPI.Services.Interfaces;

public interface ITokenExtract
{
  string ExtractID(StringValues stringValues);
}