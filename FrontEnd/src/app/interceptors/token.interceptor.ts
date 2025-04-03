import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const router=inject(Router);
  const myToken=localStorage.getItem('token');

  if(myToken)
  {
    req = req.clone({
      setHeaders: {
         Authorization: `Bearer ${myToken}`
      }
    });
  }
  else{
    req = req.clone();
  }
  return next(req).pipe(
    catchError((err)=>{
      if(err instanceof HttpErrorResponse)
      {
        if(err.status===401){
          router.navigate(['login']);
        }
      }
      return throwError(()=> new Error("Error occured"));
    })
  );
}