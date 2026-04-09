import { Component, Inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DeviceService } from '../../services/device.service';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Device } from '../../models/device.model';

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
  templateUrl: './device-form.html',
  styleUrl: './device-form.css'
})
export class DeviceFormComponent {

  mode: 'view' | 'edit' | 'create' | 'create_unique';

  form;
  edited_device: Device | null = null;

  constructor(
    private fb: FormBuilder,
    private deviceService: DeviceService,
    private dialogRef: MatDialogRef<DeviceFormComponent>,
    private cdr: ChangeDetectorRef,
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
      this.edited_device = data.device;
    }

    if (this.mode === 'view') {
      this.form.disable();
    }
  }

  canGenerateAi(): boolean {
    if (this.mode === 'view') return false;

    const requiredFields = [
      'name',
      'manufacturer',
      'type',
      'os',
      'osVersion',
      'processor',
      'ram'
    ];

    return requiredFields.every(field => this.form.get(field)?.valid);
  }

  generateAiDescription() {
    if (!this.canGenerateAi()) {
      this.form.markAllAsTouched();
      return;
    }

    this.form.patchValue({
      description: 'generating ai description...'
    });
    this.cdr.detectChanges();

    const device = this.form.getRawValue();

    this.deviceService.generateDescription(device).subscribe(res => {
      this.form.patchValue({
        description: res.description
      });
    });
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
      const updatedDevice: Device = {
        ...this.edited_device,
        ...device
      };

      this.deviceService.update(updatedDevice).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  close() {
    this.dialogRef.close();
  }
}
