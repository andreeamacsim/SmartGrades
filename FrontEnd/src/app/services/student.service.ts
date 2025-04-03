import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Student } from '../models/student';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  baseUrl="https://localhost:7261/student"
  constructor(private httpClient:HttpClient) { }

  public registerStudent(username:string,password:string,email:string) :Observable<boolean>
  {
    const student=<Student>{
      id:'',
      username:username,
      password:password,
      email:email,
      courseIds:[],
      grades:[]
    }
    return this.httpClient.post<boolean>(this.baseUrl,student);
  }
  public getStudentById(id:string){
    return this.httpClient.get<Student>(`${this.baseUrl}/id?id=${id}`);
  }
}
