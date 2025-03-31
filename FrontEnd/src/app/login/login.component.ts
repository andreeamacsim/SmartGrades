import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common'; 
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule], 
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  successMessage: string = '';
  userType: 'student' | 'teacher' = 'student';

  constructor(private router: Router ,private authService:AuthService) {}

  onLogin() {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.username || !this.password) {
      this.errorMessage = 'Please enter username and password';
      return;
    }

    this.authService.login(this.username, this.password, this.userType).subscribe({
      next: (user) => {
        this.authService.setAuthenticatedUser(user);
        this.successMessage = 'Login successful! Redirecting...';
        setTimeout(() => {
          if (this.userType === 'teacher') {
            this.router.navigate(['/teacher-dashboard']);
          } else {
            this.router.navigate(['/student-dashboard']);
          }
        }, 2000);
      },
      error: () => {
        this.errorMessage = 'Invalid username or password. Please try again.';
      }
    });
  }
  toggleUserType(type: 'student' | 'teacher') {
    this.userType = type;
  }
}
