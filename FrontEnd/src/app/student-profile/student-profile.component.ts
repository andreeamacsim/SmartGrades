import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student';
import { Grade } from '../models/grade';
import { Course } from '../models/course';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-profile.component.html',
  styleUrl: './student-profile.component.css'
})
export class StudentProfileComponent implements OnInit {
  username: string = '';
  email: string = '';
  studentId: string = '';
  password: string = '';
  confirmPassword: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  grades: Grade[];
  courses: string[];
  constructor(private router: Router, private authService: AuthService, private studentService: StudentService) { }

  ngOnInit(): void {
    this.studentId = this.authService.connectedUserId;
    this.authService.connectedUser$.subscribe(student => {
      if(student){
      this.username = student.username;
      this.email = student.email;
      this.password = student.password;
      this.confirmPassword = student.password;
      }
    })
  }

  onUpdate() {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    const newStudent = <Student>{
      id: this.studentId,
      username: this.username,
      password: this.password,
      email: this.email,
      grades: [],
      courseIds: []
    }
    this.studentService.updateStudent(newStudent).subscribe({
      next: (value) => {
        if (value) {
          setTimeout(() => {
            this.isLoading = false;
            this.successMessage = 'Profile updated successfully!';
          }, 1500);
        }
        else {
          setTimeout(() => {
            this.isLoading = false;
            this.errorMessage = 'Profile updated failed!';
          }, 1500);
        }
      },
      error:()=>{
        setTimeout(() => {
          this.isLoading = false;
          this.errorMessage = 'Profile updated failed!';
        }, 1500);
      }
    });
  }

  navigateToDashboard() {
    this.router.navigate(['/student-dashboard']);
  }
}