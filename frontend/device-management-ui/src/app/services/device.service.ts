import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Device } from '../models/device.model';
import { Observable } from 'rxjs';
import { RuntimeConfigService } from './runtime-config.service';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  
  private domain = 'device';

  constructor(
      private http: HttpClient,
      private config: RuntimeConfigService
    ) {}

  getApiUrl(): string {
    return `${this.config.apiUrl}/${this.domain}`;
  }

  getAll(): Observable<Device[]> {
    return this.http.get<Device[]>(this.getApiUrl());
  }

  getById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.getApiUrl()}/${id}`);
  }

  create(device: Device) {
    return this.http.post(this.getApiUrl(), device);
  }

  update(device: Device) {
    return this.http.put(`${this.getApiUrl()}/${device.id}`, device);
  }

  delete(id: number) {
    return this.http.delete(`${this.getApiUrl()}/${id}`);
  }

  generateDescription(device: any) {
    return this.http.post<{ description: string }>(`${this.getApiUrl()}/generate-description`, device);
  }

  search(query: string): Observable<{ device: Device; score: number }[]> {
    return this.http.get<{ device: Device; score: number }[]>(
      `${this.getApiUrl()}/search/${encodeURIComponent(query)}`
    );
  }
}
