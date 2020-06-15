import { QuestionData } from './QuestionsData';
import { Action } from 'redux';

type QuestionsActions =
  | GettingUnansweredQuestionsAction
  | GotUnansweredQuestionsAction
  | PostedQuestionAction;

interface QuestionsState {
  readonly loading: boolean;
  readonly unanswered: QuestionData[] | null;
  readonly postedResult?: QuestionData;
}

export interface AppState {
  readonly questions: QuestionsState;
}

const initialQuestionState: QuestionsState = {
  loading: false,
  unanswered: null,
};

interface GettingUnansweredQuestionsAction
  extends Action<'GettingUnansweredQuestions'> {}

export interface GotUnansweredQuestionsAction
  extends Action<'GotUnansweredQuestions'> {
  questions: QuestionData[];
}
export interface PostedQuestionAction extends Action<'PostedQuestion'> {
  result: QuestionData | undefined;
}
