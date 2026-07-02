import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth-interceptor';

// Angular Material modules
import { importProvidersFrom } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { APP_INITIALIZER } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { RuntimeConfigService, AppConfig, AppSecrets } from './services/runtime-config.service';

function loadAppConfig(
  http: HttpClient,
  configService: RuntimeConfigService
) {
  return async () => {
    const config = await firstValueFrom(
      http.get<AppConfig>('/assets/config/config.json')
    );

    const secrets = await firstValueFrom(
      http.get<AppSecrets>('/assets/secrets/app-name.json')
    );

    configService.setConfig(config);
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),

    // 👇 Register your interceptor globally
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },

    {
      provide: APP_INITIALIZER,
      useFactory: loadAppConfig,
      deps: [HttpClient, RuntimeConfigService],
      multi: true
    },

    importProvidersFrom(
      MatDialogModule,
      MatButtonModule,
      MatTableModule,
      MatIconModule,
      MatInputModule,
      MatFormFieldModule
    )
  ]
};
