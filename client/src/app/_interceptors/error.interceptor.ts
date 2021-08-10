import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(err => {
        if(err) {
          switch (err.status)
          {
            case 400:
              if(err.error.errors) {
                const modalStateErrors = [];
                for(const key in modalStateErrors){
                  if(err.error.errors[key]){
                    modalStateErrors.push(err.error.errors[key])
                  }
                }
                throw modalStateErrors;
              } else {
                this.toastr.error(err.statusText, err.status);
              }
              break;
            
            case 401:
              this.toastr.error(err.statusText, err.status);
              break;
            
            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              const navigationExtras : NavigationExtras = {state: {error: err.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;

            default:
              this.toastr.error("Something wrong is not right");
              console.log(err)
              break;
          }
        }

        return throwError(err);
      })
    );
  }
}
