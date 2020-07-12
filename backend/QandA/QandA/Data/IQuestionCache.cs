using QandA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.Data
{
    public interface IQuestionCache
    {
        QuestionGetSingleResponse Get(int questiondId);
        void Remove(int quesitonId);
        void Set(QuestionGetSingleResponse question);

    }
}
