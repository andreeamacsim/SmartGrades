import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { TeacherService } from '../services/teacher.service';
import { Teacher } from '../models/teacher';

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
  teahcerId:string=''
  constructor(private router: Router,private authService:AuthService,private teacherService:TeacherService) { }

  ngOnInit(): void {
    this.authService.connectedUser$.subscribe(teacher=>{
      if(teacher){
      this.teahcerId=teacher.id;
      this.username=teacher.username;
      this.email=teacher.email;
      this.password=teacher.password;
      this.confirmPassword=teacher.password;
      }
    })
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
    const teacher=<Teacher>{
      id:this.teahcerId,
      username:this.username,
      email:this.email,
      password:this.password,
      courseIds:[]
    }
    this.teacherService.updateTeacher(teacher).subscribe({
      next: (value)=>{
        if(value)
        {
          setTimeout(() => {
            this.isLoading = false;
            this.successMessage = 'Profile updated successfully';
          }, 1500);
        }
        else{
          setTimeout(() => {
            this.isLoading = false;
            this.errorMessage = 'Profile updated failed';
          }, 1000);
        }
      },
      error:()=>{
        setTimeout(() => {
          this.isLoading = false;
          this.errorMessage = 'Profile updated failed';
        }, 1500);
      }
    })
  }

  navigateToDashboard(): void {
    this.router.navigate(['/teacher-dashboard']);
  }
}