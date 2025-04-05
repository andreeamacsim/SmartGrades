import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Grade } from '../models/grade';
import { HttpClient } from '@angular/common/http';
import { DashboardSummaryDto } from '../models/Dto/DashboardSummaryDto';

@Injectable({
  providedIn: 'root'
})
export class GradeService {
  baseUrl='https://localhost:7261/grade'

  constructor(private httpClient:HttpClient) { }

  getStudentGrades(studentId: string): Observable<Grade[]> {
    return this.httpClient.get<Grade[]>(`${this.baseUrl}/id?id=${studentId}`);
  }

  getStudentDashboard(studentId: string): Observable<DashboardSummaryDto> {
    return this.httpClient.get<DashboardSummaryDto>(`${this.baseUrl}/dashboard/${studentId}`);
  }
}