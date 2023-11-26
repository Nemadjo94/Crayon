using Crayon.Domain.Entities;

namespace Crayon.Application.Interfaces
{
    public interface IJwtGenerator
    {
        public string CreateToken(User user);
    }
}
