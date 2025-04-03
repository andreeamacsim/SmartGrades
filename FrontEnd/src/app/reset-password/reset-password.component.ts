import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StudentService } from '../services/student.service';
import { TeacherService } from '../services/teacher.service';
import { ResetPassword } from '../models/reset-password'; 

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  email: string = '';
  emailToken: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';
  successMessage: string = '';
  userType: 'student' | 'teacher' = 'student';

  constructor(
    private router: Router,
    private studentService: StudentService,
    private teacherService: TeacherService,
    private activatedRoute: ActivatedRoute 
  ) {}

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.email = params['email'] || ''; 
      this.emailToken = params['code'] || ''; 
    });
  }

  onResetPassword() {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.newPassword || !this.confirmPassword) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    const resetPasswordObj: ResetPassword = {
      email: this.email,
      emailToken: this.emailToken,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    };

    const resetPasswordObservable = this.userType === 'teacher' 
      ? this.teacherService.resetPassword(resetPasswordObj) 
      : this.studentService.resetPassword(resetPasswordObj);

    resetPasswordObservable.subscribe({
      next: () => {
        this.successMessage = 'Password reset successful! Redirecting to login...';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: () => {
        this.errorMessage = 'Error resetting password. Please try again.';
      }
    });
  }

  toggleUserType(type: 'student' | 'teacher') {
    this.userType = type;
  }
}
