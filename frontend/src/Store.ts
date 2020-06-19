import {
  QuestionData,
  getUnansweredQuestions,
  postQuestion,
  PostQuestionData,
} from './QuestionsData';
import { Action, ActionCreator, Dispatch } from 'redux';
import { ThunkAction } from 'redux-thunk';
// Creating the state
interface QuestionsState {
  readonly loading: boolean;
  readonly unanswered: QuestionData[] | null;
  readonly postedResult?: QuestionData;
}
// Initial State
const initialQuestionState: QuestionsState = {
  loading: false,
  unanswered: null,
};
export interface AppState {
  readonly questions: QuestionsState;
}

interface GettingUnansweredQuestionsAction
  extends Action<'GettingUnansweredQuestions'> {}

export interface GotUnansweredQuestionsAction
  extends Action<'GotUnansweredQuestions'> {
  questions: QuestionData[];
}
export interface PostedQuestionAction extends Action<'PostedQuestion'> {
  result: QuestionData | undefined;
}

// Creating Action
type QuestionsActions =
  | GettingUnansweredQuestionsAction
  | GotUnansweredQuestionsAction
  | PostedQuestionAction;

// Create Action Creator
export const getUnansweredQuestionsActionCreator: ActionCreator<ThunkAction<
  Promise<void>,
  QuestionData[],
  null,
  GotUnansweredQuestionsAction
>> = () => {
  return async (dispatch: Dispatch) => {
    // TODO - dispatch the GettingUnansweredQuestionsAction action
    const gettingUnansweredQuestionsAction: GettingUnansweredQuestionsAction = {
      type: 'GettingUnansweredQuestions',
    };

    // TODO - get the questions from server
    dispatch(gettingUnansweredQuestionsAction);
    const questions = await getUnansweredQuestions();

    // TODO - dispatch the GotUnansweredQuestionsAction action

    const gotUnansweredQuestionAction: GotUnansweredQuestionsAction = {
      questions,
      type: 'GotUnansweredQuestions',
    };
    dispatch(gotUnansweredQuestionAction);
  };
};

export const postQuestionActionCreator: ActionCreator<ThunkAction<
  Promise<void>,
  QuestionData,
  PostQuestionData,
  PostedQuestionAction
>> = (question: PostQuestionData) => {
  return async (dispatch: Dispatch) => {
    const result = await postQuestion(question);
    const postedQuestionAction: PostedQuestionAction = {
      type: 'PostedQuestion',
      result,
    };
    dispatch(postedQuestionAction);
  };
};
