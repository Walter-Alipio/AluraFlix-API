namespace PlayListAPI.Exceptions;

public class ErrorToGetUserIdException : Exception
{
  public ErrorToGetUserIdException()
  {

  }
  public ErrorToGetUserIdException(string message) : base(message)
  {

  }
}