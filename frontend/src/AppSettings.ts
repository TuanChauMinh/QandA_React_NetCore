export const server = 'https://localhost:44398';

export const webAPIUrl = `${server}/api`;

export const authSettings = {
  domain: 'tuan-test.au.auth0.com',
  client_id: 'xmsN7gHgSNJWrVe2tHxyLz0eKR3UnFed',
  redirect_uri: window.location.origin + '/signin-callback',
  scope: 'openid profile QandAAPI email',
  audience: 'https://qanda',
};
