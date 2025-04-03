import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Student } from '../models/student';
import { Teacher } from '../models/teacher';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { StudentService } from './student.service';
import { TeacherService } from './teacher.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userSubject = new BehaviorSubject<Student|Teacher|null>(null);
  connectedUser$ = this.userSubject.asObservable()
  baseUrlTeacher = 'https://localhost:7261/teacher/authenticate';
  baseUrlStudent = 'https://localhost:7261/student/authenticate';
  connectedUserId='';
  constructor(private httpClient: HttpClient,private router:Router,private studentService:StudentService,private teacherService:TeacherService) {

    let token: string = this.getToken();
    if (token != null) {
      this.connectedUserId = this.decodedToken().nameid;
      let role=this.decodedToken().role;
      if(role=="student")
        this.studentService.getStudentById(this.connectedUserId).subscribe((data) => this.userSubject.next(data))
      if(role=="teacher")
        this.teacherService.getTeacherById(this.connectedUserId).subscribe((data)=>this.userSubject.next(data))
    }
    console.log(this.connectedUserId);
   }

  login(username: string, password: string, userType: string): Observable<any> {
    const requestBody = { username, password };
    const url = userType === 'student' ? this.baseUrlStudent : this.baseUrlTeacher;
    return this.httpClient.post<any>(url, requestBody);
  }

  logout(): void {
    this.userSubject.next(null);
    localStorage.clear();
    this.router.navigate(['login']);
  }
  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue); 
  }

  getToken() {
    return localStorage.getItem('token'); 
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); 
  }

  decodedToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken();
    return jwtHelper.decodeToken(token);
  }

  setUserConnected(user: Student | Teacher) {
    this.userSubject.next(user);
  }
}
