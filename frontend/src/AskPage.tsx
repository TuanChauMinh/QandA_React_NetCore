import React from 'react';
import { Page } from './Page';
import { Form, required, minLength, Values } from './Form';
import { Field } from './Field';
import { postQuestion } from './QuestionsData';

export const AskPage = () => {
  const handleSubmit = async (values: Values) => {
    const question = await postQuestion({
      title: values.title,
      content: values.content,
      userName: 'Fred',
      created: new Date(),
    });
    return { success: question ? true : false };
  };
  return (
    <Page title="Ask a question">
      <Form
        submitCaption="Submit your Question"
        onSubmit={handleSubmit}
        failureMessage="There was a problem with your question"
        successMessage="Your question was successfully submitted"
        validationRules={{
          title: [{ validator: required }, { validator: minLength, arg: 10 }],
          content: [{ validator: required }, { validator: minLength, arg: 50 }],
        }}
      >
        <Field name="title" label="Title"></Field>
        <Field name="content" label="Content" type="TextArea"></Field>
      </Form>
    </Page>
  );
};
export default AskPage;
