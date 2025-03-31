import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Teacher } from '../models/teacher';
import { Grade } from '../models/grade';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  baseUrl = 'https://localhost:7261/teacher';
  baseUrlGrade='https://localhost:7261/grade'


  constructor(private httpClient: HttpClient) { }

  public registerTeacher(username: string, password: string, email: string): Observable<boolean> {
    const teacher: Teacher = {
      id: '',
      username: username,
      password: password,
      email: email,
      courseIds:[]
    };
    return this.httpClient.post<boolean>(this.baseUrl, teacher);
  }
  public addGrade(grade:Grade)
  {
    console.log(grade);
    return this.httpClient.post<boolean>(`${this.baseUrlGrade}/add-grade`,grade);
  }
}
