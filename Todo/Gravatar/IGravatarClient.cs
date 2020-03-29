using System.Threading.Tasks;

namespace Todo.Gravatar
{
    /// <summary>
    /// Interface for getting different data from Gravatar.
    /// </summary>
    public interface IGravatarClient
    {
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="email">Email identifier.</param>
        Task<string> GetName(string email);
    }
}
