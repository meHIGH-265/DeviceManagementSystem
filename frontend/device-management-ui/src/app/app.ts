import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DeviceListComponent } from './devices/device-list/device-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, DeviceListComponent],
  template: `<app-device-list></app-device-list>`
})
export class App {
  protected readonly title = signal('device-management-ui');
}
