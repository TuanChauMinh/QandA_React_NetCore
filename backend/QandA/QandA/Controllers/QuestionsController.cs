using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QandA.Data;
using QandA.Data.Models;
using QandA.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QandA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IHubContext<QuestionsHub> _questionHubContext;
        private readonly IQuestionCache _cache;
        public QuestionsController(IDataRepository dataRepository, IHubContext<QuestionsHub> questionHubContext, IQuestionCache cache)
        {
            _dataRepository = dataRepository;
            _questionHubContext = questionHubContext;
            _cache = cache;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions(string search, bool includeAnswers, int page = 1, int pageSize =20)
        {
            if(string.IsNullOrEmpty(search))
            {
                if (includeAnswers)
                {
                    return _dataRepository.GetQuestionsWithAnswers();
                }
                else
                {
                    return _dataRepository.GetQuestions();
                }
            }
            else
            {
                return _dataRepository.GetQuestionsBySearchWithPaging(search,page,pageSize);
            }
        }
        [AllowAnonymous]
        [HttpGet("unanswered")]
        public async  Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestions()
        {
            return await _dataRepository.GetUnansweredQuestionsAsync();
        }

        [AllowAnonymous]
        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            //var question = _dataRepository.GetQuestion(questionId);
            var question = _cache.Get(questionId);
          
            if(question == null)
            {
                question = _dataRepository.GetQuestion(questionId);
                if (question == null)
                {
                    return NotFound();
                }
            }
            _cache.Set(question);
            return question;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionGetSingleResponse>> PostQuestion(QuestionPostRequest questionPostRequest)
        {
            //var json = await new StreamReader(Request.Body).ReadToEndAsync();

            //var questionPostRequest = JsonConvert.DeserializeObject<QuestionPostRequest>(json);

            var savedQuestion =
             _dataRepository.PostQuestion(new QuestionPostFullRequest
             {
                 Title = questionPostRequest.Title,
                 Content = questionPostRequest.Content,
                 UserId = "1",
                 UserName = "bob.test@test.com",
                 Created = DateTime.UtcNow
             });

            return CreatedAtAction(nameof(GetQuestion),
             new { questionId = savedQuestion.QuestionId },
             savedQuestion);
        }

        [Authorize]
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest questionPutRequest)
        {
            var question = _dataRepository.GetQuestion(questionId);
            if (question == null)
            {
                return NotFound();
            }

            questionPutRequest.Title = string.IsNullOrEmpty(questionPutRequest.Title) ? question.Title : questionPutRequest.Title;
            questionPutRequest.Content = string.IsNullOrEmpty(questionPutRequest.Content) ? question.Content : questionPutRequest.Content;

            var savedQuestion = _dataRepository.PutQuestion(questionId, questionPutRequest);

            _cache.Remove(savedQuestion.QuestionId);
            return savedQuestion;
        }

        [Authorize]
        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
            var question = _dataRepository.GetQuestion(questionId);
            if (question == null)
            {
                return NotFound();
            }
            _dataRepository.DeleteQuestion(questionId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("answer")]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostFullRequest answerPostRequest)
        {
            var questionExists =
            _dataRepository.QuestionExists(answerPostRequest.QuestionId.Value);
            if (!questionExists)
            {
                return NotFound();
            }

            var savedAnswer =
             _dataRepository.PostAnswer(new AnswerPostFullRequest
                 {
                     QuestionId = answerPostRequest.QuestionId.Value,
                     Content = answerPostRequest.Content,
                     UserId = "1",
                     UserName = "bob.test@test.com",
                     Created = DateTime.UtcNow
                 }
             );
            _cache.Remove(answerPostRequest.QuestionId.Value);

            _questionHubContext.Clients.Group(
             $"Question-{answerPostRequest.QuestionId.Value}")
             .SendAsync(
             "ReceiveQuestion",
             _dataRepository.GetQuestion(
             answerPostRequest.QuestionId.Value));

            return savedAnswer;
        }
    }
}
