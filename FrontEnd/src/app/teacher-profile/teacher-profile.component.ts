import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-teacher-profile',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule, RouterModule, FormsModule],
  templateUrl: './teacher-profile.component.html',
  styleUrl: './teacher-profile.component.css'
})
export class TeacherProfileComponent implements OnInit {
  username: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  userType: string = 'teacher';

  constructor(private router: Router) { }

  ngOnInit(): void {
    // Initialized with mock data 
    this.username = 'teacheruser';
    this.email = 'teacher@example.com';
  }

  toggleUserType(type: string): void {
    this.userType = type;
  }

  onUpdate(): void {
    this.isLoading = true;
    
   
    if (!this.username || !this.email) {
      this.errorMessage = 'Please fill in all required fields';
      this.isLoading = false;
      return;
    }
    
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      this.isLoading = false;
      return;
    }

    // simulated update profile
    setTimeout(() => {
      this.successMessage = 'Profile updated successfully';
      this.errorMessage = '';
      this.isLoading = false;
      
      this.password = '';
      this.confirmPassword = '';
    }, 1000);
  }

  navigateToDashboard(): void {
    this.router.navigate(['/teacher-dashboard']);
  }
}