import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common'; 

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

  constructor(private router: Router) {}

  onLogin() {
    if (this.username && this.password) {
      console.log('Login attempt:', this.username);
      this.successMessage = 'Login successful!';
      this.errorMessage = ''; 
    } else {
      this.errorMessage = 'Please enter username and password';
      this.successMessage = ''; 
    }
  }
}
