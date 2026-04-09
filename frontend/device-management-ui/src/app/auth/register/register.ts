import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {

  form;
  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      email: ['', Validators.required],
      role: ['', Validators.required],
      location: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  register() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { password, confirmPassword } = this.form.getRawValue();

    if (password !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }

    this.auth.getUserByEmail(this.form.value.email!).subscribe((email_exists) => {
      if (email_exists) {
        alert('There already is an account made for this email');
        return;
      }
      const user = {
        name: this.form.value.name,
        email: this.form.value.email,
        role: this.form.value.role,
        location: this.form.value.location,
        passwordHash: password
      };

      this.auth.register(user).subscribe(() => {
        alert('Account created successfully');
        this.router.navigate(['/']);
      });
    });
  }
}