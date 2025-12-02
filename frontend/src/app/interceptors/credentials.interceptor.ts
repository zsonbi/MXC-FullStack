import { HttpInterceptorFn } from '@angular/common/http';
//This is used to make it sure it uses the cookie auth
export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    withCredentials: true
  });

  return next(clonedRequest);
};