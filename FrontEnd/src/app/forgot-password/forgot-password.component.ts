import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  email: string = '';
  errorMessage: string = '';
  successMessage: string = '';
  userType: 'student' | 'teacher' = 'student';

  constructor(private router: Router ,private studentService:StudentService) {}

  onResetPassword() {
    this.errorMessage = '';
    this.successMessage = '';
  
    if (!this.email) {
      this.errorMessage = 'Please enter your email address.';
      return;
    }
  
    this.studentService.sendResetEmail(this.email).subscribe({
      next: () => {
        this.successMessage = 'Email sent successfully! Please check your inbox.';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to send reset email. Please try again.';
      }
    });
  }

  toggleUserType(type: 'student' | 'teacher') {
    this.userType = type;
  }
}
