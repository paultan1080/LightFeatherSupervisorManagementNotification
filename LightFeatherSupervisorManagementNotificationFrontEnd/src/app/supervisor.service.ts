import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { NotificationFormModel } from './notification-form.model';

@Injectable({
  providedIn: 'root',
})
export class SupervisorService {
  private readonly supervisorApiUrl = '/api/supervisors';
  private readonly submitUrl = `/api/submit`;

  constructor(private http: HttpClient) { }

  getSupervisors(): Observable<string[]> {
    return this.http.get<string[]>(this.supervisorApiUrl);
  }

  submitForm(formData: NotificationFormModel): Observable<any> {
    return this.http.post(this.submitUrl, formData).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 400) {
      if (error && error.error && error.error.errors && error.error.errors) {
        let errorMessage = "";

        for (const key of Object.keys(error.error.errors)) {
          errorMessage += `${key}: ${(error.error.errors[key] || []).join(',')}\n`
        }

        return throwError(() => new Error(errorMessage));
      }
      return throwError(() => new Error(error.error.errors));
    } else {
      console.log("Status: ", error.status);
      return throwError(() => new Error('An unexpected error occurred.'));
    }
  }
}
