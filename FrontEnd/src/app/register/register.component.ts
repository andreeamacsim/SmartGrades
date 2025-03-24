import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

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

  constructor(private router: Router) {}

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

    // Simulate registration process --- dummy simulation NEED TO BE CHANGED WITH BACKEND
    this.isLoading = true;
    this.registerMessage = `Creating ${this.userType} account for ${this.username}...`;

   
    setTimeout(() => {
      
      if (this.username.length >= 3) {
       
        this.registerMessage = `${this.userType.charAt(0).toUpperCase() + this.userType.slice(1)} account created successfully for ${this.username}! Redirecting to login...`;
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      } else {
        
        this.errorMessage = 'Registration failed. Please try again.';
        this.registerMessage = '';
      }

      this.isLoading = false;
    }, 2000); 
  }

 
  toggleUserType(type: 'student' | 'teacher') {
    this.userType = type;
  }
}