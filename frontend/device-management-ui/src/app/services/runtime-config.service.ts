import { Injectable } from '@angular/core';

export interface AppConfig {
  apiUrl: string;
}

@Injectable({ providedIn: 'root' })
export class RuntimeConfigService {
  private config!: AppConfig;

  setConfig(config: AppConfig) {
    this.config = config;
  }

  get apiUrl(): string {
    if (!this.config) {
      throw new Error('Runtime config not loaded');
    }
    return this.config.apiUrl;
  }
}
