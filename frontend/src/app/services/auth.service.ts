import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginRequest, UserProfileDto } from '../models/api-models';
import { BehaviorSubject, Observable, catchError, finalize, map, of, tap } from 'rxjs'; 
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/User`;

  private loggedIn = new BehaviorSubject<boolean>(false);
  private currentUser = new BehaviorSubject<string>('');

  isLoggedIn$ = this.loggedIn.asObservable();
  currentUser$ = this.currentUser.asObservable();

  constructor(private http: HttpClient, private router: Router) {
  }

  checkAuthStatus(): Observable<boolean> {
    return this.http.get<UserProfileDto>(`${this.apiUrl}/LoggedIn`).pipe(
      tap((userData) => {
        this.loggedIn.next(true);
        const displayName = userData.email || userData.userName || 'Unknown User';
        this.currentUser.next(displayName);
      }),
      //Send back true to the guard
      map(() => true), 
      catchError(() => {
        this.loggedIn.next(false);
        this.currentUser.next('');
        //In case of error send back false to the guard
        return of(false); 
      })
    );
  }

  isAuthenticated(): boolean {
    return this.loggedIn.value;
  }

  login(credentials: LoginRequest) {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap(() => {
        // After login subscribe to auth state changes
        this.checkAuthStatus().subscribe();
      })
    );
  }

  logout() {
    this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      finalize(() => {
        this.clearLocalState();
      })
    ).subscribe();
  }

private clearLocalState() {
  this.loggedIn.next(false);
  this.currentUser.next('');
  this.router.navigate(['/login']);
}
}