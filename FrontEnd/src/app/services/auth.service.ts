import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Student } from '../models/student';
import { Teacher } from '../models/teacher';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  authenticatedUser: Student | Teacher | null = null;
  baseUrlTeacher = 'https://localhost:7261/teacher/authenticate';
  baseUrlStudent = 'https://localhost:7261/student/authenticate';

  constructor(private httpClient: HttpClient) { }

  login(username: string, password: string, userType: string): Observable<Student | Teacher> {
    const requestBody = { username, password };
    const url = userType === 'student' ? this.baseUrlStudent : this.baseUrlTeacher;
    return this.httpClient.post<Student | Teacher>(url, requestBody);
  }

  setAuthenticatedUser(user: Student | Teacher): void {
    this.authenticatedUser = user;
  }

  getAuthenticatedUser(): Student | Teacher | null {
    return this.authenticatedUser;
  }

  logout(): void {
    this.authenticatedUser = null;
  }
}
