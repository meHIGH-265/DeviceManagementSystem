import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { DeviceListComponent } from './devices/device-list/device-list';
import { AuthGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'devices', component: DeviceListComponent, canActivate: [AuthGuard] }
];
