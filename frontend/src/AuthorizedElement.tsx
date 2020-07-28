import React, { FC, Fragment } from 'react';
import { useAuth } from './Auth';
import { authSettings } from './AppSettings';

export const AuthorizedElement: FC = ({ children }) => {
  const auth = useAuth();
  if (auth.isAuthenticated) {
    return <Fragment>{children}</Fragment>;
  } else {
    return null;
  }
};
