import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common'; 
import { AuthService } from '../services/auth.service';
import { StudentService } from '../services/student.service';
import { TeacherService } from '../services/teacher.service';

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

  constructor(private router: Router ,private authService:AuthService,private studentService:StudentService,private teacherService:TeacherService) {
    let token: string = this.authService.getToken();
    if (token != null) {
      let payloadId = this.authService.decodedToken().nameid;
      let role=this.authService.decodedToken().role;
      if(role=="student")
        this.studentService.getStudentById(payloadId).subscribe((data) => this.authService.setUserConnected(data))
      if(role=="teacher")
        this.teacherService.getTeacherById(payloadId).subscribe((data)=>this.authService.setUserConnected(data))
      this.successMessage = 'Login successful! Redirecting...';
      this.authService.connectedUserId=payloadId;
      setTimeout(() => {
        if (role=="teacher") {
          this.router.navigate(['/teacher-dashboard']);
        } else {
          this.router.navigate(['/student-dashboard']);
        }
      }, 2000)
    }
  }
  onLogin() {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.username || !this.password) {
      this.errorMessage = 'Please enter username and password';
      return;
    }

    this.authService.login(this.username, this.password, this.userType).subscribe({
      next: (res) => {
        this.authService.storeToken(res.token);
        let tokenContent=this.authService.decodedToken();
        if(tokenContent.role=="student")
        {
          this.studentService.getStudentById(tokenContent.nameid).subscribe(student=>{
            this.authService.setUserConnected(student);
          })
        }
        if(tokenContent.role=="teacher")
        {
          this.teacherService.getTeacherById(tokenContent.nameid).subscribe(teacher=>{
            this.authService.setUserConnected(teacher);
          })
        }
        this.successMessage = 'Login successful! Redirecting...';
        this.authService.connectedUserId=tokenContent.nameid;
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
