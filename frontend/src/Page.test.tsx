import React from 'react';
import { render, cleanup } from '@testing-library/react';
import { Page } from './Page';

test('When the page component is rendered, it should contain the correct title and content', () => {
  const { getByText } = render(
    <Page title="Title test">
      <span>Test Content</span>
    </Page>,
  );

  const title = getByText('Title test');
  expect(title).not.toBeNull();

  const content = getByText('Test content');
  expect(content).not.toBeNull();
});
