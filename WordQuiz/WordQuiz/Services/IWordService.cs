using System.Collections.Generic;
using System.Threading.Tasks;
using WordQuiz.Models;

namespace WordQuiz.Services
{
    public interface IWordService
    {
        Task<IEnumerable<Word>> GetWords();
    }
}
