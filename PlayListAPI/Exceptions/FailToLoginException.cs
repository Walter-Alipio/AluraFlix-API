public class FailToLoginException : Exception
{
  public FailToLoginException() { }

  public FailToLoginException(string message) : base(message)
  { }
}