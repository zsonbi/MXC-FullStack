// src/app/components/login/login.component.ts

import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required], 
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    // A Swagger LoginRequest userName mezőt vár
    const request = {
      userName: this.loginForm.value.username,
      password: this.loginForm.value.password,
      rememberMe: false
    };

    this.authService.login(request).subscribe({
      next: () => {
        this.router.navigate(['/events']);
      },
      error: (err) => {
        this.errorMessage = 'Login failed. Please check credentials.';
      }
    });
  }
}