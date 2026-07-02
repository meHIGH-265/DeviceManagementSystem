import { Injectable } from '@angular/core';

export interface AppConfig {
  apiUrl: string;
}

export interface AppSecrets {
  applicationName: string;
}

@Injectable({ providedIn: 'root' })
export class RuntimeConfigService {
  private config!: AppConfig;
  private secrets!: AppSecrets;

  setConfig(config: AppConfig) {
    this.config = config;
  }

  setSecrets(secrets: AppSecrets) {
    this.secrets = secrets;
  }

  get apiUrl(): string {
    if (!this.config) {
      throw new Error('Runtime config not loaded');
    }
    return this.config.apiUrl;
  }

  get aplicationName(): string {
    if (!this.secrets) {
      throw new Error('Runtime secrets not loaded');
    }
    return this.secrets.applicationName;
  }
}
