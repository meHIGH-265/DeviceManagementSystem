import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Device } from '../models/device.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private apiUrl = 'https://localhost:7144/api/device';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl);
  }

  getById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }

  create(device: Device) {
    return this.http.post(this.apiUrl, device);
  }

  update(device: Device) {
    return this.http.put(`${this.apiUrl}/${device.id}`, device);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  generateDescription(device: any) {
    return this.http.post<{ description: string }>(`${this.apiUrl}/generate-description`, device);
  }

  search(query: string): Observable<{ device: Device; score: number }[]> {
    return this.http.get<{ device: Device; score: number }[]>(
      `${this.apiUrl}/search/${encodeURIComponent(query)}`
    );
  }
}
