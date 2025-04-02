import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Grade } from '../models/grade';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GradeService {
  baseUrl='https://localhost:7261/grade'

  constructor(private httpClient:HttpClient) { }

  getStudentGrades(userId:string): Observable<Grade[]> {
    return this.httpClient.get<Grade[]>(`${this.baseUrl}/id?id=${userId}`);
  }
}