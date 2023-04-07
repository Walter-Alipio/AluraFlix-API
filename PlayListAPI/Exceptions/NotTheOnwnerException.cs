namespace PlayListAPI.Exceptions;

public class NotTheOwnerException : Exception
{
  public NotTheOwnerException()
  {

  }
  public NotTheOwnerException(string message) : base(message)
  {

  }

}