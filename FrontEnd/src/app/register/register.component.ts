import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Student } from '../models/student';
import { StudentService } from '../services/student.service';
import { TeacherService } from '../services/teacher.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  userType: 'student' | 'teacher' = 'student';
  errorMessage: string = '';
  registerMessage: string = '';
  isLoading: boolean = false;

  constructor(private router: Router, private studentService: StudentService, private teacherService: TeacherService) { }

  onRegister() {

    this.errorMessage = '';
    this.registerMessage = '';


    if (!this.username || !this.email || !this.password || !this.confirmPassword) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }


    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.email)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }


    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    if (this.password.length < 8) {
      this.errorMessage = 'Password must be at least 8 characters long';
      return;
    }
    if (this.userType === 'student') {
      this.studentService.registerStudent(this.username, this.password, this.email).subscribe({
        next: (results) => {
          if (results) {
            this.registerMessage = `Student account created successfully for ${this.username}! Redirecting to login...`;
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 2000);
          } else {
            this.errorMessage = 'Registration failed. Please try again.';
          }
        },
        error: () => {
          this.errorMessage = 'An error occurred during registration. Please try again later.';
        }
      });
    } else if (this.userType === 'teacher') {
      this.teacherService.registerTeacher(this.username, this.password, this.email).subscribe({
        next: (results) => {
          if (results) {
            this.registerMessage = `Teacher account created successfully for ${this.username}! Redirecting to login...`;
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 2000);
          } else {
            this.errorMessage = 'Registration failed. Please try again.';
          }
        },
        error: () => {
          this.errorMessage = 'An error occurred during registration. Please try again later.';
        }
      });
    }

  }


  toggleUserType(type: 'student' | 'teacher') {
    this.userType = type;
  }
}