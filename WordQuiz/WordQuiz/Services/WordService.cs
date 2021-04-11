using System.Collections.Generic;
using System.Threading.Tasks;
using WordQuiz.Models;

namespace WordQuiz.Services
{
    public class WordService : IWordService
    {
        public Task<IEnumerable<Word>> GetWords() => Task.FromResult((IEnumerable<Word>)new List<Word>() { new Word { Name = "Maca"},
                                                                                        new Word { Name = "Pera" } ,
                                                                                        new Word { Name = "Uva" } ,
                                                                                        new Word { Name = "Goiaba" },
                                                                                        new Word { Name = "Melancia" } });
    }
}
