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

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),

    provideHttpClient(withInterceptorsFromDi()),

    // 👇 Register your interceptor globally
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
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
