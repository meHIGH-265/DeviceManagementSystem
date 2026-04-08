import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Router, RouterModule } from '@angular/router';

import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { UserDialogComponent } from '../../user/user-dialog/user-dialog';
import { DeviceFormComponent } from '../device-form/device-form';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    FormsModule,
    MatFormFieldModule,
    RouterModule,
    MatInputModule
  ],
  templateUrl: './device-list.html'
})
export class DeviceListComponent implements OnInit {

  devices: Device[] = [];
  userMap: { [key: number]: User } = {};
  displayedColumns = ['name', 'manufacturer', 'user', 'actions'];
  currentUser: User | null;
  searchQuery: string = '';

  constructor(
    private deviceService: DeviceService,
    private userService: UserService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    private auth: AuthService,
    private router: Router
  ) {
    this.currentUser = this.auth.getUser();
  }

  ngOnInit() {
    this.loadDevices();
    this.loadUsers();
  }

  onSearchChange() {
    this.loadDevices();
  }

  loadDevices() {
    if (!this.searchQuery || this.searchQuery.trim() === '') {
      this.deviceService.getAll().subscribe(data => {
        this.devices = data;
        this.cdr.detectChanges();
      });
    } else {
      this.deviceService.search(this.searchQuery).subscribe(results => {

        this.devices = results
          .filter(r => r.score > 0)
          .map(r => r.device);

        this.cdr.detectChanges();
      });
    }
  }

  loadUsers() {
    this.userService.getAll().subscribe(users => {
      users.forEach(u => this.userMap[u.id] = u);
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
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Delete this device?' }
    });

    ref.afterClosed().subscribe(result => {
      if (result) {
        this.deviceService.delete(device.id!).subscribe(() => {
          this.loadDevices();
        });
      }
    });
  }

  getUserName(device: Device): string {
    if (!device.assignedUserId) return 'Unassigned';
    return this.userMap[device.assignedUserId]?.name || 'Loading...';
  }

  assignDevice(device: Device) {
    device.assignedUserId = this.currentUser!.id;

    this.deviceService.update(device).subscribe(() => {
      this.loadDevices();
    });
  }

  openAssignDialog(device: Device) {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Assign this device to yourself?' }
    });

    ref.afterClosed().subscribe(result => {
      if (result) this.assignDevice(device);
    });
  }

  unassignDevice(device: Device) {
    device.assignedUserId = null;

    this.deviceService.update(device).subscribe(() => {
      this.loadDevices();
    });
  }

  openUnassignDialog(device: Device) {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Unassign this device?' }
    });

    ref.afterClosed().subscribe(result => {
      if (result) this.unassignDevice(device);
    });
  }

  openUserViewDialog(user: any) {
    this.dialog.open(UserDialogComponent, {
      width: '400px',
      data: user
    });
  }

  handleUserClick(device: Device) {

    if (!device.assignedUserId) {
      this.openAssignDialog(device);
    }
    else if (device.assignedUserId === this.currentUser?.id) {
      this.openUnassignDialog(device);
    }
    else {
      const user = this.userMap[device.assignedUserId];

      if (user) {
        this.openUserViewDialog(user);
      }
    }
  }

  logout() {
    this.router.navigate(['']);
    this.auth.setSession({token: '', user: {id: 0, name: '', email: '', role: '', location: ''}});
  }
}
