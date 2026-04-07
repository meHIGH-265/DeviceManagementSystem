import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';
import { AuthResponse } from '../models/auth-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'https://localhost:7144/api/user';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, {
      email: email,
      passwordHash: password
    });
  }

  setSession(authResult: AuthResponse) {
    localStorage.setItem('token', authResult.token);
    localStorage.setItem('user', JSON.stringify(authResult.user));
  }

  register(user: any) {
    return this.http.post(`${this.apiUrl}`, user);
  }

  setUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser(): User | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('user');
  }
}
