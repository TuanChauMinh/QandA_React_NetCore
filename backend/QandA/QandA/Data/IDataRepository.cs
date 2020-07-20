using QandA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.Data
{
    public interface IDataRepository
    {
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearchWithPaging(string search, int pageNumber, int pageSize);
        Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestionsAsync();

        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions();
        Task<QuestionGetSingleResponse> GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        QuestionGetSingleResponse PostQuestion(QuestionPostFullRequest question);
        Task<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest question);
        void DeleteQuestion(int questionId);
        Task<AnswerGetResponse> PostAnswer(AnswerPostFullRequest answer);
    }
}
