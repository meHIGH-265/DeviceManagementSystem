import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DeviceService } from '../../services/device.service';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './device-form.html'
})
export class DeviceFormComponent {

  mode: 'view' | 'edit' | 'create' | 'create_unique';

  form;

  constructor(
    private fb: FormBuilder,
    private deviceService: DeviceService,
    private dialogRef: MatDialogRef<DeviceFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.mode = data.mode;

    this.form = this.fb.nonNullable.group({
      id: [],
      name: ['', Validators.required],
      manufacturer: ['', Validators.required],
      type: ['', Validators.required],
      os: ['', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ram: ['', Validators.required],
      description: ['', Validators.required]
    });

    if (data.device) {
      this.form.patchValue(data.device);
    }

    if (this.mode === 'view') {
      this.form.disable();
    }
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const device = this.form.getRawValue();

    if (this.mode === 'create') {
      this.deviceService.create(device).subscribe(() => {
        this.dialogRef.close(true);
      });
    }

    if (this.mode === 'create_unique') {
      this.deviceService.getAll().subscribe(devices => {
        const exists = devices.some(d =>
          d.name === device.name &&
          d.manufacturer === device.manufacturer &&
          d.os === device.os &&
          d.ram === device.ram &&
          d.processor === device.processor
        );

        if (exists) {
          alert('Device already exists!');
          return;
        }

        this.deviceService.create(device).subscribe(() => {
          this.dialogRef.close(true);
        });
      });
    }

    if (this.mode === 'edit') {
      this.deviceService.update(device).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  close() {
    this.dialogRef.close();
  }
}
