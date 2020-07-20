import { webAPIUrl } from './AppSettings';
import { rejects } from 'assert';
export interface HttpRequest<REQB> {
  path: string;
}
export interface HttpResponse<RESB> extends Response {
  parsedBody?: RESB;
}

export const http = <REQB, RESB>(
  config: HttpRequest<REQB>,
): Promise<HttpResponse<RESB>> => {
  return new Promise((resolve, reject) => {
    const request = new Request(`${webAPIUrl}${config.path}`);
    let response: HttpResponse<RESB>;
    fetch(request)
      .then((res) => {
        response = res;
        return res.json();
      })
      .then((body) => {
        if (response.ok) {
          response.parsedBody = body;
          resolve(response);
        } else {
          reject(response);
        }
      })
      .catch((err) => {
        console.error(err);
        reject(err);
      });
  });
};
