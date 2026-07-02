import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';
import { AuthResponse } from '../models/auth-response.model';
import { RuntimeConfigService } from './runtime-config.service';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private domain = 'user';

  constructor(
    private http: HttpClient,
    private config: RuntimeConfigService
  ) {}

  getApiUrl(): string {
    return `${this.config.apiUrl}/${this.domain}`;
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.getApiUrl()}/login`, {
      email: email,
      passwordHash: password
    });
  }

  setSession(authResult: AuthResponse) {
    localStorage.setItem('token', authResult.token);
    localStorage.setItem('user', JSON.stringify(authResult.user));
  }

  getUserByEmail(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.getApiUrl()}/by_email/${email}`);
  }

  register(user: any) {
    return this.http.post(this.getApiUrl(), user);
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
