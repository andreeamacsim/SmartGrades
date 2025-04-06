import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Student } from '../models/student';
import { Observable } from 'rxjs';
import { ResetPassword } from '../models/reset-password';
import { ResetPasswordResponse } from '../models/reset-password-response';

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
  public updateStudent(student:Student)
  {
    return this.httpClient.put<boolean>(`${this.baseUrl}`,student);
  }
  public sendResetEmail(email: string): Observable<any> {
    const url = `${this.baseUrl}/send-reset-email/${email}`;
    return this.httpClient.post(url, {});
  }

  public resetPassword(resetPasswordObj: ResetPassword): Observable<ResetPasswordResponse> {
    const url = `${this.baseUrl}/reset-password`;
    return this.httpClient.post<ResetPasswordResponse>(url, resetPasswordObj);
  }

  getAllStudents(): Observable<Student[]> {
    const url = `${this.baseUrl}/all`;
    return this.httpClient.get<Student[]>(url);
  }
}
