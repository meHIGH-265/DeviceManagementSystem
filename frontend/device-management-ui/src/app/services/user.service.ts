import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';
import { RuntimeConfigService } from './runtime-config.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  private domain = 'user';

  constructor(
    private http: HttpClient,
    private config: RuntimeConfigService
  ) {}

  getApiUrl(): string {
    return `${this.config.apiUrl}/${this.domain}`;
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.getApiUrl());
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.getApiUrl()}/${id}`);
  }

  create(user: User) {
    console.info(user)
    return this.http.post(this.getApiUrl(), user);
  }

  update(user: User) {
    return this.http.put(`${this.getApiUrl()}/${user.id}`, user);
  }

  delete(id: number) {
    return this.http.delete(`${this.getApiUrl()}/${id}`);
  }
}
