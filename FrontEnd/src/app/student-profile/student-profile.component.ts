import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

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

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Load student data here from our server
 
  }

  onUpdate() {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }
    
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    

    
    // Simulate API call
    setTimeout(() => {
      this.isLoading = false;
      this.successMessage = 'Profile updated successfully!';
    }, 1500);
  }

  navigateToDashboard() {
    this.router.navigate(['/student-dashboard']);
  }
}