import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DeviceFormComponent } from '../device-form/device-form';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule
  ],
  templateUrl: './device-list.html'
})
export class DeviceListComponent implements OnInit {

  devices: Device[] = [];
  displayedColumns = ['name', 'manufacturer', 'user', 'actions'];

  constructor(
    private deviceService: DeviceService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadDevices();
  }

  loadDevices() {
    this.deviceService.getAll().subscribe(data => {
      this.devices = data;
      this.cdr.detectChanges();
    });
  }

  openView(device: Device) {
    this.dialog.open(DeviceFormComponent, {
      width: '500px',
      data: { device, mode: 'view' }
    });
  }

  openEdit(device: Device) {
    const ref = this.dialog.open(DeviceFormComponent, {
      width: '500px',
      data: { device, mode: 'edit' }
    });

    ref.afterClosed().subscribe(() => this.loadDevices());
  }

  openCreate() {
    const ref = this.dialog.open(DeviceFormComponent, {
      width: '500px',
      data: { mode: 'create' }
    });

    ref.afterClosed().subscribe(() => this.loadDevices());
  }

  delete(device: Device) {
    const ref = this.dialog.open(ConfirmDialogComponent);

    ref.afterClosed().subscribe(result => {
      if (result) {
        this.deviceService.delete(device.id!).subscribe(() => {
          this.loadDevices();
        });
      }
    });
  }
}
