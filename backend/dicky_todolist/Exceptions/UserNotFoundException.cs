namespace dicky_todolist.Exceptions;

public class UserNotFoundException : Exception
{
  public UserNotFoundException(string message) : base(message) { }
}
