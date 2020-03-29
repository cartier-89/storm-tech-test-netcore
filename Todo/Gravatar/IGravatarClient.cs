using System.Threading.Tasks;

namespace Todo.Gravatar
{
    /// <summary>
    /// Interface for getting different data from Gravatar.
    /// </summary>
    public interface IGravatarClient
    {
        /// <summary>
        /// Asynchronously gets the Gravatar user name of the given e-mail/
        /// </summary>
        /// <param name="email">Email identifier.</param>
        Task<string> GetNameAsync(string email);
    }
}
