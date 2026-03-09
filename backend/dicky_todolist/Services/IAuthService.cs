using dicky_todolist.Models;

namespace dicky_todolist.Services;

public interface IAuthService
{
  string GenerateToken(User user);
}