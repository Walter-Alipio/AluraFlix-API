namespace PlayListAPI.Exceptions;

public class NotTheVideoOwnerException : Exception
{
  public NotTheVideoOwnerException()
  {

  }
  public NotTheVideoOwnerException(string message) : base(message)
  {

  }

}