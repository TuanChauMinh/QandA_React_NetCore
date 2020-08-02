export const server =
  process.env.REACT_APP_ENV === 'production'
    ? '<host_production>'
    : process.env.REACT_APP_ENV === 'staging'
    ? '<host_staging>'
    : '<localhost>';

export const webAPIUrl = `${server}/api`;

export const authSettings = {
  domain: '<auth0_domain>',
  client_id: '<client_id>',
  redirect_uri: window.location.origin + '/signin-callback',
  scope: 'openid profile QandAAPI email',
  audience: '<audience>',
};
