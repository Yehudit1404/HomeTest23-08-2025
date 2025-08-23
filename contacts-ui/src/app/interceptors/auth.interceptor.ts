import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const isWrite = ['POST', 'PUT', 'DELETE', 'PATCH'].includes(req.method.toUpperCase());
  if (isWrite) {
    req = req.clone({ setHeaders: { 'X-Api-Key': 'dev-secret' } });
  }
  return next(req);
};
